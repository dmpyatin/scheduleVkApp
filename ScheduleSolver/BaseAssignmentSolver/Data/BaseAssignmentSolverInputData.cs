using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSolver.Data;

namespace ScheduleSolver.BaseAssignmentSolver.Data
{
    public class BaseAssignmentSolverInputData : Entity, ICloneable
    {
        public int N1, N2;
        public long[,] M;

        public Dictionary<int, int> MapIdToRowNumber;
        public Dictionary<int, int> MapRowNumberToId;

        public Dictionary<int, int> MapIdToColumnNumber;
        public Dictionary<int, int> MapColumnNumberToId;

        #region constructor
        public BaseAssignmentSolverInputData(List<int> firstSetIds, List<int> secondSetIds, List<Link> links)
        {
            N1 = firstSetIds.Count;
            N2 = secondSetIds.Count;
            M = new long[N1, N2];

            MapIdToRowNumber = new Dictionary<int, int>();
            MapRowNumberToId = new Dictionary<int, int>();

            MapIdToColumnNumber = new Dictionary<int, int>();
            MapColumnNumberToId = new Dictionary<int, int>();

            for (var i = 0; i < firstSetIds.Count; ++i)
            {
                MapIdToRowNumber.Add(firstSetIds[i], i);
                MapRowNumberToId.Add(i, firstSetIds[i]);
            }

            for (var i = 0; i < secondSetIds.Count; ++i)
            {
                MapIdToColumnNumber.Add(secondSetIds[i], i);
                MapColumnNumberToId.Add(i, secondSetIds[i]);
            }


            foreach (var link in links)
            {
                M[MapIdToRowNumber[link.FirstId], MapIdToColumnNumber[link.SecondId]] = link.Cost;
            }

        }
        #endregion

        #region IClonable implementation

        public object Clone()
        {
            return new
            {
                N1 = N1,
                N2 = N2,
                M = M,
                MapColumnNumberToId = new Dictionary<int, int>(MapColumnNumberToId),
                MapIdToColumnNumber = new Dictionary<int, int>(MapIdToColumnNumber),
                MapIdToRowNumber = new Dictionary<int, int>(MapIdToRowNumber),
                MapRowNumberToId = new Dictionary<int, int>(MapRowNumberToId)
            };
        }
        #endregion
    }
}
