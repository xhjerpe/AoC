using System;
using System.Collections.Generic;
using System.IO;

namespace Day10
{
    internal class Program
    {
        static List<Pos> allPos = new List<Pos>();
        static char[][] Maze2;

        static void Main(string[] args)
        {
            string[] data = File.ReadAllLines("indata.txt");

            allPos.Clear();
            var maze = GetMaze(data);

            var p1 = GetP1(maze);

            Console.WriteLine($"P1={p1} 7086 is right");

            var p2 = GetP2();

            Console.WriteLine($"P1={p1} 7086 is right");
            Console.WriteLine($"P2={p2} 317 is right");
        }

        private static Pos[][] GetMaze(string[] readText)
        {
            var result = new Pos[readText.Length][];
            Maze2 = new char[readText.Length][];
            var y = 0;
            foreach (var line in readText)
            {
                int x = 0;
                result[y] = new Pos[line.Length];
                Maze2[y] = new char[line.Length];
                foreach (var pos in line)
                {
                    result[y][x] = new Pos()
                    {
                        X = x,
                        Y = y,
                        Val = pos
                    };
                    allPos.Add(result[y][x]);
                    Maze2[y][x] = ' ';
                    x++;
                }
                y++;
            }

            return result;
        }

        private static long GetP1(Pos[][] maze)
        {
            long returnVal = 0;

            for (var j = 0; j < maze.Length; j++)
            {
                for (var i = 0; i < maze[0].Length; i++)
                {
                    if (maze[j][i].Val == 'S')
                    {
                        var curr = maze[j][i];
                        var dist = 1;
                        var x = i;
                        var y = j;


                        int orientation = 0; //ner
                        if (i < maze[0].Length - 1 && "-J7".Contains(maze[j][i + 1].Val)) orientation = 1; //höger
                        else if (j > 0 && "|7F".Contains(maze[j - 1][i].Val)) orientation = 2; // upp
                        else if (i > 0 && "-FL".Contains(maze[j][i - 1].Val)) orientation = 3; // vänster

                        while (true)
                        {
                            switch (orientation)
                            {
                                case 0: //ner
                                    if (maze[y + 1][x].Val == 'J')
                                    {
                                        orientation = 3;
                                    }
                                    else if (maze[y + 1][x].Val == 'L')
                                    {
                                        orientation = 1;
                                    }

                                    y++;
                                    break;

                                case 1: // höger
                                    if (maze[y][x + 1].Val == 'J')
                                    {
                                        orientation = 2;
                                    }
                                    else if (maze[y][x + 1].Val == '7')
                                    {
                                        orientation = 0;
                                    }

                                    x++;
                                    break;

                                case 2: // upp
                                    if (maze[y - 1][x].Val == 'F')
                                    {
                                        orientation = 1;
                                    }
                                    else if (maze[y - 1][x].Val == '7')
                                    {
                                        orientation = 3;
                                    }

                                    y--;
                                    break;

                                case 3: // vänster
                                    if (maze[y][x - 1].Val == 'F')
                                    {
                                        orientation = 0;
                                    }
                                    else if (maze[y][x - 1].Val == 'L')
                                    {
                                        orientation = 2;
                                    }

                                    x--;
                                    break;

                                default:
                                    break;
                            }

                            Maze2[y][x] = maze[y][x].Val;

                            if (maze[y][x].Val == 'S')
                            {
                                Maze2[y][x] = '*';
                                break;
                            }

                            dist++;
                        }

                        returnVal = dist / 2;
                        break;
                    }
                }
            }

            return returnVal;
        }

        private static long GetP2()
        {
            long returnVal = 0;

            for (int row = 0; row < Maze2.Length; row++)
            {
                var outside = true;
                char turning = ' ';
                for (int col = 0; col < Maze2[0].Length; col++)
                {
                    char val = Maze2[row][col];
                    bool onALine;
                    if ("*-FLJ7|".Contains(val))
                    {
                        onALine = true;
                        // something happens
                        switch (val)
                        {
                            case '*': outside = !outside; break;
                            case '|': outside = !outside; break;
                            case 'F': turning = 'F'; break;
                            case 'L': turning = 'L'; break;
                            case '7': if (turning == 'L') outside = !outside; break;
                            case 'J': if (turning == 'F') outside = !outside; break;
                            default: break;
                        }
                    }
                    else
                    {
                        onALine = false;
                    }

                    if (outside == false && onALine == false)
                    {
                        Maze2[row][col] = '&';
                        returnVal++;
                    }
                }
                Console.WriteLine(Maze2[row]);
            }

            return returnVal;
        }
    }

    internal class Pos
    {
        public long X;
        public long Y;
        public char Val = ' ';

        public override string ToString()
        {
            return $"{Val} {Y} {X}";
        }
    }
}
