# Deepseek AI 控制台应用

这是一个使用C#开发的控制台应用程序，可以与Deepseek大模型进行对话，并支持调用本地函数获取当前时间。

## 功能特性

- 与Deepseek AI模型进行自然语言对话
- 支持函数调用功能（Function Calling）
- 内置获取当前时间的本地函数
- 完整的对话历史记录
- 异常处理和错误提示

## 环境要求

- .NET 8.0 或更高版本
- 有效的Deepseek API密钥

## 安装和配置

1. **克隆或下载项目文件**

2. **配置API密钥**
   
   编辑 `appsettings.json` 文件，将 `YOUR_DEEPSEEK_API_KEY_HERE` 替换为您的实际API密钥：
   
   ```json
   {
     "DeepseekApi": {
       "ApiKey": "您的实际API密钥",
       "BaseUrl": "https://api.deepseek.com/v1"
     }
   }
   ```

3. **安装依赖包**
   
   在项目目录中运行：
   ```bash
   dotnet restore
   ```

4. **编译项目**
   
   ```bash
   dotnet build
   ```

5. **运行应用**
   
   ```bash
   dotnet run
   ```

## 使用方法

1. 启动应用后，您可以输入任何问题与AI进行对话
2. 当您询问时间相关的问题时，AI会自动调用本地函数获取当前时间
3. 输入 `quit` 或 `exit` 退出程序

## 示例对话

```
您: 现在几点了？
AI正在调用函数: get_current_time
AI: 现在是2024年1月15日 14:30:25，星期一。

您: 今天是星期几？
AI正在调用函数: get_current_time
AI: 今天是星期一。

您: 你好，介绍一下自己
AI: 您好！我是Deepseek AI助手，很高兴为您服务...
```

## 项目结构

```
DeepseekConsoleApp/
├── Models/
│   └── DeepseekModels.cs          # API数据模型
├── Services/
│   ├── DeepseekApiClient.cs       # API客户端
│   └── LocalFunctionHandler.cs    # 本地函数处理器
├── Program.cs                     # 主程序入口
├── appsettings.json              # 配置文件
├── DeepseekConsoleApp.csproj     # 项目文件
└── README.md                     # 说明文档
```

## 扩展功能

您可以通过以下方式扩展应用功能：

1. **添加新的本地函数**：在 `LocalFunctionHandler.cs` 中添加新的函数定义和实现
2. **修改AI模型参数**：在 `Program.cs` 中调整温度、最大令牌数等参数
3. **添加更多配置选项**：在 `appsettings.json` 中添加新的配置项

## 注意事项

- 请确保您的API密钥有效且有足够的配额
- 网络连接需要稳定以确保API调用成功
- 函数调用功能需要Deepseek模型支持，请确认您使用的模型版本支持此功能