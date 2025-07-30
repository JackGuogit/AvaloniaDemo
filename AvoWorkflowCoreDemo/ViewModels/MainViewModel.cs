using AvoWorkflowCoreDemo.Step;
using AvoWorkflowCoreDemo.Workflow;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkflowCore.Interface;
using WorkflowCore.Services;

namespace AvoWorkflowCoreDemo.ViewModels
{
    public class MainViewModel: ViewModelBase
    {


        public ICommand StartWorkflowCommand => ReactiveCommand.Create(() =>
        {
            var host = SP.GetService<IWorkflowHost>();

            host.StartWorkflow("HelloWorld");

            IPersistenceProvider persistenceStore = host.PersistenceStore;
            IQueueProvider queueProvider = host.QueueProvider;

            IWorkflowRegistry registry = host.Registry;
            List<WorkflowCore.Models.WorkflowDefinition> workflowDefinitions = registry.GetAllDefinitions().ToList();

           var workflowDefinition = (IWorkflow)workflowDefinitions.Where(x => x.Id == "").FirstOrDefault();






            Info.Add("开始工作流 HelloWorld");
        });


        public ICommand StopWorkflowCommand => ReactiveCommand.Create(() =>
        {
            var host = SP.GetService<IWorkflowHost>();
            host.TerminateWorkflow("HelloWorld");
            host.Stop();
            Info.Add("停止工作流 HelloWorld");
        });

        public ICommand RegisterWorkflowCommand => ReactiveCommand.Create(() =>
        {
            var host = SP.GetService<IWorkflowHost>();
            host.RegisterWorkflow<HelloWorldWorkflow>();
            host.Start();
            Info.Add("注册工作流 HelloWorldWorkflow");
        });


        private ObservableCollection<string> _info = new ObservableCollection<string>();
        public ObservableCollection<string> Info
        {
            get => _info;
            set
            {
                this.RaiseAndSetIfChanged(ref _info, value);
            }
        }

        private IServiceProvider _sp;
        public IServiceProvider SP
        {
            get => _sp;
            set
            {
                this.RaiseAndSetIfChanged(ref _sp, value);
            }
        }

        public MainViewModel(IServiceProvider serviceProvider)
        {

            SP = ConfigureServices();
            MessageBus.Current.Listen<string>().Subscribe(x =>
            {
                Info.Add(x);
            });
            ////start the workflow host
            //var host = SP.GetService<IWorkflowHost>();
            //host.RegisterWorkflow<HelloWorldWorkflow>();
            //host.Start();

            //host.StartWorkflow("HelloWorld");


            //host.Stop();


        }
        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddWorkflow();
            //services.AddWorkflow(x => x.UseMongoDB(@"mongodb://localhost:27017", "workflow"));
            services.AddTransient<GoodbyeWorld>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
