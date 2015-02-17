using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScheduleData.Models;

namespace ScheduleApp.Models
{
    public class ScheduleViewModel
    {
        public string UserName { get; set; }
        public bool IsAuth { get; set; }


        public UserSettings UserSettings { get; set; } 
    }
}