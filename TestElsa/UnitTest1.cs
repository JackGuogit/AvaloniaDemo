using Elsa.Workflows;
using Elsa.Workflows.Activities.Flowchart.Models;
using TestElsa.Model;

namespace TestElsa;

public class Tests
{
    private readonly IWorkflowRunner _workflowRunner;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        NodeModel node1 = new NodeModel("node1");
        NodeModel node2 = new NodeModel("node2");

        List<Connection> connections = new List<Connection>();
        connections.Add(new Connection(node1.Output[1], node2.Input[1]));
        var flowGraph = new FlowGraph(connections, node1);
        flowGraph.ValidateForwardInboundConnections(["node1:output1->node2:input1"], node1);
        Assert.Pass();
    }
}