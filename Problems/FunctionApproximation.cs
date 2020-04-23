using Halal.Solvers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Halal.Problems
{
    public class ValuePair
    {
        public float input { get; set; }
        public float output { get; set; }
    }
    public class Chromosome
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }
        public int E { get; set; }
        public float currentFitness { get; set; }
    }
    public class FunctionApproximation : IGeneticProblem<Chromosome>
    {
        public int _populationSize;
        public int _bestRangeSize;
        private int generationCounter = 0;


        static public Random rnd = new Random();
        public List<ValuePair> _known_values { get; set; }
        public List<Chromosome> _population { get; set; }

        public FunctionApproximation(int populationSize, int bestRangeSize)
        {
            _populationSize = populationSize;
            _bestRangeSize = bestRangeSize;

            _known_values = new List<ValuePair>();
            _population = new List<Chromosome>();

            LoadKnownValuesFromFile(@"../../Helpers/function_approx.txt");
        }

        public void InitializePopulations()
        {
            for (int i = 0; i < _populationSize; i++)
            {
                _population.Add(new Chromosome
                {
                    A = rnd.Next(-200, 200),
                    B = rnd.Next(-200, 200),
                    C = rnd.Next(-200, 200),
                    D = rnd.Next(-200, 200),
                    E = rnd.Next(-200, 200)
                });
            }
        }

        public void Evaluation()
        {
            for (int i = 0; i < _populationSize; i++)
            {
                Fitness(_population[i]);
            }
        }

        public bool StopCondition()
        {
            this.Evaluation();
            this.PrintCurrentResult(GetBestFitness());
            float min = _population.Min(x => x.currentFitness);
            return min == 0;
        }

        public List<Chromosome> SelectParents()
        {
            _population.Sort((x, y) => x.currentFitness.CompareTo(y.currentFitness));

            return _population.Take(_bestRangeSize).ToList();
        }

        public List<List<int>> Selection(List<Chromosome> parents)
        {
            List<List<int>> matingPool = new List<List<int>>();

            for (int i = 0; i < 5; i++)
            {
                matingPool.Add(new List<int>());
            }

            for (int i = 0; i < 10; i++)
            {
                matingPool[0].Add(parents[rnd.Next(parents.Count - 1)].A);
                matingPool[1].Add(parents[rnd.Next(parents.Count - 1)].B);
                matingPool[2].Add(parents[rnd.Next(parents.Count - 1)].C);
                matingPool[3].Add(parents[rnd.Next(parents.Count - 1)].D);
                matingPool[4].Add(parents[rnd.Next(parents.Count - 1)].E);
            }
            return matingPool;
        }

        public Chromosome CrossOver(List<List<int>> matingPool)
        {
            Chromosome c = new Chromosome();
            var length = matingPool[0].Count;

            int value = matingPool[0].ElementAt(rnd.Next(length - 1));
            c.A = value;

            value = matingPool[1].ElementAt(rnd.Next(length - 1));
            c.B = value;

            value = matingPool[2].ElementAt(rnd.Next(length - 1));
            c.C = value;

            value = matingPool[3].ElementAt(rnd.Next(length - 1));
            c.D = value;

            value = matingPool[4].ElementAt(rnd.Next(length - 1));
            c.E = value;

            return c;
        }

        public Chromosome Mutate(Chromosome c)
        {
            if(rnd.Next(0, 2) == 0)
            {
                c.A = c.A + rnd.Next(-5, 6);
                c.B = c.B + rnd.Next(-5, 6);
                c.C = c.C + rnd.Next(-5, 6);
                c.D = c.D + rnd.Next(-5, 6);
                c.E = c.E + rnd.Next(-5, 6);
            }
            return c;
        }

        private void LoadKnownValuesFromFile(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                while (!sr.EndOfStream)
                {
                    string[] temp = sr.ReadLine().Split(';');
                    var vp = new ValuePair();
                    vp.input = float.Parse(temp[0]);
                    vp.output = float.Parse(temp[1]);
                    _known_values.Add(vp);
                }
            }
        }

        public void Fitness(Chromosome coefficient) // fitness - Objective
        {
            float sum_diff = 0;
            foreach (var valuepair in _known_values)
            {
                float x = valuepair.input;
                float y = coefficient.A * (float)Math.Pow(x - coefficient.B, 3) +
                          coefficient.C * (float)Math.Pow(x - coefficient.D, 2) +
                          coefficient.E;
                float diff = (float)Math.Pow(y - valuepair.output, 2);
                sum_diff += diff;
            }
            coefficient.currentFitness = sum_diff;
        }

        public int GetPopulationSize()
        {
            return _populationSize;
        }

        public void PopultionOverWrite(List<Chromosome> newPopulation)
        {
            _population = newPopulation;
        }

        public Chromosome GetBestFitness()
        {
            var minFitness = _population.Min(x => x.currentFitness);
            return _population.FirstOrDefault(x => x.currentFitness == minFitness);
        }

        private void PrintCurrentResult(Chromosome c)
        {
            if (generationCounter % 100 == 0 || c.currentFitness == 0)
            {
                Console.Clear();
                Console.WriteLine($"A: {c.A}, B: {c.B}, C: {c.C}, D: {c.D}, E: {c.E}, fitness: {c.currentFitness}");
                Console.WriteLine("GenerationCounter: " + generationCounter);
            }
            ++generationCounter;
        }
    }
}
