using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;
using ChatAI.Models;

namespace ChatAI.Services
{
    public class OpenAIApiClient
    {
        private readonly OpenAIClient _client;
        private readonly ChatClient _chatClient;

        public OpenAIApiClient(string apiKey, string? baseUrl = null)
        {
            var options = new OpenAIClientOptions();
            if (!string.IsNullOrEmpty(baseUrl))
            {
                options.Endpoint = new Uri(baseUrl);
            }

            _client = new OpenAIClient(new System.ClientModel.ApiKeyCredential(apiKey), options);
            _chatClient = _client.GetChatClient("deepseek-chat"); // 默认模型，可以在请求时覆盖
        }

        public async Task<string?> CreateChatCompletionAsync(List<Models.ChatMessage> messages, string model = "deepseek-chat")
        {
            try
            {
                var chatMessages = ConvertToChatMessages(messages);

                var completion = await _chatClient.CompleteChatAsync(chatMessages, new ChatCompletionOptions
                {
                    MaxOutputTokenCount = 1000,
                    Temperature = 0.7f
                });

                return completion.Value.Content.FirstOrDefault()?.Text;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OpenAI API调用异常: {ex.Message}");
                return null;
            }
        }

        public async IAsyncEnumerable<string> CreateChatCompletionStreamAsync(
            List<Models.ChatMessage> messages,
            string model = "gpt-3.5-turbo",
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var chatMessages = ConvertToChatMessages(messages);

            var streamingCompletion = _chatClient.CompleteChatStreamingAsync(chatMessages, new ChatCompletionOptions
            {
                MaxOutputTokenCount = 1000,
                Temperature = 0.7f
            }, cancellationToken);

            await foreach (var update in streamingCompletion.WithCancellation(cancellationToken))
            {
                if (update.ContentUpdate.Count > 0)
                {
                    foreach (var contentPart in update.ContentUpdate)
                    {
                        if (!string.IsNullOrEmpty(contentPart.Text))
                        {
                            yield return contentPart.Text;
                        }
                    }
                }
            }
        }

        private static List<OpenAI.Chat.ChatMessage> ConvertToChatMessages(List<Models.ChatMessage> messages)
        {
            var result = new List<OpenAI.Chat.ChatMessage>();

            foreach (var message in messages)
            {
                switch (message.Role.ToLower())
                {
                    case "system":
                        result.Add(OpenAI.Chat.ChatMessage.CreateSystemMessage(message.Content));
                        break;

                    case "user":
                        result.Add(OpenAI.Chat.ChatMessage.CreateUserMessage(message.Content));
                        break;

                    case "assistant":
                        result.Add(OpenAI.Chat.ChatMessage.CreateAssistantMessage(message.Content));
                        break;

                    default:
                        result.Add(OpenAI.Chat.ChatMessage.CreateUserMessage(message.Content));
                        break;
                }
            }

            return result;
        }
    }
}