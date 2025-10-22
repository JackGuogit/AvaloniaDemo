using Akavache;
using Akavache.Settings;
using Akavache.Sqlite3;
using Akavache.SystemTextJson;
using Avalonia;
using Avalonia.ReactiveUI;
using Splat;
using System;
using System.IO;
using System.Text.Json.Serialization;

namespace AvaloniaCross.Desktop;

internal sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized yet
    // and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .UseReactiveUI()
            .LogToTrace();

    //public static AppBuilder BuildAvaloniaApp()
    //    => AppBuilder.Configure<App>()
    //        .UsePlatformDetect()
    //        .WithInterFont()
    //        .LogToTrace()
    //        .UseAkavache(builder =>
    //            builder.WithApplicationName("AvaloniaCross")
    //                    // 使用加密存储（需安装 Akavache.EncryptedSqlite3 包）
    //                    .WithSqliteProvider()
    //                    .WithSqliteDefaults()
    //                    .WithSettingsCachePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AvaloniaCross12.db"))
    //                   // 自定义 System.Text.Json 序列化器
    //                   .UseSystemTextJsonSerializer(options =>
    //                   {
    //                       options.PropertyNameCaseInsensitive = true;      // 忽略属性名大小写
    //                       options.WriteIndented = false;       // 不格式化输出（节省空间）
    //                                                            // 可添加其他配置，如日期格式、转换器等
    //                       options.Converters.Add(new JsonStringEnumConverter());
    //                   })
    //        //.UseNewtonsoftJsonSerializer(settings =>
    //        //{
    //        //    settings.NullValueHandling = NullValueHandling.Ignore;
    //        //    settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
    //        //})
    //        )
    //        .UseReactiveUI();
}