
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
            Debug.WriteLine("����������ȷ�󶨵� Avalonia UI �߳�");
        }

        // ���������ڣ�ViewModel-First ��ʽ��
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









        //// �������ȳ�ʼ��������
        //Locator.CurrentMutable.InitializeReactiveUI(RegistrationNamespace.Avalonia);
        //// ע�������������߳�ִ��
        //Dispatcher.UIThread.Invoke(() =>
        //{


        //});

        //Locator.CurrentMutable.RegisterLazySingleton(() => new ReactiveUIViewLocator(), typeof(IViewLocator));
        //Locator.CurrentMutable.RegisterLazySingleton(() => new MainViewModel(), typeof(MainViewModel));



        //Locator.CurrentMutable.RegisterLazySingleton(() => new MainWindowViewModel(Locator.Current.GetService<MainViewModel>()), typeof(MainWindowViewModel));

        //Locator.CurrentMutable.InitializeReactiveUI();
        //ContainerBuilder builder = new ContainerBuilder();

        //builder.RegisterType<ReactiveUIViewLocator>().SingleInstance().As<IViewLocator>();
        // �Զ�ע���������������
        //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
        //       .AsImplementedInterfaces()
        //       .InstancePerDependency();


        //var container = builder.Build();

        //// ����Splatʹ��Autofac
        //AutofacDependencyResolver autofacDependencyResolver = new AutofacDependencyResolver(builder);

        //SplatAutofacExtensions.UseAutofacDependencyResolver(builder);
        //Locator.SetLocator(autofacDependencyResolver);
        base.RegisterServices();
    }

}