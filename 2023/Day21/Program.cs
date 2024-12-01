using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Day21
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var map = new char[readText.Length][];
            var SPos = (0, 0, 0, 0);
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
                        SPos = (y, x, 0, 0);
                    }
                    x++;
                }
                y++;
            }

            var p1 = GetP1(map, SPos);

            Console.WriteLine($"P1={p1} 3687 is right");

            var p2 = GetP2(map, SPos);

            Console.WriteLine($"P1={p1} 3687 is right");
            Console.WriteLine($"P2={p2} ?? is right");
        }

        private static long GetP1(char[][] map, (int, int, int, int) sPos)
        {
            var points = new List<(int, int, int, int)>() { sPos };

            for (int step = 0; step < 6; step++)
            {
                var len = points.Count;
                var newPoints = new List<(int, int, int, int)>();
                for (int pt = 0; pt < len; pt++)
                {
                    for (int dir = 0; dir < 4; dir++)
                    {
                        if (ValidPos(map, points[pt], dir, out (int, int, int, int) newPos))
                        {
                            newPoints.Add(newPos);
                        }
                    }
                }
                points = newPoints.Distinct(new PontComp()).ToList();
            }

            return points.Count;
        }

        private static long GetP2(char[][] map, (int, int, int, int) sPos)
        {
            var points = new List<(int, int, int, int)>() { sPos };

            for (int step = 0; step < 5000; step++)
            {
                var len = points.Count;
                var newPoints = new List<(int, int, int, int)>();
                for (int pt = 0; pt < len; pt++)
                {
                    for (int dir = 0; dir < 4; dir++)
                    {
                        if (ValidPos(map, points[pt], dir, out (int, int, int, int) newPos))
                        {
                            newPoints.Add(newPos);
                        }
                    }
                }
                points = newPoints.Distinct(new PontComp()).ToList();

                Console.WriteLine($"{step} {newPoints.Count}");
                var data11 = points.Where(x => x.Item3 == -1 && x.Item4 == -1).ToArray();
                var data12 = points.Where(x => x.Item3 == -1 && x.Item4 == 0).ToArray();
                var data13 = points.Where(x => x.Item3 == -1 && x.Item4 == 1).ToArray();
                var data21 = points.Where(x => x.Item3 == 0 && x.Item4 == -1).ToArray();
                var data22 = points.Where(x => x.Item3 == 0 && x.Item4 == 0).ToArray();
                var data23 = points.Where(x => x.Item3 == 0 && x.Item4 == 1).ToArray();
                var data31 = points.Where(x => x.Item3 == 1 && x.Item4 == -1).ToArray();
                var data32 = points.Where(x => x.Item3 == 1 && x.Item4 == 0).ToArray();
                var data33 = points.Where(x => x.Item3 == 1 && x.Item4 == 1).ToArray();
                Console.WriteLine($"");
                Console.WriteLine($"{data11.Length} {data12.Length} {data13.Length}");
                Console.WriteLine($"{data21.Length} {data22.Length} {data23.Length}");
                Console.WriteLine($"{data31.Length} {data32.Length} {data33.Length}");
                Console.WriteLine($"");

                points = RemovePointsInTheMiddle(points);
            }
            return points.Count;
        }

        private static List<(int, int, int, int)> RemovePointsInTheMiddle(List<(int, int, int, int)> points)
        {
            var newPoints = new List<(int, int, int, int)>();
            var maxNorth = points.OrderBy(x => x.Item3).First().Item3;
            var maxSouth = points.OrderBy(x => x.Item3).Last().Item3;
            var maxWest = points.OrderBy(x => x.Item4).First().Item4;
            var maxEast = points.OrderBy(x => x.Item4).Last().Item4;
            Console.WriteLine($"     {maxNorth}");
            Console.WriteLine($"{maxWest} {maxEast}");
            Console.WriteLine($"     {maxSouth}");

            foreach (var item in points)
            {
                if (item.Item3 > (maxNorth + 1) && item.Item3 < (maxSouth - 1))
                {
                    continue;
                }
                if (item.Item4 > (maxWest + 1) && item.Item4 < (maxEast - 1))
                {
                    continue;
                }
                newPoints.Add(item);
            }

            return newPoints;
        }

        private static bool ValidPos(char[][] map, (int, int, int, int) value, int dir, out (int, int, int, int) newPos)
        {
            newPos = value;

            if (dir == 0)
            {
                // North
                if (value.Item1 == 0)
                {
                    newPos.Item1 = map.Length - 1;
                    newPos.Item3--;
                }
                else
                {
                    newPos.Item1--;
                }
            }
            if (dir == 1)
            {
                // South
                if (value.Item1 == map.Length - 1)
                {
                    newPos.Item1 = 0;
                    newPos.Item3++;
                }
                else
                {
                    newPos.Item1++;
                }
            }
            if (dir == 2)
            {
                // west
                if (value.Item2 == 0)
                {
                    newPos.Item2 = map[0].Length - 1;
                    newPos.Item4--;
                }
                else
                {
                    newPos.Item2--;
                }
            }
            if (dir == 3)
            {
                // east
                if (value.Item2 == map[0].Length - 1)
                {
                    newPos.Item2 = 0;
                    newPos.Item4++;
                }
                else
                {
                    newPos.Item2++;
                }
            }
            return map[newPos.Item1][newPos.Item2] != '#';
        }

    }

    public class PontComp : IEqualityComparer<(int, int, int, int)>
    {
        public bool Equals((int, int, int, int) x, (int, int, int, int) y)
        {
            return x.Item1 == y.Item1 && x.Item2 == y.Item2 &&
                x.Item3 == y.Item3 && x.Item4 == y.Item4;
        }

        public int GetHashCode([DisallowNull] (int, int, int, int) obj)
        {
            unchecked
            {
                int hash = 23;
                hash = (hash * 31) + obj.Item1;
                hash = (hash * 31) + obj.Item2;
                hash = (hash * 31) + obj.Item3;
                hash = (hash * 31) + obj.Item4;
                return hash;
            }
        }
    }
}
