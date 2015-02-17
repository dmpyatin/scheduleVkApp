using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Exceptions
{
    public class IncorrectProfileSelectorNameException : Exception
    {
        public string Message { get; set; }

        public IncorrectProfileSelectorNameException(string message){
            Message = message;
        }
    }
}
