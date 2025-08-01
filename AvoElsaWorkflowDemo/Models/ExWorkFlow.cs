using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvoElsaWorkflowDemo.Models
{
    public class ExWorkFlow:WorkflowBase
    {

        protected override ValueTask BuildAsync(IWorkflowBuilder builder, CancellationToken cancellationToken = default)
        {
            Build(builder);
            return ValueTask.CompletedTask;
        }


        protected override void Build(IWorkflowBuilder builder)
        {
            // 创建第一个数值输入活动（用于输入Input1）
            var numberInput1 = new NumberInputActivity { Prompt = new Input<string>("请输入第一个数") };

            // 创建第二个数值输入活动（用于输入Input2）
            var numberInput2 = new NumberInputActivity { Prompt = new Input<string>("请输入第二个数") };

            // 创建求和活动，并绑定输入源
            var sumActivity = new SumActivity
            {
                Input1 = new Input<int>(numberInput1.Number), // 绑定numberInput1的输出到Input1
                Input2 = new Input<int>(numberInput2.Number)  // 绑定numberInput2的输出到Input2
            };

            // 定义工作流执行顺序：先执行两个输入活动，再执行求和活动

            Sequence sequence = new Sequence();

            sequence.Activities.Add(numberInput1);
            sequence.Activities.Add(numberInput2);
            sequence.Activities.Add(sumActivity);
            
            builder.Root = sequence;
        }
    }
}
