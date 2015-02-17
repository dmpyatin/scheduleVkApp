using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Models.Temp
{
    public class TimeToPair
    {
      
        public int GetPair(Time Time){

            if (Time.StartTime == "13:30" && Time.EndTime == "15:05")
                return 4;

            if (Time.StartTime == "15:15" && Time.EndTime == "16:50")
                return 5;

            if (Time.StartTime == "17:00" && Time.EndTime == "18:35")
                return 6;

            if (Time.StartTime == "08:00" && Time.EndTime == "09:35")
                return 1;

            if (Time.StartTime == "09:45" && Time.EndTime == "11:20")
                return 2;

            if (Time.StartTime == "11:30" && Time.EndTime == "13:05")
                return 1;

            if (Time.StartTime == "17:00" && Time.EndTime == "20:15")
                return 6;

            if (Time.StartTime == "16:00" && Time.EndTime == "20:00")
                return 6;

            if (Time.StartTime == "15:15" && Time.EndTime == "17:30")
                return 5;

            if (Time.StartTime == "11:00" && Time.EndTime == "13:15")
                return 3;

            if (Time.StartTime == "14:30" && Time.EndTime == "16:05")
                return 4;

            if (Time.StartTime == "09:45" && Time.EndTime == "13:05")
                return 2;

            if (Time.StartTime == "14:00" && Time.EndTime == "15:35")
                return 4;

            if (Time.StartTime == "09:00" && Time.EndTime == "11:15")
                return 2;

            if (Time.StartTime == "11:30" && Time.EndTime == "13:45")
                return 3;

            if (Time.StartTime == "15:30" && Time.EndTime == "17:05")
                return 5;

            if (Time.StartTime == "12:50" && Time.EndTime == "15:05")
                return 4;

            if (Time.StartTime == "18:40" && Time.EndTime == "20:15")
                return 7;

            if (Time.StartTime == "13:30" && Time.EndTime == "16:50")
                return 4;

            if (Time.StartTime == "14:15" && Time.EndTime == "16:05")
                return 4;

            if (Time.StartTime == "11:40" && Time.EndTime == "13:15")
                return 3;

            if (Time.StartTime == "13:15" && Time.EndTime == "14:50")
                return 4;

            if (Time.StartTime == "09:00" && Time.EndTime == "09:45")
                return 2;

            if (Time.StartTime == "18:40" && Time.EndTime == "20:05")
                return 7;

            if (Time.StartTime == "11:40" && Time.EndTime == "13:05")
                return 3;

            if (Time.StartTime == "13:30" && Time.EndTime == "16:30")
                return 4;

            return 0;
        }
    }
}
