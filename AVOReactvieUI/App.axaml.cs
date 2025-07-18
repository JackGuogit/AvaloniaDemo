
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using AVOReactiveUI.ViewModels;
using AVOReactiveUI.Views;
using AVOReactiveUI;
using ReactiveUI;
using Splat;

using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace AVOReactiveUI;

public partial class App : Application
{
    public override void Initialize()
    {

        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            Console.WriteLine($"Unobserved Task Exception: {e.Exception}");
        };

        AvaloniaXamlLoader.Load(this);

        //Locator.CurrentMutable.Register(() => new ReactiveUIViewLocator(), typeof(IViewLocator));

    }

    public override void OnFrameworkInitializationCompleted()
    {

        if (RxApp.MainThreadScheduler is AvaloniaScheduler)
        {
            Debug.WriteLine("调度器已正确绑定到 Avalonia UI 线程");
        }

        // 启动主窗口（ViewModel-First 方式）
        //MainWindowViewModel? mainWindowVM = Locator.Current.GetService<MainWindowViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Locator.Current.GetService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    public override void RegisterServices()
    {

        SplatRegistrations.Register<MainWindowViewModel>();
        SplatRegistrations.Register<MainViewModel>();
        SplatRegistrations.SetupIOC();









        //// 必须优先初始化调度器
        //Locator.CurrentMutable.InitializeReactiveUI(RegistrationNamespace.Avalonia);
        //// 注册服务必须在主线程执行
        //Dispatcher.UIThread.Invoke(() =>
        //{


        //});

        //Locator.CurrentMutable.RegisterLazySingleton(() => new ReactiveUIViewLocator(), typeof(IViewLocator));
        //Locator.CurrentMutable.RegisterLazySingleton(() => new MainViewModel(), typeof(MainViewModel));



        //Locator.CurrentMutable.RegisterLazySingleton(() => new MainWindowViewModel(Locator.Current.GetService<MainViewModel>()), typeof(MainWindowViewModel));

        //Locator.CurrentMutable.InitializeReactiveUI();
        //ContainerBuilder builder = new ContainerBuilder();

        //builder.RegisterType<ReactiveUIViewLocator>().SingleInstance().As<IViewLocator>();
        // 自动注册程序集内所有类型
        //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
        //       .AsImplementedInterfaces()
        //       .InstancePerDependency();


        //var container = builder.Build();

        //// 配置Splat使用Autofac
        //AutofacDependencyResolver autofacDependencyResolver = new AutofacDependencyResolver(builder);

        //SplatAutofacExtensions.UseAutofacDependencyResolver(builder);
        //Locator.SetLocator(autofacDependencyResolver);
        base.RegisterServices();
    }

}