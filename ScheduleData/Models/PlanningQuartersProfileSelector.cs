using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models
{
    public class PlanningQuartersProfileSelector
    {
        public ObjectId Id { get; set; }

        public List<string> BuildingNames { get; set; }
        public List<string> AuditoriumNumbers { get; set; }

        public List<string> Days { get; set; }
        public List<string> Times { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public PlanningQuartersProfileSelector()
        {
            BuildingNames = new List<string>();
            AuditoriumNumbers = new List<string>();
            Days = new List<string>();
            Times = new List<string>();
        }

        public PlanningQuartersProfileSelector(string name, string userName, List<string> buildingNames, List<string> auditoriumNumbers, List<string> days, List<string> times)
        {
            Name = name;
            UserName = userName;
            BuildingNames = buildingNames;
            AuditoriumNumbers = auditoriumNumbers;
            Days = days;
            Times = times;
        }
    }
}
