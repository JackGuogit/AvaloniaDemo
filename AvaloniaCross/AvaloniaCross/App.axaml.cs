using Akavache;
using Akavache.EncryptedSqlite3;
using Akavache.Settings;
using Akavache.Sqlite3;
using Akavache.SystemTextJson;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaCross.ViewModels;
using AvaloniaCross.Views;
using Splat;
using System;
using System.IO;

namespace AvaloniaCross
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel()
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = new MainViewModel()
                };
            }
        }

        public override void RegisterServices()
        {
            /*
            string v = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AvaloniaCross12.db");

            CacheDatabase.Initialize<SystemJsonSerializer>("AvaloniaCross");

            bool isInitialized = CacheDatabase.IsInitialized;

            Splat.Builder.IAppBuilder appBuilder = Splat.Builder.AppBuilder.CreateSplatBuilder()
                .WithAkavacheCacheDatabase<SystemJsonSerializer>(
                    static build => build.WithApplicationName("AvaloniaCross").WithSqliteProvider()
                    .WithSettingsCachePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AvaloniaCross12.db"))
                    .WithSqliteDefaults()
                );

            // 注册服务

            IAkavacheBuilder akavacheBuilder = CacheDatabase.CreateBuilder();

            appBuilder.WithAkavache<SystemJsonSerializer>("AvaloniaCross",

                bu =>
                {
                    bu.WithSettingsCachePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AvaloniaCross12.db"))
                    .WithApplicationName("AvaloniaCross");
                },

                instance => { });

            bool ised = CacheDatabase.IsInitialized;
            */

            /*
            IAkavacheInstance? akavacheInstance = CacheDatabase.CreateBuilder()
                .WithSerializer<SystemJsonSerializer>()
                .WithApplicationName("AvaloniaCross")
                .WithSqliteProvider()
                .WithSettingsCachePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AvaloniaCross12.db"))
                .WithSqliteDefaults()
                .Build();
            Splat.Locator.CurrentMutable.RegisterConstant<IAkavacheInstance>(akavacheInstance);*/

            string v = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            //获取当前运行目录
            string currentDirectory = AppContext.BaseDirectory;
            //IAkavacheInstance? akavacheInstance = CacheDatabase.CreateBuilder()
            //     .WithSerializer<SystemJsonSerializer>()
            //     .WithSettingsCachePath(currentDirectory)
            //     .Build();

            //Splat.Builder.AppBuilder.CreateSplatBuilder()
            //      .WithAkavache<SystemJsonSerializer>(applicationName: "AvaloniaCross123", builder =>
            //      {
            //          builder.WithApplicationName("AvaloniaCross123")
            //           .WithSqliteProvider()
            //           .WithSqliteDefaults()
            //           .UseForcedDateTimeKind(DateTimeKind.Utc)
            //           .WithSettingsCachePath(currentDirectory).Build();
            //      }, instence => { });
            //Splat.Builder.AppBuilder.CreateSplatBuilder()
            //    .WithAkavacheCacheDatabase<SystemJsonSerializer>(builder =>
            //        builder.WithApplicationName("AvaloniaCross12")
            //               .UseForcedDateTimeKind(DateTimeKind.Utc)  // 可选：强制UTC时间
            //               .WithSqliteProvider()
            //               .WithSqliteDefaults());

            // ✅ 正确的使用方式
            string? v1;
            Splat.Builder.AppBuilder.CreateSplatBuilder()
                .WithAkavacheCacheDatabase<SystemJsonSerializer>(builder =>
                {
                    builder.WithSqliteProvider()
                           .WithSqliteDefaults()
                           .UseForcedDateTimeKind(DateTimeKind.Utc)
                           .WithSettingsCachePath(currentDirectory); // 设置缓存路径
                    v1 = builder.GetIsolatedCacheDirectory("SettingsCache");
                },
                    applicationName: "AvaloniaCross123")
                .Build();

            var akavacheInstance = CacheDatabase.CreateBuilder()
                .WithSerializer<SystemJsonSerializer>()
                .WithApplicationName("AvaloniaCross123456")
                .WithSqliteProvider()
                .WithSettingsCachePath(currentDirectory)
                .WithSqliteDefaults()
                .Build();
        }
    }

    public static class AkavacheAvaloniaExtensions
    {
        /// <summary>
        /// 扩展 Avalonia AppBuilder，集成 Akavache 缓存
        /// </summary>
        /// <param name="builder">
        /// Avalonia 应用构建器
        /// </param>
        /// <param name="applicationName">
        /// 应用名称（用于缓存路径）
        /// </param>
        /// <returns>
        /// 配置后的 Avalonia AppBuilder
        /// </returns>
        public static AppBuilder UseAkavache(this AppBuilder builder, string applicationName)
        {
            // 初始化 Splat 服务定位器，并配置 Akavache
            Splat.Builder.AppBuilder.CreateSplatBuilder()
                .WithAkavacheCacheDatabase<SystemJsonSerializer>(akavacheBuilder =>
                {
                    // 配置应用名称（缓存文件路径的基础）
                    akavacheBuilder.WithApplicationName(applicationName)
                        // 显式指定 SQLite 存储提供器（必填）
                        .WithSqliteProvider()
                        // 使用 SQLite 默认缓存配置（包含 UserAccount/LocalMachine 等缓存实例）
                        .WithSqliteDefaults();
                });

            // 返回 Avalonia 构建器以支持链式调用
            return builder;
        }

        /// <summary>
        /// 扩展 Avalonia AppBuilder，集成 Akavache 并支持自定义配置
        /// </summary>
        /// <param name="builder">
        /// Avalonia 应用构建器
        /// </param>
        /// <param name="configure">
        /// 自定义 Akavache 配置的委托
        /// </param>
        /// <returns>
        /// 配置后的 Avalonia AppBuilder
        /// </returns>
        public static AppBuilder UseAkavache(this AppBuilder builder, Action<IAkavacheBuilder>? configure = null)
        {
            //if (string.IsNullOrWhiteSpace(applicationName))
            //    throw new ArgumentException("应用程序名称不能为空", nameof(applicationName));

            return builder.AfterPlatformServicesSetup(_ =>
            {
                if (Locator.CurrentMutable == null) return;
                // 创建基础Splat构建器并配置Akavache
                Splat.Builder.AppBuilder.CreateSplatBuilder()
                    .WithAkavacheCacheDatabase<SystemJsonSerializer>(builder =>
                    {
                        // 配置默认SQLite存储提供器
                        builder.WithSqliteProvider()
                               .UseForcedDateTimeKind(DateTimeKind.Utc);

                        // 应用SQLite默认设置（路径等）
                        builder.WithSqliteDefaults();

                        // 应用用户自定义配置（允许覆盖默认设置）
                        configure?.Invoke(builder);
                    });
            });
        }
    }
}