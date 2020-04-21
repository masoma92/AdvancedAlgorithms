using Halal.Problems;
using Halal.Solvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal
{
    class Program
    {
        static void Main(string[] args)
        {
            FunctionApproximation FA = new FunctionApproximation(1000, 50);

            GeneticAlgorithm<Chromosome> GA = new GeneticAlgorithm<Chromosome>(FA);

            var res = GA.GeneticAlgorithSolver();
            
            Console.ReadKey();
        }
    }
}
