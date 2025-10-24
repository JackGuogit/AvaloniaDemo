using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using OpenAI.Chat;

namespace ChatAI.Services
{
    public class LocalFunctionHandler
    {
        public static List<ChatTool> GetAvailableTools()
        {
            return new List<ChatTool>
            {
                ChatTool.CreateFunctionTool(
                    functionName: "get_current_time",
                    functionDescription: "获取当前的日期和时间",
                    functionParameters: BinaryData.FromObjectAsJson(new
                    {
                        type = "object",
                        properties = new
                        {
                            format = new
                            {
                                type = "string",
                                description = "时间格式，可选值：'full'（完整格式）、'date'（仅日期）、'time'（仅时间）",
                                @enum = new[] { "full", "date", "time" }
                            }
                        },
                        required = new string[] { }
                    })
                ),
                ChatTool.CreateFunctionTool(
                    functionName: "get_local_name",
                    functionDescription: "获取当前设备名称",
                    functionParameters: BinaryData.FromObjectAsJson(new
                    {
                        type = "object",
                        properties = new { },
                        required = new string[] { }
                    })
                ),
                ChatTool.CreateFunctionTool(
                    functionName: "get_system_info",
                    functionDescription: "获取系统信息，包括操作系统、处理器、内存等",
                    functionParameters: BinaryData.FromObjectAsJson(new
                    {
                        type = "object",
                        properties = new { },
                        required = new string[] { }
                    })
                ),
                ChatTool.CreateFunctionTool(
                    functionName: "get_current_directory",
                    functionDescription: "获取当前工作目录路径",
                    functionParameters: BinaryData.FromObjectAsJson(new
                    {
                        type = "object",
                        properties = new { },
                        required = new string[] { }
                    })
                ),
                ChatTool.CreateFunctionTool(
                    functionName: "list_directory",
                    functionDescription: "列出指定目录的文件和文件夹",
                    functionParameters: BinaryData.FromObjectAsJson(new
                    {
                        type = "object",
                        properties = new
                        {
                            path = new
                            {
                                type = "string",
                                description = "要列出的目录路径，如果为空则使用当前目录"
                            }
                        },
                        required = new string[] { }
                    })
                ),
                ChatTool.CreateFunctionTool(
                    functionName: "calculate",
                    functionDescription: "执行基本的数学计算",
                    functionParameters: BinaryData.FromObjectAsJson(new
                    {
                        type = "object",
                        properties = new
                        {
                            expression = new
                            {
                                type = "string",
                                description = "要计算的数学表达式，支持 +、-、*、/ 运算"
                            }
                        },
                        required = new[] { "expression" }
                    })
                )
            };
        }

        public static async Task<string> ExecuteFunctionAsync(string functionName, string arguments)
        {
            try
            {
                // 解析参数
                var args = new Dictionary<string, object>();
                if (!string.IsNullOrWhiteSpace(arguments))
                {
                    try
                    {
                        var jsonDoc = JsonDocument.Parse(arguments);
                        foreach (var property in jsonDoc.RootElement.EnumerateObject())
                        {
                            args[property.Name] = property.Value.GetRawText().Trim('"');
                        }
                    }
                    catch
                    {
                        // 如果解析失败，使用空参数
                    }
                }

                switch (functionName.ToLower())
                {
                    case "get_current_time":
                        return await GetCurrentTimeAsync(args);

                    case "get_local_name":
                        return await GetLocalNameAsync();

                    case "get_system_info":
                        return await GetSystemInfoAsync();

                    case "get_current_directory":
                        return await GetCurrentDirectoryAsync();

                    case "list_directory":
                        return await ListDirectoryAsync(args);

                    case "calculate":
                        return await CalculateAsync(args);

                    default:
                        return $"未知的函数: {functionName}";
                }
            }
            catch (Exception ex)
            {
                return $"执行函数 {functionName} 时发生错误: {ex.Message}";
            }
        }

        private static async Task<string> GetCurrentTimeAsync(Dictionary<string, object>? args = null)
        {
            await Task.Delay(10); // 模拟异步操作

            var format = args?.TryGetValue("format", out var formatValue) == true 
                ? formatValue?.ToString() ?? "full" 
                : "full";

            var now = DateTime.Now;
            
            object result = format.ToLower() switch
            {
                "date" => new
                {
                    current_date = now.ToString("yyyy年MM月dd日"),
                    day_of_week = GetChineseDayOfWeek(now.DayOfWeek)
                },
                "time" => new
                {
                    current_time = now.ToString("HH:mm:ss")
                },
                _ => new
                {
                    current_time = now.ToString("yyyy年MM月dd日 HH:mm:ss"),
                    day_of_week = GetChineseDayOfWeek(now.DayOfWeek),
                    timezone = TimeZoneInfo.Local.DisplayName,
                    timestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
                }
            };

            return JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }

