using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Interfaces
{
    public interface IRandomOptimization<T>
    {
        List<T> Init();
        bool StopCondition();
        int RND();
        double Fitness(List<T> solution);
        void BetterSolutionHelper(List<T> oldSolution, List<T> newSolution);
        List<T> Step(List<T> route, int step);
    }
}
