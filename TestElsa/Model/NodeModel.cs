using Elsa.Workflows;
using Elsa.Workflows.Activities.Flowchart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestElsa.Model
{
    public class NodeModel : Activity
    {
        public List<Endpoint> Input = new List<Endpoint> { };
        public List<Endpoint> Output = new List<Endpoint> { };

        public NodeModel(string nodeid)
        {
            this.NodeId = nodeid;
            this.Name = nodeid;
            Input.Add(new Endpoint(this, "input1"));
            Input.Add(new Endpoint(this, "input2"));

            Output.Add(new Endpoint(this, "output1"));
            Output.Add(new Endpoint(this, "output2"));
        }

        protected override ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            return base.ExecuteAsync(context);
        }

        protected override ValueTask<bool> CanExecuteAsync(ActivityExecutionContext context)
        {
            return base.CanExecuteAsync(context);
        }

        protected override ValueTask OnSignalReceivedAsync(object signal, SignalContext context)
        {
            return base.OnSignalReceivedAsync(signal, context);
        }
    }
}