using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models
{
    public class PlanningQuartersProfile
    {
        public List<Building> Buildings { get; set; }
        public List<Auditorium> Auditoriums { get; set; }

        public List<Time> Times { get; set; }

        public List<string> Days { get; set; }

        public PlanningQuartersProfile()
        {
            Buildings = new List<Building>();
            Auditoriums = new List<Auditorium>();
            Times = new List<Time>();
            Days = new List<string>();
        }

        public PlanningQuartersProfile(List<Building> buildings, List<Auditorium> auditoriums, List<string> days, List<Time> times)
        {
            Buildings = buildings;
            Auditoriums = auditoriums;
            Days = days;
            Times = times;
        }
    }
}
