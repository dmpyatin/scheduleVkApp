using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models.SigmaGraph
{
    public class Edge
    {
        public string Id { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }

        public Edge(string id, string source, string target)
        {
            Id = id;
            Source = source;
            Target = target;
        }
    }
}
