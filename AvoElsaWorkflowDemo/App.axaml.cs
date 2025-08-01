using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvoElsaWorkflowDemo.ViewModels;
using AvoElsaWorkflowDemo.Views;
using Microsoft.Extensions.DependencyInjection;
using Splat;

namespace AvoElsaWorkflowDemo
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
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        public override void RegisterServices()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();

            ApplicationBuilder applicationBuilder= new ApplicationBuilder(serviceDescriptors);
            System.IServiceProvider serviceProvider = applicationBuilder.Build();


            Locator.CurrentMutable.RegisterLazySingleton(() => serviceProvider);


            base.RegisterServices();
        }


    }
}