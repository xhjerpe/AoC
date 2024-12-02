using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day02
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
            Console.WriteLine($"P1={p1} (359 is correct)");
            Console.WriteLine($"P2={p2} (418 is correct)");
        }

        private static string GetP1(string[] readText, bool p2)
        {
            var dblArr = readText.Select(x => x.Split().Select(int.Parse).ToArray()).ToArray();

            var result = 0;
            foreach (var numberRow in dblArr)
            {
                var isValid = CheckValid(numberRow);

                if (p2 & !isValid)
                {
                    for (var j = 0; j < numberRow.Length; j++)
                    {
                        var subset = numberRow[0..j].ToList();
                        var subset2 = numberRow[(j + 1)..numberRow.Length].ToList();
                        subset.AddRange(subset2);
                        var sublist = subset.ToArray();
                        if (CheckValid(sublist))
                        {
                            isValid = true;
                            break;
                        }
                    }
                }

                if (isValid)
                {
                    result++;
                }
            }

            return result.ToString();
        }

        private static bool CheckValid(int[] intArray)
        {
            var diffList = new List<int>();
            for (var i = 0; i < intArray.Length - 1; i++)
            {
                diffList.Add(intArray[i + 1] - intArray[i]);
            };

            if (diffList.Any(diff => Math.Abs(diff) > 3 || diff == 0))
            {
                return false;
            }

            return diffList.All(diff => diff < 0) || diffList.All(diff => diff > 0);
        }
    }
}
