using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day9
{
    internal class Program
    {
        static char[] path;

        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var sequences = GetSequences(readText);

            var p1 = GetP1(sequences);

            Console.WriteLine($"P1={p1} 1819125966 is right");

            var p2 = GetP2(GetSequences(readText));

            Console.WriteLine($"P1={p1} 1819125966 is right");
            Console.WriteLine($"P2={p2} 1140 is right");
        }

        private static List<List<long>> GetSequences(string[] readText)
        {
            var result = new List<List<long>>();

            foreach (var line in readText)
            {
                var items = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

                result.Add(items);
            }

            return result;
        }

        private static long GetP1(List<List<long>> sequences)
        {
            long returnVal = 0;

            var res = new List<long>();

            foreach (var sequence in sequences)
            {
                var data = GetNext(sequence.ToArray());
                Console.WriteLine($"{data} {sequence.Last()} {data + sequence.Last()}");
                res.Add(data + sequence.Last());
            }

            returnVal = res.Sum(x => x);

            return returnVal;
        }

        private static long GetNext(long[] longs, bool left = false)
        {
            long[] newItems = new long[longs.Length - 1];
            for (int i = 0; i < newItems.Length; i++)
            {
                newItems[i] = longs[i+1] - longs[i];
            }

            if (newItems.All(x => x == 0))
            {
                return 0;
            }
            else
            {
                var data = GetNext(newItems, left);
                var data2 = left ? newItems[0] : newItems[newItems.Length - 1];
                return left ? data2 - data : data2 + data;
            }
        }

        private static long GetP2(List<List<long>> sequences)
        {
            long returnVal = 0;

            var res = new List<long>();

            foreach (var sequence in sequences)
            {
                var data = GetNext(sequence.ToArray(), true);
                Console.WriteLine($"{data} {sequence.First()} {data + sequence.First()}");
                res.Add(sequence.First() - data);
            }

            returnVal = res.Sum(x => x);

            return returnVal;
        }        
    }

}