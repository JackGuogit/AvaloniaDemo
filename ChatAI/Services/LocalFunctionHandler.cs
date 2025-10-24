using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ChatAI.Models;

namespace ChatAI.Services
{
    public class LocalFunctionHandler
    {
        public static List<Tool> GetAvailableTools()
        {
            return new List<Tool>
            {
                new Tool
                {
                    Type = "function",
                    Function = new FunctionDefinition
                    {
                        Name = "get_current_time",
                        Description = "获取当前的日期和时间",
                        Parameters = new
                        {
                            type = "object",
                            properties = new { },
                            required = new string[] { }
                        }
                    }
                },
                new Tool
                {
                    Type= "function",
                    Function = new FunctionDefinition
                    {
                        Name = "get_loacl_name",
                        Description = "获取当前设备名称",
                        Parameters = new
                        {
                            type = "object",
                            properties = new { },
                            required = new string[] { }
                        }
                    }
                }
            };
        }

        public static async Task<string> ExecuteFunctionAsync(string functionName, string arguments)
        {
            try
            {
                switch (functionName.ToLower())
                {
                    case "get_current_time":
                        return await GetCurrentTimeAsync();

                    case "get_loacl_name":
                        return await GetLocalNameAsync();

                    default:
                        return $"未知的函数: {functionName}";
                }
            }
            catch (Exception ex)
            {
                return $"执行函数 {functionName} 时发生错误: {ex.Message}";
            }
        }

        private static async Task<string> GetCurrentTimeAsync()
        {
            await Task.Delay(10); // 模拟异步操作

            var now = DateTime.Now;
            var result = new
            {
                current_time = now.ToString("yyyy-MM-dd HH:mm:ss"),
                day_of_week = now.DayOfWeek.ToString(),
                timezone = TimeZoneInfo.Local.DisplayName,
                timestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
            };

            return JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        }

        private static async Task<string> GetLocalNameAsync()
        {
            await Task.Delay(10); // 模拟异步操作
            var machineName = Environment.MachineName;
            var result = new
            {
                machine_name = machineName
            };
            return JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        }
    }
}