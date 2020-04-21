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
    class SmallestBoundaryPoligon
    {
        public float min_dist { get; set; }
        public List<Point> Points { get; set; }

        private void LoadPointsFromFile(string fileName)
        {
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
            var result = ((lp2.y - lp1.y) * p.x - (lp2.x - lp1.x) * p.y + lp2.x * lp1.y - lp2.y * lp1.x) / Math.Sqrt(Math.Pow(lp2.y - lp1.y, 2) + Math.Pow(lp2.x - lp1.x, 2));
            return (float)result;
        }

        public float OuterDistanceToBoundary(List<Point> solution)
        {
            float sum_min_distances = 0;

            for (int pi = 0; pi < Points.Count; pi++) // végigmegyünk a megadott pontokon
            {
                for (int li = 0; li < solution.Count; li++) // végigmegyünk az adott megoldáson
                {
                    // act_dist az adott megoldás 2 pontja között szakaszra nézi, hogy benne van-e
                    float act_dist = DistanceFromLine(solution[li], solution[(li + 1) % solution.Count], Points[pi]);
                    if (li == 0 || act_dist < min_dist)
                        min_dist = act_dist;
                }
                if (min_dist < 0)
                    sum_min_distances += -min_dist;
            }

            return sum_min_distances;
        }

        public float LengthOfBoundary(List<Point> solution)
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
    }
}
