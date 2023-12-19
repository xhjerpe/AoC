using System;
using System.IO;

namespace Day12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var data = GetData(readText);

            var p1 = GetP1(data);

            Console.WriteLine($"P1={p1} ?? is right");

            var p2 = GetP2(data);

            Console.WriteLine($"P1={p1} ?? is right");
            Console.WriteLine($"P2={p2} ?? is right");
        }

        private static char[][] GetData(string[] readText)
        {
            var map = new char[readText.Length][];
            var y = 0;
            foreach (var line in readText)
            {
                var x = 0;
                map[y] = new char[line.Length];
                foreach (var pos in line)
                {
                    map[y][x] = pos;
                    x++;
                }
                y++;
            }

            return map;
        }

        private static long GetP1(char[][] data)
        {
            long returnVal = 0;

            return returnVal;
        }

        private static long GetP2(char[][] data)
        {
            long returnVal = 0;

            return returnVal;
        }
    }
}