using System;
using System.Collections.Generic;
using System.IO;

namespace Day06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var data = GetData(readText);

            var p1 = GetP1(data);

            Console.WriteLine($"P1={p1} 220320 is right");

            var p2 = GetP2(readText);

            Console.WriteLine($"P1={p1} 220320 is right");
            Console.WriteLine($"P2={p2} 34454850 is right");
        }

        private static List<Tuple<string, string>> GetData(string[] readText)
        {
            var list = new List<Tuple<string, string>>();

            var data = readText[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var data2 = readText[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var i = 0;
            foreach (var item in data)
            {
                list.Add(new Tuple<string, string>(item, data2[i]));
                i++;
            }

            return list;
        }

        private static long GetP1(List<Tuple<string, string>> data)
        {
            long returnVal = 1;

            foreach (var item in data)
            {
                var wins = Calc(long.Parse(item.Item1), long.Parse(item.Item2));
                returnVal *= wins;
            }

            return returnVal;
        }

        private static long GetP2(string[] readText)
        {
            var data = readText[0].Split(':')[1].Replace(" ", "", StringComparison.CurrentCultureIgnoreCase);
            var data2 = readText[1].Split(':')[1].Replace(" ", "", StringComparison.CurrentCultureIgnoreCase);

            return Calc(long.Parse(data), long.Parse(data2));
        }

        private static long Calc(long time, long dist)
        {
            var result = 0;

            long it = 0;

            while (true)
            {
                var test = (time - it) * it;
                if (test >= dist)
                {
                    result++;
                }
                if (result > 0 && test < dist)
                {
                    break;
                }

                it++;
            }

            return result;
        }

    }
}
