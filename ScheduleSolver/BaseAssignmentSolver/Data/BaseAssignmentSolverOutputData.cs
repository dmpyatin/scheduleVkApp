using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSolver.Data;

namespace ScheduleSolver.BaseAssignmentSolver.Data
{
    public class BaseAssignmentSolverOutputData : Entity, ICloneable
    {
        public List<Link> SolutionList;
        public long TotalSolutionCost;

        #region constructor
        public BaseAssignmentSolverOutputData(List<Link> solutionList)
        {
            SolutionList = solutionList;
            TotalSolutionCost = SolutionList.Sum(x => x.Cost);
        }
        #endregion

        #region IClonable implementation

        public object Clone()
        {
            return new
            {
                SolutionList = SolutionList.Select(x => x.Clone()).ToList(),
                TotalSolutionCost = TotalSolutionCost
            };
        }
        #endregion
    }

}
