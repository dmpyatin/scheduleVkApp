using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSolver.Data
{
    public class Link : Entity, ICloneable
    {
        public int FirstId { get; set; }
        public int SecondId { get; set; }
        public long Cost { get; set; }

        #region constructor
        public Link(int firstId, int secondId, long cost)
        {
            FirstId = firstId;
            SecondId = secondId;
            Cost = cost;
        }
        #endregion

        #region IClonable implementation
        public object Clone()
        {
            return new
            {
                FirstId = FirstId,
                SecondId = SecondId,
                Cost = Cost
            };
        }
        #endregion
    }
}
