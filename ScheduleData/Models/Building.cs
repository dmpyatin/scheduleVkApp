using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models
{
    public class Building
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }

        public bool Living { get; set; }

        public string IAIS_ID { get; set; }
    }
}
