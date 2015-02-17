using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models.DataTransfer
{
    public class Placing
    {
        public Time Time { get; set; }
        public int Day { get; set; }
        public Auditorium Auditorium { get; set; }

        public Placing(Time time, int day, Auditorium auditorium)
        {
            Time = time;
            Day = day;
            Auditorium = auditorium;
        }
    }
}
