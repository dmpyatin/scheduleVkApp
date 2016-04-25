using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ScheduleSolver.Data
{
    public class PlaceObject : Entity
    {
        public long TimeHash { get; set; }
        public long AuditoriumHash { get; set; }
        public long AuditoriumTypeHash { get; set; }
        public long WeekTypeHash { get; set; }

        public int Capacity { get; set; }
    }
}
