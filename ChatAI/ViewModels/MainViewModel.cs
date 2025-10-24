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
        private static OpenAIApiClient? _apiClient;
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
            // OpenAIApiClient 不需要手动释放资源
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

            // OpenAIApiClient 不需要手动释放资源
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
                    Console.WriteLine("请创建 appsettings.json 文件并配置您的 OpenAI API Key。");
                    return false;
                }

                var configJson = await File.ReadAllTextAsync(configPath);
                var config = JsonSerializer.Deserialize<JsonElement>(configJson);

                var apiKey = config.GetProperty("OpenAI").GetProperty("ApiKey").GetString();
                var baseUrl = config.GetProperty("OpenAI").GetProperty("BaseUrl").GetString();

                if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_OPENAI_API_KEY_HERE")
                {
                    Console.WriteLine("请在 appsettings.json 中配置有效的 OpenAI API Key。");
                    return false;
                }

                _apiClient = new OpenAIApiClient(apiKey, baseUrl);
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

                // 准备消息列表（不包含刚添加的空助手消息）
                var messages = ConversationHistory.Take(ConversationHistory.Count - 1).ToList();

                // 使用流式传输
                var contentBuilder = new StringBuilder();

                await foreach (var contentChunk in _apiClient!.CreateChatCompletionStreamAsync(messages, "gpt-3.5-turbo"))
                {
                    contentBuilder.Append(contentChunk);
                    assistantMessage.Content = contentBuilder.ToString();
                    
                    // 触发UI更新
                    this.RaisePropertyChanged(nameof(ConversationHistory));
                    
                    // 添加小延迟以便用户能看到流式效果
                    await Task.Delay(50);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理用户输入时发生错误: {ex.Message}");
                
                // 如果流式失败，尝试普通请求
                try
                {
                    var messages = ConversationHistory.Take(ConversationHistory.Count - 1).ToList();
                    var response = await _apiClient!.CreateChatCompletionAsync(messages, "gpt-3.5-turbo");
                    
                    if (!string.IsNullOrEmpty(response))
                    {
                        var assistantMessage = ConversationHistory.LastOrDefault(m => m.Role == "assistant");
                        if (assistantMessage != null)
                        {
                            assistantMessage.Content = response;
                            this.RaisePropertyChanged(nameof(ConversationHistory));
                        }
                    }
                }
                catch (Exception fallbackEx)
                {
                    Console.WriteLine($"备用请求也失败了: {fallbackEx.Message}");
                }
            }
        }
    }
}