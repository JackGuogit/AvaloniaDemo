using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ChatAI.Models;

namespace ChatAI.Services
{
    public class DeepseekApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public DeepseekApiClient(string apiKey, string baseUrl = "https://api.deepseek.com/v1")
        {
            _apiKey = apiKey;
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "DeepseekConsoleApp/1.0");
        }

        public async Task<ChatCompletionResponse?> CreateChatCompletionAsync(ChatCompletionRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

                Debug.WriteLine($"发送请求到Deepseek API: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/chat/completions", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"API请求失败: {response.StatusCode} - {errorContent}");
                    return null;
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"收到API响应: {responseJson}");

                var result = JsonSerializer.Deserialize<ChatCompletionResponse>(responseJson, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                });

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"API调用异常: {ex.Message}");
                return null;
            }
        }

        public async IAsyncEnumerable<ChatCompletionStreamResponse?> CreateChatCompletionStreamAsync(
            ChatCompletionRequest request, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            // 设置流式传输
            request.Stream = true;

            var streamReader = await CreateStreamReaderAsync(request, cancellationToken);
            if (streamReader == null)
                yield break;

            await foreach (var response in ProcessStreamAsync(streamReader, cancellationToken))
            {
                yield return response;
            }
        }

        private async Task<StreamReader?> CreateStreamReaderAsync(ChatCompletionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

                Debug.WriteLine($"发送流式请求到Deepseek API: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/chat/completions", content, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"API请求失败: {response.StatusCode} - {errorContent}");
                    response.Dispose();
                    return null;
                }

                var stream = await response.Content.ReadAsStreamAsync();
                return new StreamReader(stream);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"创建流式连接异常: {ex.Message}");
                return null;
            }
        }

        private async IAsyncEnumerable<ChatCompletionStreamResponse?> ProcessStreamAsync(
            StreamReader reader, 
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            try
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // SSE格式：data: {...}
                    if (line.StartsWith("data: "))
                    {
                        var jsonData = line.Substring(6); // 移除 "data: " 前缀

                        // 检查是否是结束标记
                        if (jsonData.Trim() == "[DONE]")
                        {
                            Debug.WriteLine("流式传输完成");
                            break;
                        }

                        var streamResponse = TryParseStreamResponse(jsonData);
                        if (streamResponse != null)
                        {
                            Debug.WriteLine($"收到流式数据: {jsonData}");
                            yield return streamResponse;
                        }
                    }
                }
            }
            finally
            {
                reader?.Dispose();
            }
        }

        private ChatCompletionStreamResponse? TryParseStreamResponse(string jsonData)
        {
            try
            {
                return JsonSerializer.Deserialize<ChatCompletionStreamResponse>(jsonData, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                });
            }
            catch (JsonException ex)
            {
                Debug.WriteLine($"解析流式响应JSON失败: {ex.Message}, 数据: {jsonData}");
                return null;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}