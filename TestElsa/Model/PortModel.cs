using Elsa.Workflows.Activities.Flowchart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestElsa.Model
{
    public class PortModel : Endpoint
    {
        public PortModel(string portid)
        {
            this.Port = portid;
        }
    }
}