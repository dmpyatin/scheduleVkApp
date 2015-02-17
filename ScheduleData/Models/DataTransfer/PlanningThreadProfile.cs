using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models
{
    public class PlanningThreadProfile
    {
        public List<Speciality> Specialities { get; set; }
        public List<Group> Groups { get; set; }

        public PlanningThreadProfile()
        {
            Specialities = new List<Speciality>();
            Groups = new List<Group>();
        }

        public PlanningThreadProfile(List<Speciality> specialities, List<Group> groups)
        {
            Specialities = specialities;
            Groups = groups;
        }
    }
}
