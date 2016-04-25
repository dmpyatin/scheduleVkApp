using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSolver.Data
{
    public class Timing : Entity, ICloneable
    {
        public TimeSpan Time { get; set; }
        public string Description { get; set; }

        public Timing(string description, TimeSpan time)
        {
            Time = time;
            Description = description;
        }

        public object Clone()
        {
            return new
            {
                Time = Time,
                Description = Description
            };
        }
    }
}
