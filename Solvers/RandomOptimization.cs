using Halal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Solvers
{
    public class RandomOptimization<T>
    {
        private readonly IRandomOptimization<T> _problem;

        public RandomOptimization(IRandomOptimization<T> problem)
        {
            _problem = problem;
        }

        public void RandomOptimizationSolver()
        {
            var p = _problem.Init();
            while (_problem.StopCondition())
            {
                var step = _problem.RND();
                var q = _problem.Step(p, step);
                if (_problem.Fitness(q) < _problem.Fitness(p))
                {
                    _problem.BetterSolutionHelper(q, p);
                    p = q;
                }
            }
        }
    }
}
