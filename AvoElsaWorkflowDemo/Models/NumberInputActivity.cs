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
    [Activity("Samples", "NumberInput", "提供数值输入")]
    public class NumberInputActivity : CodeActivity
    {
        public NumberInputActivity() : base() { }
        public NumberInputActivity(Input<string> prompt) : this()
        {
            Prompt = prompt;
        }

        [Input(Description = "输入提示信息")]
        public Input<string> Prompt { get; set; } = new("请输入数值");

        [Output(Description = "用户输入的数值")]
        public Output<int> Number { get; set; } = new();

        protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
        {

            // 实际场景中可结合工作流上下文或外部输入（如表单）获取数值
            // 此处示例直接使用默认值或模拟输入（实际需替换为真实输入逻辑）
            var inputNumber = 10; // 模拟用户输入的数值
            context.Set(Number, inputNumber);

            

            await ValueTask.CompletedTask; // 模拟异步操作
        }
    }
}
