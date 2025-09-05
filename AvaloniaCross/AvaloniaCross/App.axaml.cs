using Akavache;
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

            base.OnFrameworkInitializationCompleted();
        }

        public override void RegisterServices()
        {
            
            // ×¢²á·þÎñ
            IAkavacheInstance? akavacheInstance = CacheDatabase.CreateBuilder()
                .WithSerializer<SystemJsonSerializer>()
                .WithApplicationName("AvaloniaCross")
                .WithSqliteProvider()
                .WithSettingsCachePath("AvaloniaCross.db")
                .WithSettingsCachePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
                .WithSqliteDefaults()
                .Build();


            Splat.Locator.CurrentMutable.RegisterConstant<IAkavacheInstance>(akavacheInstance);
        }


    }
}