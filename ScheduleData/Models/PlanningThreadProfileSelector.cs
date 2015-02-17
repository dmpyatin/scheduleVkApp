using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models
{
    public class PlanningThreadProfileSelector
    {
        public ObjectId Id { get; set; }

        public List<string> SpecialityNames { get; set; }
        public List<string> GroupCodes { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public PlanningThreadProfileSelector()
        {
            SpecialityNames = new List<string>();
            GroupCodes = new List<string>();
        }

        public PlanningThreadProfileSelector(string name, string userName, List<string> specialityNames, List<string> groupCodes)
        {
            Name = name;
            UserName = userName;
            SpecialityNames = specialityNames;
            GroupCodes = groupCodes;
        }
    }
}
