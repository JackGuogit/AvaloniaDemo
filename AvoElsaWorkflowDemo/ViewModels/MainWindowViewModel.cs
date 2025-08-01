using AvoElsaWorkflowDemo.Models;
using Elsa.Workflows;
using Elsa.Workflows.Runtime;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvoElsaWorkflowDemo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting { get; } = "Welcome to Avalonia!";

        private IServiceProvider _serviceProvider;
        private readonly IWorkflowRunner _workflowRunner;


        public ICommand ExcuteWorkflowCommand { get; }


        public MainWindowViewModel()
        {
            _serviceProvider = Locator.Current.GetService<IServiceProvider>()!;

            //IWorkflowRunner workflowRunner = _serviceProvider.GetRequiredService<IWorkflowRunner>();



            //IWorkflowRunner workflowRunner = _serviceProvider.GetRequiredService<IWorkflowRunner>();
            _workflowRunner = _serviceProvider.GetRequiredService<IWorkflowRunner>();





            ExcuteWorkflowCommand = ReactiveCommand.CreateFromTask(ExcuteWorkflow);




            // Initialization logic can go here if needed
        }


        private async Task ExcuteWorkflow()
        {
            var registriesPopulator = _serviceProvider.GetRequiredService<IRegistriesPopulator>();
            await registriesPopulator.PopulateAsync();
            try
            {

                ExWorkFlow exWorkFlow = new ();


                await _workflowRunner.RunAsync<ExWorkFlow>();

            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                Console.WriteLine($"Error starting workflow: {ex.Message}");
            }



        }



    }
}
