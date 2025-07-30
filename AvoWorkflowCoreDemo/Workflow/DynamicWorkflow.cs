using AvoWorkflowCoreDemo.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AvoWorkflowCoreDemo.Workflow
{
    public class DynamicWorkflow : IWorkflow<DynamicStep>
    {
        public string Id => "DynamicWorkflow";
        public int Version => 1;

        public void Build(IWorkflowBuilder<DynamicStep> builder)
        {
            // 开始步骤
            var workflowBuilder = builder
                .StartWith(context =>
                {
                    Console.WriteLine("Workflow started");
                    return ExecutionResult.Next();
                });

            // 动态构建步骤
            workflowBuilder = workflowBuilder.Then((context) =>
            {
                // 检查 StepsToExecute，动态添加步骤


                return ExecutionResult.Next();
            });
            var branch1 = builder.CreateBranch()
                .StartWith<DynamicStep>()
                    .Input(step => step.Input, data => "hi from 1")
                .Then<DynamicStep>()
                    .Input(step => step.Input, data => "bye from 1");

            var branch2 = builder.CreateBranch()
                .StartWith<DynamicStep>()
                    .Input(step => step.Input, data => "hi from 2")
                .Then<DynamicStep>()
                    .Input(step => step.Input, data => "bye from 2");
            // 分支逻辑，基于输入值动态选择路径
            workflowBuilder
                .Decide(data => data.Input)
                    .Branch<DynamicStep>("condition1", branch1)
                    .Branch<DynamicStep>("condition2", branch2);

            // 结束步骤
            workflowBuilder.Then(context =>
            {
                Console.WriteLine("Workflow completed");
                return ExecutionResult.Next();
            });
        }
    }
}
