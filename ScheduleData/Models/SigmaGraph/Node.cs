using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models.SigmaGraph
{
    public class Node
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Size { get; set; }

        public Node(string id, string label, double x, double y, double size)
        {
            Id = id;
            Label = label;
            X = x;
            Y = y;
            Size = size;
        }
    }
}