        private static string GetChineseDayOfWeek(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => "星期一",
                DayOfWeek.Tuesday => "星期二",
                DayOfWeek.Wednesday => "星期三",
                DayOfWeek.Thursday => "星期四",
                DayOfWeek.Friday => "星期五",
                DayOfWeek.Saturday => "星期六",
                DayOfWeek.Sunday => "星期日",
                _ => dayOfWeek.ToString()
            };
        }

        private static async Task<string> GetLocalNameAsync()
        {
            await Task.Delay(10); // 模拟异步操作
            var machineName = Environment.MachineName;
            var userName = Environment.UserName;
            var result = new
            {
                machine_name = machineName,
                user_name = userName
            };
            return JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }

        private static async Task<string> GetSystemInfoAsync()
        {
            await Task.Delay(10); // 模拟异步操作

            var result = new
            {
                operating_system = Environment.OSVersion.ToString(),
                processor_count = Environment.ProcessorCount,
                user_name = Environment.UserName,
                machine_name = Environment.MachineName,
                working_set_memory_mb = Environment.WorkingSet / 1024 / 1024,
                clr_version = Environment.Version.ToString(),
                is_64bit_os = Environment.Is64BitOperatingSystem,
                is_64bit_process = Environment.Is64BitProcess
            };

            return JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }

        private static async Task<string> GetCurrentDirectoryAsync()
        {
            await Task.Delay(10); // 模拟异步操作
            var currentDir = Directory.GetCurrentDirectory();
            var result = new
            {
                current_directory = currentDir
            };
            return JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }

        private static async Task<string> ListDirectoryAsync(Dictionary<string, object>? args = null)
        {
            await Task.Delay(10); // 模拟异步操作

            var path = args?.TryGetValue("path", out var pathValue) == true 
                ? pathValue?.ToString() ?? Directory.GetCurrentDirectory()
                : Directory.GetCurrentDirectory();

            if (!Directory.Exists(path))
            {
                var errorResult = new
                {
                    error = $"目录不存在: {path}"
                };
                return JsonSerializer.Serialize(errorResult, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
            }

            try
            {
                var directories = Directory.GetDirectories(path)
                    .Select(d => new { name = Path.GetFileName(d), type = "directory" });
                var files = Directory.GetFiles(path)
                    .Select(f => new { name = Path.GetFileName(f), type = "file" });
                
                var items = directories.Concat(files).ToList();
                
                var result = new
                {
                    path = path,
                    item_count = items.Count,
                    items = items
                };

                return JsonSerializer.Serialize(result, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
            }
            catch (Exception ex)
            {
                var errorResult = new
                {
                    error = $"无法访问目录: {ex.Message}"
                };
                return JsonSerializer.Serialize(errorResult, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
            }
        }

        private static async Task<string> CalculateAsync(Dictionary<string, object>? args = null)
        {
            await Task.Delay(10); // 模拟异步操作

            if (args?.TryGetValue("expression", out var expressionValue) != true || 
                string.IsNullOrWhiteSpace(expressionValue?.ToString()))
            {
                var errorResult = new
                {
                    error = "缺少表达式参数"
                };
                return JsonSerializer.Serialize(errorResult, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
            }

            var expression = expressionValue.ToString()!;
            
            try
            {
                // 简单的表达式计算器（仅支持基本运算）
                var result = EvaluateExpression(expression);
                var calcResult = new
                {
                    expression = expression,
                    result = result
                };
                return JsonSerializer.Serialize(calcResult, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
            }
            catch (Exception ex)
            {
                var errorResult = new
                {
                    error = $"计算错误: {ex.Message}",
                    expression = expression
                };
                return JsonSerializer.Serialize(errorResult, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
            }
        }

        /// <summary>
        /// 简单的表达式计算器
        /// </summary>
        private static double EvaluateExpression(string expression)
        {
            // 移除空格
            expression = expression.Replace(" ", "");
            
            // 简单的计算器实现（仅支持基本运算）
            var dataTable = new System.Data.DataTable();
            var result = dataTable.Compute(expression, null);
            
            return Convert.ToDouble(result);
        }
    }
}