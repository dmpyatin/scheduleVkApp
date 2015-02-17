using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ScheduleData.Models
{
    public class Speciality
    {
        public ObjectId Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public string IAIS_ID { get; set; }
    }
}
