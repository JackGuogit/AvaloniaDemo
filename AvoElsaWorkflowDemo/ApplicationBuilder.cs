using Elsa.Extensions;
using Elsa.Features.Services;
using Elsa.Workflows.Activities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvoElsaWorkflowDemo
{
    public class ApplicationBuilder
    {
        private readonly IServiceCollection _services;
        private Action<IModule> _configureElsa;

        public ApplicationBuilder(IServiceCollection serviceDescriptors)
        {
            _services = serviceDescriptors;

            _services.AddSingleton<IConfiguration, ConfigurationManager>();

            _configureElsa += elsa => elsa
                //.AddActivitiesFrom<WriteLine>()

                .UseCSharp()
                //.UseJavaScript()
                .UseLiquid()
                .UseWorkflowManagement();
            //.UseWorkflows(workflows => workflows
            //    .WithStandardOutStreamProvider(_ => new StandardOutStreamProvider(new ConsoleTextWriter(_testOutputHelper)))
            //);

        }
        public IServiceProvider Build()
        {
            _services.AddElsa(_configureElsa);
            return _services.BuildServiceProvider();
        }
    }
}
