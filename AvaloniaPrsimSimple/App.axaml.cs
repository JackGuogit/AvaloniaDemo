using Avalonia;
using Avalonia.Controls;
using AvaloniaPrsimSimple.ViewModels;
using AvaloniaPrsimSimple.Views;
using Prism;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;

namespace AvaloniaPrsimSimple;

public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Register you Services, Views, Dialogs, etc.
        // 注册 MainWindowViewModel
        containerRegistry.RegisterSingleton<MainWindowViewModel>();
    }

    protected override AvaloniaObject CreateShell()
    {
        var eventAggregator = Container.Resolve<IEventAggregator>();

        return new MainWindow(eventAggregator);
        return new MainWindow("avalonia prsim simple");
    }
    
}