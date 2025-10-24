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
using System.Threading;
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
            Task.Run(async () => await InitChat());
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
            bool flowControl = await InitChat();
            if (!flowControl)
            {
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

        private async Task<bool> InitChat()
        {
            if (!await InitializeApiClientAsync())
            {
                Console.WriteLine("初始化失败，程序退出。");
                return false;
            }

            return true;
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

                // 创建一个空的助手消息用于流式更新
                var assistantMessage = new ChatMessage
                {
                    Role = "assistant",
                    Content = string.Empty
                };
                ConversationHistory.Add(assistantMessage);

                // 创建请求
                var request = new ChatCompletionRequest
                {
                    Model = "deepseek-chat",
                    Messages = new List<ChatMessage>(ConversationHistory.Take(ConversationHistory.Count - 1)), // 不包含刚添加的空助手消息
                    Tools = LocalFunctionHandler.GetAvailableTools(),
                    ToolChoice = "auto",
                    Temperature = 0.7,
                    MaxTokens = 1000
                };

                // 使用流式传输
                var contentBuilder = new StringBuilder();
                var toolCallsBuilder = new List<ToolCall>();
                var currentToolCall = new Dictionary<int, ToolCall>();

                await foreach (var streamResponse in _apiClient!.CreateChatCompletionStreamAsync(request))
                {
                    if (streamResponse?.Choices?.Count > 0)
                    {
                        var choice = streamResponse.Choices[0];
                        var delta = choice.Delta;

                        // 处理内容增量
                        if (!string.IsNullOrEmpty(delta.Content))
                        {
                            contentBuilder.Append(delta.Content);
                            assistantMessage.Content = contentBuilder.ToString();
                            
                            // 触发UI更新
                            this.RaisePropertyChanged(nameof(ConversationHistory));
                        }

                        // 处理工具调用增量
                        if (delta.ToolCalls?.Count > 0)
                        {
                            foreach (var toolCallDelta in delta.ToolCalls)
                            {
                                if (!currentToolCall.ContainsKey(toolCallDelta.Index))
                                {
                                    currentToolCall[toolCallDelta.Index] = new ToolCall
                                    {
                                        Id = toolCallDelta.Id ?? string.Empty,
                                        Type = toolCallDelta.Type ?? "function",
                                        Function = new FunctionCall
                                        {
                                            Name = toolCallDelta.Function?.Name ?? string.Empty,
                                            Arguments = string.Empty
                                        }
                                    };
                                }

                                var toolCall = currentToolCall[toolCallDelta.Index];
                                
                                if (!string.IsNullOrEmpty(toolCallDelta.Id))
                                    toolCall.Id = toolCallDelta.Id;
                                
                                if (!string.IsNullOrEmpty(toolCallDelta.Type))
                                    toolCall.Type = toolCallDelta.Type;

                                if (toolCallDelta.Function != null)
                                {
                                    if (!string.IsNullOrEmpty(toolCallDelta.Function.Name))
                                        toolCall.Function.Name = toolCallDelta.Function.Name;
                                    
                                    if (!string.IsNullOrEmpty(toolCallDelta.Function.Arguments))
                                        toolCall.Function.Arguments += toolCallDelta.Function.Arguments;
                                }
                            }
                        }

                        // 检查是否完成
                        if (choice.FinishReason != null)
                        {
                            // 如果有工具调用，设置到消息中
                            if (currentToolCall.Count > 0)
                            {
                                assistantMessage.ToolCalls = currentToolCall.Values.ToList();
                            }
                            break;
                        }
                    }
                }

                // 处理工具调用（如果有）
                if (assistantMessage.ToolCalls?.Count > 0)
                {
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

                    // 再次调用API获取最终回复（使用流式传输）
                    var finalRequest = new ChatCompletionRequest
                    {
                        Model = "deepseek-chat",
                        Messages = new List<ChatMessage>(ConversationHistory),
                        Temperature = 0.7,
                        MaxTokens = 1000
                    };

                    // 创建新的助手消息用于最终回复
                    var finalAssistantMessage = new ChatMessage
                    {
                        Role = "assistant",
                        Content = string.Empty
                    };
                    ConversationHistory.Add(finalAssistantMessage);

                    var finalContentBuilder = new StringBuilder();
                    await foreach (var streamResponse in _apiClient.CreateChatCompletionStreamAsync(finalRequest))
                    {
                        if (streamResponse?.Choices?.Count > 0)
                        {
                            var choice = streamResponse.Choices[0];
                            var delta = choice.Delta;

                            if (!string.IsNullOrEmpty(delta.Content))
                            {
                                finalContentBuilder.Append(delta.Content);
                                finalAssistantMessage.Content = finalContentBuilder.ToString();
                                
                                // 触发UI更新
                                this.RaisePropertyChanged(nameof(ConversationHistory));
                            }

                            if (choice.FinishReason != null)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理用户输入时发生错误: {ex.Message}");
            }
        }
    }
}