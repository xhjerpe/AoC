using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
{
    internal class Program
    {
        static char[] path;
        
        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var maze = GetMaze(readText);

            var p1 = GetP1(maze);

            Console.WriteLine($"P1={p1} 20659 is right");

            var p2 = GetP2(maze);

            Console.WriteLine($"P1={p1} 20659 is right");
            Console.WriteLine($"P2={p2} 15690466351717 is right");
        }

        private static Dictionary<string, Pos> GetMaze(string[] readText)
        {
            var result = new Dictionary<string, Pos>();

            int i = 0;
            path = new char[readText[0].Length];
            foreach (var item in readText[0])
            {
                path[i++] = item;
            }

            i = 0;
            foreach (var line in readText)
            {
                if (i++ < 2)
                    continue;

                var pos = new Pos();
                var posTxt = line.Split("=", StringSplitOptions.RemoveEmptyEntries);

                pos.Id = posTxt[0].Trim().Substring(0);
                var leftRight = posTxt[1].Split(",", StringSplitOptions.RemoveEmptyEntries);
                pos.Left = leftRight[0].Substring(2);
                pos.Right = leftRight[1].Trim().Trim(')');

                result.Add(pos.Id, pos);
            }

            return result;
        }

        private static int GetP1(Dictionary<string, Pos> games)
        {
            var returnVal = 0;

            var notFinished = true;

            var currPathIx = 0;
            var curr = games["AAA"];
            while (notFinished)
            {
                var currPath = path[currPathIx];
                if (currPath == 'L')
                {
                    curr = games[curr.Left];
                }
                else
                {
                    curr = games[curr.Right];
                }

                returnVal++;

                if (curr.Id == "ZZZ")
                {
                    notFinished = false;
                }

                if (currPathIx < path.Length - 1)
                {
                    currPathIx++;
                }
                else
                {
                    currPathIx = 0;
                }
            }

            return returnVal;
        }

        private static long GetP2(Dictionary<string, Pos> games)
        {
            long returnVal = 0;

            var notFinished = true;

            var currPathIx = 0;
            var positions = games.Where(x => x.Key[2] == 'A').Select(x => x.Value).ToArray();
            long[] loops = new long[positions.Length];
            while (notFinished)
            {
                var currPath = path[currPathIx];
                if (currPath == 'L')
                {
                    for (var i = 0; i < positions.Length; i++)
                    {
                        positions[i] = games[positions[i].Left];
                    }
                }
                else
                {
                    for (var i = 0; i < positions.Length; i++)
                    {
                        positions[i] = games[positions[i].Right];
                    }
                }

                returnVal++;

                for (var i = 0; i < positions.Length; i++)
                {
                    if (positions[i].Id.EndsWith('Z') && loops[i] == 0)
                    {
                        loops[i] = returnVal;
                    }
                }

                if (loops.All(x => x > 0))
                {
                    notFinished = false;
                }

                if (currPathIx < path.Length - 1)
                {
                    currPathIx++;
                }
                else
                {
                    currPathIx = 0;
                }
            }

            return Calc(loops);
        }

        private static long Calc(long[] loops)
        {
            var test = Lcm(268442, 178296);
            var test2 = Lcm(210, 45);
            var res = loops[0];
            for (var i = 1; i < loops.Length; i++)
            {
                res = Lcm(res, loops[i]);
            }

            return res;
        }

        private static long Lcm(long a, long b)
        {
            long gcd = 0;
            var max = long.Max(a, b);
            var min = long.Min(a, b);

            while (gcd == 0)
            {
                if (max % min == 0)
                {
                    gcd = min;
                }

                var old = max;
                max = min;
                min = old % min;
            }

            return (a*b)/gcd;
        }
    }

    public class Pos
    {
        public string Id { set; get; }
        public string Left { set; get; }
        public string Right { set; get; }

        public override string ToString()
        {
            return $"{Id} goes {Left} {Right}";
        }
    }
}