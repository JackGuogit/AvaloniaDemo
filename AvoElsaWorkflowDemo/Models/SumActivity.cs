using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvoElsaWorkflowDemo.Models
{
    [Activity("Samples", "Sum", "计算两个数的和")]
    public class SumActivity : CodeActivity
    {
        public SumActivity() : base() 
        {

        }
        public SumActivity(Input<int> input1, Input<int> input2) : this()
        {
            Input1 = input1;
            Input2 = input2;
        }

        [Input(Description = "第一个输入值")]
        public Input<int> Input1 { get; set; } = new(0);

        [Input(Description = "第二个输入值")]
        public Input<int> Input2 { get; set; } = new(0);

        [Output(Description = "计算结果")]
        public Output<int> Result { get; set; } = new();

        protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            var value1 = context.Get(Input1);
            var value2 = context.Get(Input2);
            var sum = value1 + value2;
            context.Set(Result, sum);
            await ValueTask.CompletedTask; // 模拟异步操作
        }
    }
}
