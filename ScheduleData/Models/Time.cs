using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models
{
    public class Time
    {
        public ObjectId Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string IAIS_ID { get; set; }
    }
}
