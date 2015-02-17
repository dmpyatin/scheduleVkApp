using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Exceptions
{
    public class ProfileSelectorAlreadyExsistsException : Exception
    {
        public string Message { get; set; }

        public ProfileSelectorAlreadyExsistsException(string message)
        {
            Message = message;
        }
    }
}
