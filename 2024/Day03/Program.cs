using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Day03
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
            Console.WriteLine($"P1={p1} (170778545 is correct)");
            Console.WriteLine($"P2={p2} (82868252 is correct)");
        }

        private static string GetP1(string[] readText, bool p2)
        {
            var result = 0;
            var regex = new Regex(@"mul\(\d{1,3},\d{1,3}\)|don't\(\)|do\(\)");

            var doIt = true;

            foreach (var line in readText)
            {
                var data = regex.Matches(line);
                foreach (Match item in data)
                {
                    if (item.Value.Contains("mul"))
                    {
                        var x = int.Parse(item.Value.Split(",")[0].Replace("mul(", ""));
                        var y = int.Parse(item.Value.Split(",")[1].Replace(")", ""));
                        if (p2)
                        {
                            if (doIt)
                            {
                                result += x * y;
                            }
                        }
                        else
                        {
                            result += x * y;
                        }
                    }
                    else if (item.Value.Contains("don't"))
                    {
                        doIt = false;
                    }
                    else if (item.Value.Contains("do"))
                    {
                        doIt = true;
                    }
                }
            }

            return result.ToString();
        }
    }
}
