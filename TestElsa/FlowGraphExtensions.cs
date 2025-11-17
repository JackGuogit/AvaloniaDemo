using Elsa.Workflows;
using Elsa.Workflows.Activities.Flowchart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestElsa
{
    public static class FlowGraphExtensions
    {
        public static void ValidateOutboundConnections(this FlowGraph flowGraph, List<string> expected, IActivity activity)
        {
            Assert.Equals(expected, flowGraph.GetOutboundConnections(activity).Select(c => c.ToString()));
        }

        public static void ValidateForwardInboundConnections(this FlowGraph flowGraph, List<string> expected, IActivity activity)
        {
            IEnumerable<string> enumerable = flowGraph.GetForwardInboundConnections(activity).Select(c => c.ToString());
            Assert.Equals(expected, flowGraph.GetForwardInboundConnections(activity).Select(c => c.ToString()));
        }

        public static void ValidateBackwardConnection(this FlowGraph flowGraph, bool expectedBackward, bool expectedValid, Connection connection)
        {
            var actualBackward = flowGraph.IsBackwardConnection(connection, out var actualValid);
            Assert.Equals(expectedBackward, actualBackward);
            Assert.Equals(expectedValid, actualValid);
        }

        public static void ValidateDanglingActivity(this FlowGraph flowGraph, bool expected, Activity activity)
        {
            Assert.Equals(expected, flowGraph.IsDanglingActivity(activity));
        }

        public static void ValidateAncestorActivities(this FlowGraph flowGraph, List<Activity> expected, Activity activity)
        {
            Assert.Equals(expected, flowGraph.GetAncestorActivities(activity));
        }
    }
}