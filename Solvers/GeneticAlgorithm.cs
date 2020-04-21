using Halal.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Solvers
{
    class GeneticAlgorithm<T>
    {
        public const int ELIT_SIZE = 10;

        private readonly IGeneticProblem<T> _problem;

        public GeneticAlgorithm(IGeneticProblem<T> problem)
        {
            this._problem = problem;
        }

        public T GeneticAlgorithSolver()
        {
            _problem.InitializePopulations();
            _problem.Evaluation();

            while (!_problem.StopCondition())
            {
                List<T> parents = _problem.SelectParents();
                List<T> newPopulation = new List<T>();
                
                newPopulation.AddRange(parents.Take(ELIT_SIZE));
                parents.RemoveRange(0, ELIT_SIZE);

                while (newPopulation.Count < _problem.GetPopulationSize())
                {
                    List<List<int>> matingPool = _problem.Selection(parents);
                    var c = _problem.CrossOver(matingPool);
                    c = _problem.Mutate(c);
                    newPopulation.Add(c);
                }
                _problem.PopultionOverWrite(newPopulation);
            }
            return _problem.GetBestFitness();
        }
    }
}
