using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSolver.Data
{
    public class ScheduleMetaObject : Entity
    {
        public List<ScheduleObject> SO;
        public int StudentsCount;

        public ScheduleMetaObject(List<ScheduleObject> so)
        {
            SO = so;
            StudentsCount = SO.Sum(x => x.StudentsCount);
        }

    }
}
