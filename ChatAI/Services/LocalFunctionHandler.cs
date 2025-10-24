using System;
using System.Collections.Generic;
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
                        properties = new { },
                        required = new string[] { }
                    })
                ),
                ChatTool.CreateFunctionTool(
                    functionName: "get_loacl_name",
                    functionDescription: "获取当前设备名称",
                    functionParameters: BinaryData.FromObjectAsJson(new
                    {
                        type = "object",
                        properties = new { },
                        required = new string[] { }
                    })
                )
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