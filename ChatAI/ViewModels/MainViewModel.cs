using ChatAI.Models;
using ChatAI.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatAI.ViewModels
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private static DeepseekApiClient? _apiClient;
        //private static List<ChatMessage> _conversationHistory = new();

        private ObservableCollection<ChatMessage> _conversationHistory = new();

        public ObservableCollection<ChatMessage> ConversationHistory
        {
            get => _conversationHistory;
            set => this.RaiseAndSetIfChanged(ref _conversationHistory, value);
        }

        private string _userInput = string.Empty;

        public string UserInput
        {
            get => _userInput;
            set => this.RaiseAndSetIfChanged(ref _userInput, value);
        }

        public ICommand SendMessageCommand { get; }

        public MainViewModel()
        {
            SendMessageCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!string.IsNullOrWhiteSpace(UserInput))
                {
                    var input = UserInput;
                    UserInput = string.Empty;
                    await ProcessUserInputAsync(input);
                }
            });
            // 启动初始化
            Task.Run(async () => await Run(Array.Empty<string>()));
        }

        public void Dispose()
        {
            _apiClient?.Dispose();
        }

        private async Task Run(string[] args)
        {
            Console.WriteLine("=== Deepseek AI 控制台应用 ===");
            Console.WriteLine("支持函数调用功能，可以获取当前时间");
            Console.WriteLine();

            // 初始化API客户端
            if (!await InitializeApiClientAsync())
            {
                Console.WriteLine("初始化失败，程序退出。");
                return;
            }

            Console.WriteLine("初始化成功！输入 'quit' 或 'exit' 退出程序。");
            Console.WriteLine("您可以询问任何问题，包括当前时间相关的问题。");
            Console.WriteLine();

            // 主对话循环
            while (true)
            {
                Console.Write("您: ");
                var userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                    continue;

                if (userInput.ToLower() is "quit" or "exit")
                {
                    Console.WriteLine("再见！");
                    break;
                }

                await ProcessUserInputAsync(userInput);
                Console.WriteLine();
            }

            _apiClient?.Dispose();
        }

        private async Task<bool> InitializeApiClientAsync()
        {
            try
            {
                // 读取配置文件
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                if (!File.Exists(configPath))
                {
                    Console.WriteLine($"配置文件不存在: {configPath}");
                    Console.WriteLine("请创建 appsettings.json 文件并配置您的 Deepseek API Key。");
                    return false;
                }

                var configJson = await File.ReadAllTextAsync(configPath);
                var config = JsonSerializer.Deserialize<JsonElement>(configJson);

                var apiKey = config.GetProperty("DeepseekApi").GetProperty("ApiKey").GetString();
                var baseUrl = config.GetProperty("DeepseekApi").GetProperty("BaseUrl").GetString();

                if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_DEEPSEEK_API_KEY_HERE")
                {
                    Console.WriteLine("请在 appsettings.json 中配置有效的 Deepseek API Key。");
                    return false;
                }

                _apiClient = new DeepseekApiClient(apiKey, baseUrl ?? "https://api.deepseek.com/v1");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"初始化API客户端时发生错误: {ex.Message}");
                return false;
            }
        }

        private async Task ProcessUserInputAsync(string userInput)
        {
            try
            {
                // 添加用户消息到对话历史
                ConversationHistory.Add(new ChatMessage
                {
                    Role = "user",
                    Content = userInput
                });

                // 创建请求
                var request = new ChatCompletionRequest
                {
                    Model = "deepseek-chat",
                    Messages = new List<ChatMessage>(ConversationHistory),
                    Tools = LocalFunctionHandler.GetAvailableTools(),
                    ToolChoice = "auto",
                    Temperature = 0.7,
                    MaxTokens = 1000
                };

                // 发送请求
                var response = await _apiClient!.CreateChatCompletionAsync(request);
                if (response?.Choices?.Count > 0)
                {
                    var assistantMessage = response.Choices[0].Message;

                    // 检查是否需要调用函数
                    if (assistantMessage.ToolCalls?.Count > 0)
                    {
                        // 添加助手消息到历史
                        ConversationHistory.Add(assistantMessage);

                        // 处理函数调用
                        foreach (var toolCall in assistantMessage.ToolCalls)
                        {
                            Console.WriteLine($"AI正在调用函数: {toolCall.Function.Name}");

                            var functionResult = await LocalFunctionHandler.ExecuteFunctionAsync(
                                toolCall.Function.Name,
                                toolCall.Function.Arguments);

                            // 添加函数结果到对话历史
                            ConversationHistory.Add(new ChatMessage
                            {
                                Role = "tool",
                                Content = functionResult,
                                ToolCallId = toolCall.Id
                            });
                        }

                        // 再次调用API获取最终回复
                        var finalRequest = new ChatCompletionRequest
                        {
                            Model = "deepseek-chat",
                            Messages = new List<ChatMessage>(ConversationHistory),
                            Temperature = 0.7,
                            MaxTokens = 1000
                        };

                        var finalResponse = await _apiClient.CreateChatCompletionAsync(finalRequest);
                        if (finalResponse?.Choices?.Count > 0)
                        {
                            var finalMessage = finalResponse.Choices[0].Message;
                            Console.WriteLine($"AI: {finalMessage.Content}");
                            ConversationHistory.Add(finalMessage);
                        }
                    }
                    else
                    {
                        // 直接回复，无需函数调用
                        Console.WriteLine($"AI: {assistantMessage.Content}");
                        ConversationHistory.Add(assistantMessage);
                    }
                }
                else
                {
                    Console.WriteLine("抱歉，无法获取AI回复。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理用户输入时发生错误: {ex.Message}");
            }
        }
    }
}