using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Day21
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var map = new char[readText.Length][];
            var SPos = (0, 0);
            var y = 0;
            foreach (var line in readText)
            {
                var x = 0;
                map[y] = new char[line.Length];
                foreach (var pos in line)
                {
                    map[y][x] = pos;
                    if (pos == 'S')
                    {
                        SPos = (y, x);
                    }
                    x++;
                }
                y++;
            }

            var p1 = GetP1(map, SPos);

            Console.WriteLine($"P1={p1} 3687 is right");

            var p2 = GetP2(map);

            Console.WriteLine($"P1={p1} 3687 is right");
            Console.WriteLine($"P2={p2} ?? is right");
        }

        private static long GetP1(char[][] map, (int, int) sPos)
        {
            long returnVal = 0;

            var points = new List<(int, int)>() { sPos };

            for (int step = 0; step < 64; step++)
            {
                var len = points.Count;
                var newPoints = new List<(int, int)>();
                for (int pt = 0; pt < len; pt++)
                {
                    var newPos = map[points[pt].Item1 - 1][points[pt].Item2];
                    if (newPos != '#')
                    {
                        newPoints.Add((points[pt].Item1 - 1, points[pt].Item2));
                    }
                    newPos = map[points[pt].Item1 + 1][points[pt].Item2];
                    if (newPos != '#')
                    {
                        newPoints.Add((points[pt].Item1 + 1, points[pt].Item2));
                    }
                    newPos = map[points[pt].Item1][points[pt].Item2 - 1];
                    if (newPos != '#')
                    {
                        newPoints.Add((points[pt].Item1, points[pt].Item2 - 1));
                    }
                    newPos = map[points[pt].Item1][points[pt].Item2 + 1];
                    if (newPos != '#')
                    {
                        newPoints.Add((points[pt].Item1, points[pt].Item2 + 1));
                    }
                }
                points = newPoints.Distinct(new PontComp()).ToList();
            }

            returnVal = points.Count;

            return returnVal;
        }

        private static long GetP2(char[][] data)
        {
            long returnVal = 0;

            return returnVal;
        }
    }

    public class PontComp : IEqualityComparer<(int, int)>
    {
        public bool Equals((int, int) x, (int, int) y)
        {
            return x.Item1 == y.Item1 && x.Item2 == y.Item2;
        }

        public int GetHashCode([DisallowNull] (int, int) obj)
        {
            unchecked
            {
                int hash = 23;
                hash = (hash * 31) + obj.Item1;
                hash = (hash * 31) + obj.Item2;
                return hash;
            }
        }
    }
}
