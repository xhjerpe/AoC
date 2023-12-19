using System;
using System.IO;
using System.Linq;

namespace Day18
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var data = GetData(readText);

            var p1 = GetP1(data);

            Console.WriteLine($"P1={p1} 58550 is right");

            var p2 = GetP2(data);

            Console.WriteLine($"P1={p1} 58550 is right");
            Console.WriteLine($"P2={p2} 47452118468566 is right");
        }

        private static Pos[] GetData(string[] readText)
        {
            var points = new Pos[readText.Length];
            var y = 0;
            foreach (var line in readText)
            {
                var data = line.Split(" ");
                points[y] = new Pos()
                {
                    dir = data[0].Trim()[0],
                    len = int.Parse(data[1]),
                    color = data[2].Trim(')').Trim('('),
                };
                y++;
            }

            return points;
        }

        private static long GetP1(Pos[] data)
        {
            return Calc(data);
        }

        private static long GetP2(Pos[] data)
        {
            return Calc(Fix(data));
        }

        private static Pos[] Fix(Pos[] data)
        {
            var ret = new Pos[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                var xx = data[i].color.Last();
                var dir = ToDir(int.Parse(char.ToString(xx)));
                var lenStr = data[i].color[1..(data[i].color.Length - 1)];
                ret[i] = new Pos()
                {
                    dir = dir,
                    len = Convert.ToInt64(lenStr, 16),
                    color = data[i].color,
                };
            }

            return ret;
        }

        private static char ToDir(int dir)
        {
            if (dir == 0)
            {
                return 'R';
            }

            if (dir == 1)
            {
                return 'D';
            }

            if (dir == 2)
            {
                return 'L';
            }

            return 'U';
        }

        private static long Calc(Pos[] data)
        {
            // Shoelace
            // https://www.theoremoftheday.org/GeometryAndTrigonometry/Shoelace/TotDShoelace.pdf
            long sum = 0;
            for (var i = 0; i < data.Length; i++)
            {
                var curr = data[i];
                Pos next;
                if (i == (data.Length - 1))
                {
                    next = data[0];
                }
                else
                {
                    next = data[i + 1];
                }

                switch (data[i].dir)
                {
                    case 'L':
                        next.Y = curr.Y;
                        next.X = curr.X - curr.len;
                        break;
                    case 'U':
                        next.Y = curr.Y - curr.len;
                        next.X = curr.X;
                        break;
                    case 'D':
                        next.Y = curr.Y + curr.len;
                        next.X = curr.X;
                        break;
                    case 'R':
                        next.Y = curr.Y;
                        next.X = curr.X + curr.len;
                        break;
                    default:
                        break;
                }

                sum += (curr.X * next.Y) - (next.X * curr.Y);

            }

            var insideArea = Math.Abs(sum) / 2;

            var borderAreas = data.ToList().Select(x => Math.Abs(x.len)).Sum();
            var remInsideArea = insideArea - (borderAreas / 2) + 1;

            return remInsideArea + borderAreas;
        }
    }

    internal class Pos
    {
        public char dir { get; set; }
        public long len { get; set; }

        public long Y { get; set; }
        public long X { get; set; }

        public string color { get; set; }

        public override string ToString()
        {
            return $"{Y},{X} --> {dir} {len}";
        }
    }
}
