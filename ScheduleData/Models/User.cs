using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace ScheduleData.Models
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public UserSettings Settings { get; set; }
    }
}
