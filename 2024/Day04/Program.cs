using System;
using System.IO;
using System.Linq;

namespace Day04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var p1 = GetP1(readText);
            Console.WriteLine($"P1={p1} (2507 is correct)");

            var p2 = GetP2(readText);
            Console.WriteLine($"P1={p1} (2507 is correct)");
            Console.WriteLine($"P2={p2} (1969 is correct)");
        }

        private static string GetP1(string[] readText)
        {
            var result = 0;

            var data = readText.Select(str => str.ToArray()).ToArray();

            for (int y = 0; y < data.Length; y++)
            {
                for (int x = 0; x < data[y].Length; x++)
                {
                    var curr = data[y][x];

                    // starting character shall be an X or S
                    if (curr != 'X' && curr != 'S')
                    {
                        continue;
                    }

                    if (x < data[0].Length - 3)
                    {
                        // sidan
                        if ("XMAS".SequenceEqual([data[y][x], data[y][x + 1], data[y][x + 2], data[y][x + 3]]) ||
                            "SAMX".SequenceEqual([data[y][x], data[y][x + 1], data[y][x + 2], data[y][x + 3]]))
                        {
                            result++;
                        }
                    }

                    if (y < data.Length - 3)
                    {
                        // ner
                        if ("XMAS".SequenceEqual([data[y][x], data[y + 1][x], data[y + 2][x], data[y + 3][x]]) ||
                            "SAMX".SequenceEqual([data[y][x], data[y + 1][x], data[y + 2][x], data[y + 3][x]]))
                        {
                            result++;
                        }
                    }

                    if ((y < data.Length - 3) && (x < data[0].Length - 3))
                    {
                        // ner höger
                        if ("XMAS".SequenceEqual([data[y][x], data[y + 1][x + 1], data[y + 2][x + 2], data[y + 3][x + 3]]) ||
                            "SAMX".SequenceEqual([data[y][x], data[y + 1][x + 1], data[y + 2][x + 2], data[y + 3][x + 3]]))
                        {
                            result++;
                        }
                    }

                    if ((y < data.Length - 3) && (x > 2))
                    {
                        // ner vänster
                        if ("XMAS".SequenceEqual([data[y][x], data[y + 1][x - 1], data[y + 2][x - 2], data[y + 3][x - 3]]) ||
                            "SAMX".SequenceEqual([data[y][x], data[y + 1][x - 1], data[y + 2][x - 2], data[y + 3][x - 3]]))
                        {
                            result++;
                        }
                    }
                }
            }

            return result.ToString();
        }

        public static string GetP2(string[] readText)
        {
            var result = 0;

            var data = readText.Select(str => str.ToArray()).ToArray();

            for (var y = 1; y < data.Length - 1; y++)
            {
                for (var x = 1; x < data[y].Length - 1; x++)
                {
                    var center = data[y][x];

                    // Center shall be an A
                    if (center != 'A')
                    {
                        continue;
                    }

                    var topLeft = data[y - 1][x - 1];
                    var topRight = data[y - 1][x + 1];
                    var bottomLeft = data[y + 1][x - 1];
                    var bottomRight = data[y + 1][x + 1];

                    if (("MAS".SequenceEqual([topLeft, center, bottomRight]) || "SAM".SequenceEqual([topLeft, center, bottomRight])) &&
                        ("MAS".SequenceEqual([topRight, center, bottomLeft]) || "SAM".SequenceEqual([topRight, center, bottomLeft])))
                    {
                        // criss-cross found
                        result++;
                    }
                }
            }
            return result.ToString();
        }
    }
}
