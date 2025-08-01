using Elsa.Workflows.Activities.Flowchart.Activities;
using Elsa.Workflows.Activities.Flowchart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvoElsaWorkflowDemo.Models
{
    public class FlowchartViewModel:Flowchart
    {

        public StartActivity StartActivity { get; set; } = new StartActivity();
        public FlowchartViewModel()
        {
            StartActivity = new StartActivity();


            Flowchart flowchart = new Flowchart()
            {

            };


        }







    }

}
