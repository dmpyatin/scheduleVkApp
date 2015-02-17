using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models
{
    public class UserSettings
    {
        public string Mode { get; set; }
        public string View { get; set; }
        public bool ChangeMode { get; set; }

        public string GroupSelectedCode { get; set; }
        public string GroupSelectedSpecName { get; set; }
        public string GroupTitle { get; set; }

        public string LecturerSelectedName { get; set; }
        public string LecturerTitle { get; set; } 
    }
}
