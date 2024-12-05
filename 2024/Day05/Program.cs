using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day05
{
    internal class Program
    {
        private static void Main()
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var p1 = GetP1(readText, false);
            Console.WriteLine($"P1={p1} (4996 is correct)");

            var p2 = GetP1(readText, true);
            Console.WriteLine($"P1={p1} (4996 is correct)");
            Console.WriteLine($"P2={p2} (6311 is correct)");
        }

        private static string GetP1(string[] readText, bool tvåan)
        {
            var result = 0;

            var rules = readText.Where(x => x.Contains("|")).Select(x => x.Split("|"))
                                                            .GroupBy(x => x[0])
                                                            .ToDictionary(t => int.Parse(t.Key), t => t.Select(x => int.Parse(x[1])).ToList())
                                                            .ToList();

            var sequences = readText.Where(x => x.Contains(",")).Select(x => x.Split(","))
                                                                .ToList();

            foreach (var sq in sequences)
            {
                var valid = true;
                var data = sq.Select(int.Parse).ToArray();
                for (var i = 1; i < data.Count(); i++)
                {
                    var item = data[i];
                    var pagesMustBeAfter = rules.Where(x => x.Key == item).SelectMany(x => x.Value).ToList();

                    var data2 = data[..i];
                    if (data2.Any(x => pagesMustBeAfter.Any(c => c == x)))
                    {
                        valid = false;
                    }
                }

                if (valid && !tvåan)
                {
                    var mitten = data[data.Length / 2];
                    result += mitten;
                }
                else if (!valid && tvåan)
                {
                    var dataSort = GetSorted(new List<int>(data)).ToArray();

                    var mitten = dataSort[dataSort.Length / 2];
                    result += mitten;
                }
            }

            List<int> GetSorted(List<int> incoming)
            {
                var result = new List<int>();

                while (incoming.Any())
                {
                    foreach (var item in incoming)
                    {
                        var pagesBefore = rules.Where(x => x.Value.Any(x => x == item)).Select(x => x.Key).ToList();
                        var next = true;

                        if (incoming.Any(x => pagesBefore.Any(y => y == x)))
                        {
                            next = false;
                        }

                        if (next)
                        {
                            result.Add(item);
                            incoming.Remove(item);
                            break;
                        }
                    }
                }

                return result;
            }

            return result.ToString();
        }
    }
}
