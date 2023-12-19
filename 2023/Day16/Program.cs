using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var map = File.ReadAllLines("indata.txt").Select(x => x.ToArray()).ToArray();

            var p1 = GetP1(map);

            Console.WriteLine($"P1={p1} 8034 is right");

            var p2 = GetP2(map);

            Console.WriteLine($"P1={p1} 8034 is right");
            Console.WriteLine($"P2={p2} 8225 is right");
        }

        private static long GetP1(char[][] map)
        {
            long returnVal = 0;

            returnVal += Calc(map, (0, -1), Dir.East);

            return returnVal;
        }

        private static long GetP2(char[][] map)
        {
            long returnVal = 0;

            for (int row = 0; row < map.Length; row++)
            {
                returnVal = long.Max(returnVal, Calc(map, (row, -1), Dir.East));
                returnVal = long.Max(returnVal, Calc(map, (row, map.Length), Dir.West));
                returnVal = long.Max(returnVal, Calc(map, (map.Length, row), Dir.North));
                returnVal = long.Max(returnVal, Calc(map, (-1, row), Dir.South));
            }

            return returnVal;
        }

        private static long Calc(char[][] map, (int, int) start, Dir dir)
        {
            int maxX = map[0].Length - 1;
            int maxY = map.Length - 1;

            var visited = new int[map.Length][];
            for (int row = 0; row < map.Length; row++)
            {
                visited[row] = new int[map[0].Length];
            }

            Stack<Beam> beams = new Stack<Beam>();
            beams.Push(new Beam()
            {
                Pos = start,
                Dir = dir
            });

            while (beams.Any())
            {
                var beam = beams.Pop();

                while (true)
                {
                    // next pos
                    var currPos = beam.NextPos();

                    if (currPos.Item1 > maxY || currPos.Item1 < 0 || currPos.Item2 > maxX || currPos.Item2 < 0)
                    {
                        // finished
                        break;
                    }

                    // next pos on visited pos same dir ? -> finished
                    if (IsVisited(visited[currPos.Item1][currPos.Item2], beam.Dir))
                    {
                        // beam loop
                        break;
                    }

                    visited[currPos.Item1][currPos.Item2] += (int) beam.Dir;

                    char symbol = map[currPos.Item1][currPos.Item2];
                    switch (symbol)
                    {
                        case '/':
                            if (beam.Dir == Dir.North)
                            {
                                beam.Dir = Dir.East;
                            }
                            else if(beam.Dir == Dir.East)
                            {
                                beam.Dir = Dir.North;
                            }
                            else if (beam.Dir == Dir.South)
                            {
                                beam.Dir = Dir.West;
                            }
                            else if (beam.Dir == Dir.West)
                            {
                                beam.Dir = Dir.South;
                            }
                            break;

                        case '\\':
                            if (beam.Dir == Dir.North)
                            {
                                beam.Dir = Dir.West;
                            }
                            else if (beam.Dir == Dir.East)
                            {
                                beam.Dir = Dir.South;
                            }
                            else if (beam.Dir == Dir.South)
                            {
                                beam.Dir = Dir.East;
                            }
                            else if (beam.Dir == Dir.West)
                            {
                                beam.Dir = Dir.North;
                            }
                            break;

                        case '|':
                            if (beam.Dir == Dir.West || beam.Dir == Dir.East)
                            {
                                // split
                                beam.Dir = Dir.South;

                                beams.Push(new Beam()
                                {
                                    Pos = currPos,
                                    Dir = Dir.North
                                });
                            }
                            break;


                        case '-':
                            if (beam.Dir == Dir.North || beam.Dir == Dir.South)
                            {
                                // split
                                beam.Dir = Dir.East;

                                beams.Push(new Beam()
                                {
                                    Pos = currPos,
                                    Dir = Dir.West
                                });
                            }
                            break;

                        default:
                            // is a .  just continue
                            break;
                    };

                    beam.Pos = currPos;
                }
            }

            long result = 0;

            for (int row = 0; row < visited.Length; row++)
            {
                for (int col = 0; col < visited[0].Length; col++)
                {
                    result += visited[row][col] > 0 ? 1 : 0;
                }
            }

            return result;
        }

        private static bool IsVisited(int visitedDirs, Dir dir)
        {
            if (visitedDirs == 0)
            {
                return false;
            }

            var data = (byte)visitedDirs;
            var mask = (byte)dir;

            return (data & mask) != 0;
        }
    }

    internal class Beam
    {
        public (int, int) Pos { get; set; }
        public Dir Dir { get; set; }

        internal (int, int) NextPos()
        {
            switch (Dir)
            {
                case Dir.North:
                    return (Pos.Item1 - 1, Pos.Item2);

                case Dir.West:
                    return (Pos.Item1, Pos.Item2 - 1);

                case Dir.South:
                    return (Pos.Item1 + 1, Pos.Item2);

                case Dir.East:
                    return (Pos.Item1, Pos.Item2 + 1);

                case Dir.None:
                    throw new Exception("Oh no");
                default:
                    throw new Exception("Oh no");
            }
        }
    }

    internal enum Dir
    {
        None = 0,
        North = 1,
        West = 2,
        South = 4,
        East = 8
    }
}
