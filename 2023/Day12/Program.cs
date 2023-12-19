using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] readText = File.ReadAllLines("indata.txt");

            var data = GetData(readText);

            var p1 = GetP1(data);

            Console.WriteLine($"P1={p1} 8270 is right");

            var p2 = GetP2(data);

            Console.WriteLine($"P1={p1} 8270 is right");
            Console.WriteLine($"P2={p2} 204640299929836 is right");
        }

        private static Row[] GetData(string[] readText)
        {
            var rows = readText
                .Select(str => str.Split(' '))
                .Select(substr => new Row() { Pattern = substr[0], Groups = substr[1].Split(",").Select(int.Parse).ToArray() })
                .ToArray();

            return rows;
        }

        private static long GetP1(Row[] rows)
        {
            long returnVal = rows.Sum(row => ArrCnt(row.Pattern, row.Groups, new()));

            return returnVal;
        }

        private static long GetP2(Row[] rows)
        {
            long returnVal = 0;

            var i = 0;
            foreach (var row in rows)
            {
                var dataUnfolded = string.Join('?', Enumerable.Repeat(row.Pattern, 5));
                var groupsRepeated = Enumerable.Repeat(row.Groups, 5).SelectMany(v => v).ToArray();

                var data = ArrCnt(dataUnfolded, groupsRepeated, new());

                returnVal += data;
                i++;
            }

            return returnVal;
        }

        private static long ArrCnt(string pattern, int[] groups, Dictionary<int, long> cache, int key = 0)
        {
            // Cred to Sebbe L for this recursion function
            if (cache.TryGetValue(key, out long count))
            {
                return count;
            }

            if (groups.Length == 0)
            {
                return cache[key] = pattern.Any(c => c == '#') ? 0 : 1;
            }

            int grpLen = groups[0];

            // search window length
            int maxGrpLen = pattern.Length - groups.Length - int.Max(grpLen, groups.Sum() - 1);
            int noNonOp = pattern[..groups[0]].Count(c => c != '.');

            for (int first = 0, last = grpLen; first <= maxGrpLen;)
            {
                // search window
                var c = pattern[first++];
                var d = pattern[last++];
                if (noNonOp == grpLen && d != '#')
                {
                    // next group
                    count += ArrCnt(pattern[last..], groups[1..], cache, key + (last * 32) + 1);
                }

                if (c == '#')
                {
                    return cache[key] = count;
                }

                noNonOp += (d == '.' ? 0 : 1) - (c == '.' ? 0 : 1);
            }
            if (noNonOp == grpLen && groups.Length == 1)
            {
                count++;
            }

            return cache[key] = count;
        }
    }

    internal class Row
    {
        public int[] Groups;
        public string Pattern;
    }
}
