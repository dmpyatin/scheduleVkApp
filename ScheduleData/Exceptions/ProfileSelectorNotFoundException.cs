using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Exceptions
{
    public class ProfileSelectorNotFoundException : Exception
    {
        public string Message { get; set; }

        public ProfileSelectorNotFoundException(string message)
        {
            Message = message;
        }
    }
}
