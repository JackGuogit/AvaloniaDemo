using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AVOReactiveUI.ViewModels;
using AVOReactiveUI.Views;
using AVOReactvieUI;
using ReactiveUI;
using Splat;

namespace AVOReactiveUI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Locator.CurrentMutable.InitializeReactiveUI();
        Locator.CurrentMutable.Register(() => new ReactiveUIViewLocator(), typeof(IViewLocator));

    }

    public override void OnFrameworkInitializationCompleted()
    {

        // 启动主窗口（ViewModel-First 方式）
        MainWindowViewModel? mainWindowVM = Locator.Current.GetService<MainWindowViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindowVM,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    public override void RegisterServices()
    {
      Locator.CurrentMutable.Register(() => new MainWindowViewModel(), typeof(MainWindowViewModel));
        base.RegisterServices();
    }

}