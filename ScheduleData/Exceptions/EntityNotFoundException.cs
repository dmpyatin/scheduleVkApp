using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleData.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public IList<string> Messages;

        public EntityNotFoundException()
        {
            Messages = new List<string>();
        }

    }
}
