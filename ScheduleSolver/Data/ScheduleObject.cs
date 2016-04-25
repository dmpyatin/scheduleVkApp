using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using ScheduleData.Models;

namespace ScheduleSolver.Data
{
    public class ScheduleObject : Entity
    {
        public long ScheduleHash { get; set; }

        public long LecturerHash { get; set; }

        public long TutorialTypeHash { get; set; }

        public long SpecialityHash { get; set; }

        public long GroupHash { get; set; }

        public long SubGroupHash { get; set; }

        public int Course { get; set; }

        public int StudentsCount { get; set; }
        
    }
}
