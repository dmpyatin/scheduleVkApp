using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models
{
    public class Auditorium
    {
        public ObjectId Id { get; set; }
        public string Number { get; set; }
        public string BuildingShortName { get; set; }

        public int Building { get; set; }
        public int Type { get; set; }

        public string IAIS_ID { get; set; }

        public string ShortName { get; set; }
    }
}
