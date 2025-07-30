using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AvoWorkflowCoreDemo.Step
{
    // 通用任务步骤
    public class DynamicStep : StepBody
    {
        public string StepName { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine($"Executing {StepName} with input: {Input}");
            Output = $"{StepName} processed: {Input}";
            return ExecutionResult.Next();
        }
    }
}
