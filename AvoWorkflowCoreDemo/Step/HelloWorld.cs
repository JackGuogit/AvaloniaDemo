using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AvoWorkflowCoreDemo.Step
{
    public class HelloWorld : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Hello world");
            Task.Delay(2000).Wait(); // Simulate some work
            MessageBus.Current.SendMessage("Hello world");
            return ExecutionResult.Next();
        }
    }
}
