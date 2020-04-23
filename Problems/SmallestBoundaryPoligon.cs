using Halal.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Problems
{
    public class Point
    {
        public float x { get; set; }
        public float y { get; set; }
    }
    public class SmallestBoundaryPoligon : IHillClimbingProblem<Point>
    {
        static public Random rnd = new Random();
        public List<Point> Points { get; set; }

        private int noBetterSolutionCounter = 0;
        private float actualBoundary = float.MaxValue;
        private float previousBoundary = float.MaxValue;

        private int iterationCounter = 0;

        public SmallestBoundaryPoligon()
        {
            if (File.Exists("smallest_boundary_hillclimbingstochastic.txt"))
                File.Delete("smallest_boundary_hillclimbingstochastic.txt");
            this.LoadPointsFromFile(@"../../Helpers/smallest_boundary_fix.txt");
        }

        public List<Point> LoadStartState()
        {
            List<Point> solution = new List<Point>();
            using (StreamReader sr = new StreamReader(@"../../Helpers/smallest_boundary_start.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string[] temp = sr.ReadLine().Split('\t');
                    var p = new Point();
                    p.x = float.Parse(temp[0]);
                    p.y = float.Parse(temp[1]);
                    solution.Add(p);
                }
            }
            return solution;
        }

        public bool StopCondition(List<Point> solution)
        {
            this.actualBoundary = LengthOfBoundary(solution);

            if (this.previousBoundary > this.actualBoundary)
            {
                this.previousBoundary = this.actualBoundary;
                this.noBetterSolutionCounter = 0;

                this.WriteResults(solution);
                this.iterationCounter++;
            }
            else
            {
                this.noBetterSolutionCounter++;
            }


            if (this.noBetterSolutionCounter >= 100)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public List<Point> Distance(List<Point> solution, float e)
        {
            List<Point> newPoints;

            do
            {
                newPoints = new List<Point>();
                for (int i = 0; i < solution.Count; i++)
                {
                    newPoints.Add(new Point { x = solution[i].x, y = solution[i].y });
                }
                
                for (int i = 0; i < newPoints.Count; i++)
                {
                    int isMinus = rnd.Next(0, 2);
                    int random = rnd.Next(0, 2);

                    if (isMinus == 0)
                        e = -e;

                    if (random == 0)
                        newPoints[i].x += e;
                    else
                        newPoints[i].y += e;
                }
                
            } while (OuterDistanceToBoundary(newPoints) != 0);
            

            return newPoints;
        }

        public float Fitness(List<Point> solution)
        {
            return LengthOfBoundary(solution);
        }

        private void LoadPointsFromFile(string fileName)
        {
            this.Points = new List<Point>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                while (!sr.EndOfStream)
                {
                    string[] temp = sr.ReadLine().Split(';');
                    var p = new Point();
                    p.x = float.Parse(temp[0]);
                    p.y = float.Parse(temp[1]);
                    this.Points.Add(p);
                }
            }
        }

        private float DistanceFromLine(Point lp1, Point lp2, Point p)
        {
            var result = ((lp2.y - lp1.y) * p.x - (lp2.x - lp1.x) * p.y + lp2.x * lp1.y - lp2.y * lp1.x) / Math.Sqrt(Math.Pow(lp2.y - lp1.y, 2) + 
                Math.Pow(lp2.x - lp1.x, 2));

            return (float)result;
        }

        private float OuterDistanceToBoundary(List<Point> solution) // megszoritas sum_min_distance 0 akkor jo
        {
            float sum_min_distances = 0;
            float min_dist = 0;

            for (int pi = 0; pi < Points.Count; pi++) // végigmegyünk a fix pontokon
            {
                for (int li = 0; li < solution.Count; li++) // végigmegyünk az adott megoldáson
                {
                    // act_dist az adott megoldás 2 pontja között szakaszra nézi, hogy benne van-e
                    float act_dist = DistanceFromLine(solution[li], solution[(li + 1) % solution.Count], Points[pi]);
                    if (li == 0 || act_dist < min_dist)
                        min_dist = act_dist;
                }
                if (min_dist < 0) // min_dist negativ ha a pont a polygonon kivul van
                {
                    sum_min_distances += -min_dist;
                    return sum_min_distances;
                }
            }
            return sum_min_distances;
        }

        private float LengthOfBoundary(List<Point> solution) // kerulet
        {
            float sum_length = 0;

            for (int li = 0; li < solution.Count; li++)
            {
                Point p1 = solution[li];
                Point p2 = solution[(li + 1) % solution.Count];
                sum_length += (float)Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2));
            }

            return sum_length;
        }

        private void WriteResults(List<Point> solution)
        {
            using (StreamWriter sw = new StreamWriter("smallest_boundary_hillclimbingstochastic.txt", true))
            {
                sw.WriteLine("Clear");
                sw.WriteLine($"Iteration\t{this.iterationCounter}");
                sw.WriteLine($"Fitness\t{(int)this.actualBoundary}");
                foreach (var item in this.Points)
                {
                    sw.WriteLine($"Point\t{item.x}\t{item.y}\tBlue");
                }

                for (int i = 0; i < solution.Count; i++)
                {
                    sw.WriteLine($"Point\t{solution[i].x}\t{solution[i].y}\tRed");
                    sw.WriteLine($"Line\t{solution[i].x}\t{solution[i].y}\t{solution[(i+1) % solution.Count].x}\t{solution[(i+1) % solution.Count].y}\tred");
                }
            }
        }
    }
}
