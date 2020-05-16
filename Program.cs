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
        static void Main()
        {
            Console.WriteLine("Function Approximation with Genetic Algorithm, press enter for start!");
            Console.ReadLine();
            FunctionApproximation FA = new FunctionApproximation(1000, 50);
            GeneticAlgorithm<Chromosome> GA = new GeneticAlgorithm<Chromosome>(FA);
            GA.GeneticAlgorithSolver();
            Console.ReadLine();
            Console.Clear();

            Console.Write("Smallest Boundary Poligon with Hill Climbing, press enter for start!");
            Console.ReadLine();
            Console.WriteLine("Hillclimbing in progress...");
            SmallestBoundaryPoligon SBP = new SmallestBoundaryPoligon();
            HillClimbingStochastic<Point> HCS = new HillClimbingStochastic<Point>(SBP);
            HCS.HillClimbingStochasticSolver();
            Console.WriteLine("Result in bin/debug/smallest_boundary_hillclimbingstochastic!");
            Console.ReadKey();
            Console.Clear();

            Console.Write("Travelling Salesman with Random Optimization, press enter for start!");
            Console.ReadLine();
            Console.WriteLine("Random Optimization in progress...");
            TravellingSalesman TS = new TravellingSalesman();
            RandomOptimization<Town> RO = new RandomOptimization<Town>(TS);
            RO.RandomOptimizationSolver();
            Console.WriteLine("Result in bin/debug/travelling_salesman_results!");
            Console.ReadKey();

        }
    }
}
