using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    internal class Program
    {
        static List<int> ColumnsWithExpansion = new List<int>();

        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var galaxyList = GetGalaxyList(readText);

            var p1 = GetDistancesSum(galaxyList, 1);

            Console.WriteLine($"P1={p1} 9521550 is right");

            var p2 = GetDistancesSum(galaxyList, 999999);

            Console.WriteLine($"P1={p1} 9521550 is right");
            Console.WriteLine($"P2={p2} 298932923702 is right?");
        }

        private static List<Pos> GetGalaxyList(string[] readText)
        {
            List<Pos> galaxyList = new List<Pos>();
            int cnt = 0;
            var map = new char[readText.Length][];
            var y = 0;
            foreach (var line in readText)
            {
                var x = 0;
                map[y] = new char[line.Length];
                foreach (var pos in line)
                {
                    map[y][x] = pos;
                    if (pos == '#')
                    {
                        galaxyList.Add(new Pos() { Y = y, X = x, No = ++cnt });
                    }
                    x++;
                }
                y++;

                if (y == readText.Length)
                {
                    // last line added
                    for (var col = 0; col < map[0].Length; col++)
                    {
                        if (Enumerable.Range(0, map.Length).Select(x => map[x][col]).All(x => x == '.'))
                        {
                            ColumnsWithExpansion.Add(col);
                        }
                    }
                }
            }

            return galaxyList;
        }

        private static long GetDistancesSum(List<Pos> galaxyList, int expansion)
        {
            long returnVal = 0;

            var combinations = (from item1 in galaxyList
                               from item2 in galaxyList
                               where item1.No < item2.No
                               select Tuple.Create(item1, item2)).ToList();

            foreach (var combination in combinations)
            {
                var Ylength = Distance(combination.Item1.Y, combination.Item2.Y, 0);
                var Xlength = Distance(combination.Item1.X, combination.Item2.X, expansion);

                var length = Ylength + Xlength;

                returnVal += length;
            }

            return returnVal;
        }

        private static long Distance(long galaxyA, long galaxyB, int expansion)
        {
            var max = long.Max(galaxyA, galaxyB);
            var min = long.Min(galaxyA, galaxyB);
            var cols = ColumnsWithExpansion.Where(x => x < max && x > min).ToList();
            return (max - min) + (cols.Count * expansion);
        }
    }

    internal class Pair
    {
        public Pos Item1;
        public Pos Item2;

        public Pair(Pos item1, Pos item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

    internal class Pos
    {
        public long No;
        public long X;
        public long Y;

        public override string ToString()
        {
            return $"{No} at {Y} {X}";
        }
    }
}