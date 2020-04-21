using Halal.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Solvers
{
    public interface IGeneticProblem<T>
    {
        int GetPopulationSize();
        void PopultionOverWrite(List<T> newPopulation);
        void InitializePopulations();
        void Evaluation();
        bool StopCondition();
        List<T> SelectParents();
        List<List<int>> Selection(List<T> elit);
        T CrossOver(List<List<int>> matingPool);
        T Mutate(T c);
        T GetBestFitness();
    }
}
