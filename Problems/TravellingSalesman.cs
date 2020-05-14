using Halal.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Problems
{
    public class Town
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
    public class TravellingSalesman : IRandomOptimization<Town>
    {
        static Random rnd = new Random();
        List<Town> towns;
        private int iterationCounter = 0;
        private double previousFitness = double.MaxValue;
        private double actualFitness = double.MaxValue;
        Queue<double> changes = new Queue<double>(5);

        public TravellingSalesman()
        {
            if (File.Exists("travelling_salesman_results.txt"))
                File.Delete("travelling_salesman_results.txt");
            this.LoadTownsFromFile();
            this.changes.Enqueue(double.MaxValue);
        }

        public List<Town> Init()
        {
            List<Town> route = new List<Town>(this.towns);
            int n = route.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                Town value = route[k];
                route[k] = route[n];
                route[n] = value;
            }
            return route;
        }

        public double Fitness(List<Town> route)
        {
            return this.GetRouteLength(route);
        }

        public bool StopCondition()
        {
            return this.changes.Sum() > 10;
        }

        public int RND()
        {
            double u1 = 1.0 - rnd.NextDouble();
            double u2 = 1.0 - rnd.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2);
            double randNormal = 0 + this.towns.Count/10 * randStdNormal;
            return randNormal > this.towns.Count ? Math.Abs(this.towns.Count) : Math.Abs((int)randNormal);
        }

        public List<Town> Step(List<Town> route, int step)
        {
            List<Town> newRoute = new List<Town>(route);
            for (int i = 0; i < step; i++)
            {
                var index = rnd.Next(newRoute.Count - 1);
                var index2 = rnd.Next(newRoute.Count - 1);

                var town = newRoute[index];
                newRoute[index] = newRoute[index2];
                newRoute[index2] = town;
            }

            return newRoute;
        }

        private List<Town> LoadTownsFromFile()
        {
            this.towns = new List<Town>();
            using (StreamReader sr = new StreamReader(@"../../Helpers/travelling_salesman_start.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string[] temp = sr.ReadLine().Split(';');
                    var t = new Town
                    {
                        X = float.Parse(temp[0]),
                        Y = float.Parse(temp[1])
                    };
                    towns.Add(t);
                }
            }
            return towns;
        }

        private double GetRouteLength(List<Town> route)
        {
            double sum_length = 0;
            for (int i = 0; i < route.Count -1; i++)
            {
                var t1 = route[i];
                var t2 = route[i + 1];
                sum_length += Math.Sqrt(Math.Pow(t1.X - t2.X, 2) + Math.Pow(t1.Y - t2.Y, 2));
            }
            return sum_length;
        }

        public void BetterSolutionHelper(List<Town> newSolution, List<Town> oldSolution)
        {
            this.previousFitness = this.GetRouteLength(oldSolution);
            this.actualFitness = this.GetRouteLength(newSolution);
            if (this.changes.Count > 4)
            {
                this.changes.Dequeue();
            }
            this.changes.Enqueue(this.previousFitness - this.actualFitness);
            this.WriteResults(newSolution);
        }

        private void WriteResults(List<Town> solution)
        {
            using (StreamWriter sw = new StreamWriter("travelling_salesman_results.txt", true))
            {
                sw.WriteLine("Clear");
                sw.WriteLine($"Iteration\t{this.iterationCounter++}");
                sw.WriteLine($"Fitness\t{this.actualFitness}");
                foreach (var item in solution)
                {
                    sw.WriteLine($"Point\t{item.X}\t{item.Y}\tBlue");
                }

                for (int i = 0; i < solution.Count-1; i++)
                {
                    sw.WriteLine($"Arrow\t{solution[i].X}\t{solution[i].Y}\t{solution[i+1].X}\t{solution[i+1].Y}\tred");
                }
            }
        }
    }
}
