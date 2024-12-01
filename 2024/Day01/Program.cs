using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var p1 = GetP1(readText, false);
            Console.WriteLine($"P1={p1}");

            var p2 = GetP1(readText, true);
            Console.WriteLine($"P1={p1} (2264607 is correct)");
            Console.WriteLine($"P2={p2} (19457120 is correct)");
        }

        private static string GetP1(string[] readText, bool p2)
        {
            var total = 0;
            var total2 = 0;

            var listA = new List<int>();
            var listB = new List<int>();

            foreach (string s in readText)
            {
                var data = s.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                int a = int.Parse(data[0]);
                int b = int.Parse(data[1]);

                listA.Add(a);
                listB.Add(b);

            }

            listA = listA.Order().ToList();
            listB = listB.Order().ToList();

            int i = 0;
            foreach (var a in listA)
            {
                total += Math.Abs(a - listB[i]);
                var x = listB.Where(v => v == a).Count();
                total2 += a * x;
                i++;
            }

            Console.WriteLine($"{total} {total2}");

            if (p2)
            {
                return total2.ToString();
            }
            else
            {
                return total.ToString();
            }
        }
    }
}
