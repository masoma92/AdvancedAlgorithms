using Halal.Interfaces;
using Halal.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Solvers
{
    class HillClimbingStochastic<T>
    {
        private const int e = 20;

        private readonly IHillClimbingProblem<T> _problem;

        public HillClimbingStochastic(IHillClimbingProblem<T> problem)
        {
            _problem = problem;
        }

        public void HillClimbingStochasticSolver()
        {
            var p = _problem.LoadStartState();

            while (!_problem.StopCondition(p))
            {
                var q = _problem.Distance(p, e);

                if (_problem.Fitness(q) < _problem.Fitness(p))
                    p = q;
            }
            Console.WriteLine("END");
        }
    }
}
