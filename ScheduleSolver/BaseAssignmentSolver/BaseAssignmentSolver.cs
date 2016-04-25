using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSolver.BaseAssignmentSolver.Data;
using ScheduleSolver.Data;

namespace ScheduleSolver.BaseAssignmentSolver
{
    public class BaseAssignmentSolver : Entity
    {
        private List<Timing> RunTimings;

        public BaseAssignmentSolver()
        {
            RunTimings = new List<Timing>();
        }

        public void WriteTimings()
        {
            Console.WriteLine(": timings for BaseAssignmentSolver");
            foreach (var runTiming in RunTimings)
            {
                Console.Write(runTiming.Description + ": ");
                Console.WriteLine(runTiming.Time);
            }
        }

        public BaseAssignmentSolverOutputData HungarianSolve(BaseAssignmentSolverInputData inputData, bool debug = true)
        {
            var t1 = DateTime.Now;
            // Размеры матрицы
            var matrix = inputData.M;
            var height = inputData.N1;
            var width = inputData.N2;

            // Значения, вычитаемые из строк (u) и столбцов (v)
            var u = new long[height];
            var v = new long[width];

            for (var i = 0; i < u.Length; ++i) u[i] = 0;
            for (var i = 0; i < v.Length; ++i) v[i] = 0;

            // Индекс помеченной клетки в каждом столбце
            var markIndices = new int[width];
            for (var i = 0; i < markIndices.Length; ++i) markIndices[i] = -1;

            for (var i = 0; i < height; ++i)
            {
                var links = new int[width];
                var mins = new long[width];
                var visited = new bool[width];

                for (var j = 0; j < width; ++j)
                {
                    links[j] = -1;
                    mins[j] = long.MaxValue;
                    visited[j] = false;
                }

                var markedI = i;
                var markedJ = -1;
                int jj = 0;

                while (markedI != -1)
                {
                    jj = -1;
                    for (var j1 = 0; j1 < width; ++j1)
                    {
                        if (!visited[j1])
                        {
                            if (matrix[markedI, j1] - u[markedI] - v[j1] < mins[j1])
                            {
                                mins[j1] = matrix[markedI, j1] - u[markedI] - v[j1];
                                links[j1] = markedJ;
                            }
                            if (jj == -1 || mins[j1] < mins[jj])
                                jj = j1;
                        }
                    }

                    var delta = mins[jj];
                    for (var j1 = 0; j1 < width; ++j1)
                    {
                        if (visited[j1])
                        {
                            u[markIndices[j1]] += delta;
                            v[j1] -= delta;
                        }
                        else
                        {
                            mins[j1] -= delta;
                        }
                    }

                    u[i] += delta;

                    visited[jj] = true;
                    markedJ = jj;
                    markedI = markIndices[jj];
                }

                while (links[jj] != -1)
                {
                    markIndices[jj] = markIndices[links[jj]];
                    jj = links[jj];
                }
                markIndices[jj] = i;
            }

            var result = new List<Link>();

            for (var j = 0; j < width; ++j)
            {
                if (markIndices[j] != -1)
                {
                    var firstId = inputData.MapRowNumberToId[markIndices[j]];
                    var secondId = inputData.MapColumnNumberToId[j];
                    var cost = matrix[markIndices[j], j];
                    result.Add(new Link(firstId, secondId, cost));
                }
            }

            var outputData = new BaseAssignmentSolverOutputData(result);

            var t2 = DateTime.Now;

            if (debug)
                RunTimings.Add(new Timing("HungarianSolve", t2 - t1));

            return outputData;
        }
    }
}
