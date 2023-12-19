using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day5
{
    internal class Program
    {
        static Seed[] seeds;
        static Dictionary<int, List<Map>> mappings = new Dictionary<int, List<Map>>();

        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            FixLists(readText);

            var p1 = GetP1();
            Console.WriteLine($"P1={p1}");

            FixNewSeeds(readText);

            Console.WriteLine($"no seeds {seeds.Length}");

            var p2 = GetP1();

            Console.WriteLine($"P1={p1}");
            Console.WriteLine($"P2={p2}");
        }

        private static void FixNewSeeds(string[] readText)
        {
            var seedTxt = readText[0].Split(":", StringSplitOptions.RemoveEmptyEntries)[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var ix = 0;
            seeds = new Seed[seedTxt.Length / 2];
            for (int i = 0; i < seedTxt.Length; i+=2)
            {
                long from = long.Parse(seedTxt[i]);
                long len = long.Parse(seedTxt[i+1]);
                Console.WriteLine($"from={from} to={from+len}");
                seeds[ix++] = new Seed() { From = from, To = from + len };
            }

        }

        private static void FixLists(string[] readText)
        {
            mappings.Clear();
            for (int i = 1; i < 8; i++)
            {
                mappings.Add(i, new List<Map>());
            }

            var step = 0;
            var index = 0;
            foreach (var line in readText)
            {
                switch (step)
                {
                    case 0:
                        var seedTxt = line.Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        seeds = new Seed[seedTxt.Length];
                        var i = 0;
                        foreach (var seed in seedTxt)
                        {
                            if (string.IsNullOrEmpty(seed))
                            {
                                continue;
                            }
                            seeds[i++] = new Seed() { From = long.Parse(seed), To = (long.Parse(seed) + 1) };
                        }
                        step = 1;
                        break;

                    case 1:
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }
                        if (line.Contains(":"))
                        {
                            index++;
                            step = 2;
                        }
                        break;

                    case 2:
                        if (string.IsNullOrEmpty(line))
                        {
                            step = 1;
                            continue;
                        }
                        var values = line.Split(" ");
                        var len = long.Parse(values[2]);
                        var map = new Map()
                        {
                            ToStart = long.Parse(values[0]),
                            FromStart = long.Parse(values[1])
                        };
                        map.ToStop = map.ToStart + len - 1;
                        map.FromStop = map.FromStart + len - 1;
                        mappings[index].Add(map);

                        break;

                    default:
                        break;
                }
            }
        }

        private static long GetP1()
        {
            var value = long.MaxValue;
            long tests = 0;

            foreach (var seed in seeds)
            {
                //Console.WriteLine($"seed {seed.From} {seed.To}");
                for (var i = seed.From; i < seed.To; i++)
                {
                    var val = Next(1, i);
                    tests++;

                    if (val < value)
                    {
                        Console.WriteLine($"seed {i} gives {val} new lowest");
                        value = val;
                    }

                }
            }

            return value;
        }

        private static long Next(int index, long seed)
        {
            if (index > 7)
            {
                return seed;
            }

            var maps = mappings[index].OrderBy(x => x.FromStart).ToList();

            var nextSeed = seed;

            foreach (var map in maps)
            {
                if (seed <= map.FromStop && seed >= map.FromStart)
                {
                    nextSeed = (seed - map.FromStart) + map.ToStart;
                    break;
                }
            }

            return Next(index + 1, nextSeed);
        }

    }

    internal class Seed
    {
        public long From { set; get; }
        public long To { set; get; }
    }

    internal class Map
    {
        public long FromStart { set; get; }
        public long FromStop { set; get; }
        public long ToStart { set; get; }
        public long ToStop { set; get; }
    }
}