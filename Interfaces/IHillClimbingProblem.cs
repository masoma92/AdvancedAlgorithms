using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Interfaces
{
    public interface IHillClimbingProblem<T>
    {
        List<T> LoadStartState();
        bool StopCondition(List<T> solution);
        List<T> Distance(List<T> solution, float e);
        float Fitness(List<T> solution);
    }
}
