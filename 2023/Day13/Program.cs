using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Day13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] ss = File.ReadAllLines("indata.txt");

            var matrixes = GetData(ss);

            var p1 = GetP1(matrixes);

            Console.WriteLine($"P1={p1} 34821 is right");

            var p2 = GetP2(matrixes);

            Console.WriteLine($"P1={p1} 34821 is right");
            Console.WriteLine($"P2={p2} 36919 is right");
        }

        private static char[][][] GetData(string[] ss)
        {
            var result = new List<char[][]>();

            int last = 0;
            for (int i = 0; i < ss.Length + 1; i++)
            {
                if (i == ss.Length || ss[i].Length == 0)
                {
                    var data = ss[last..i].Select(str => str.ToArray()).ToArray();
                    result.Add(data);
                    last = i + 1;
                }
            }

            return result.ToArray();
        }

        private static long GetP1(char[][][] matrixes)
        {
            long returnVal = matrixes.Sum(matrix => Calc(matrix));

            return returnVal;
        }

        private static long GetP2(char[][][] matrixes)
        {
            long returnVal = matrixes.Sum(matrix => Calc(matrix, true));

            return returnVal;
        }

        private static long Calc(char[][] matrix, bool part2 = false)
        {
            var smudgesAllowed = part2 ? 1 : 0;

            /* cols */
            for (int col = 0; col < matrix[0].Length - 1; col++)
            {
                int smudges = 0;
                for (int row = 0; row < matrix.Length; row++)
                {
                    for (int i = col, j = col + 1; i >= 0 && j < matrix[0].Length; i--, j++)
                    {
                        if (matrix[row][i] != matrix[row][j])
                        {
                            smudges++;
                            if (smudges > smudgesAllowed)
                            {
                                break;
                            }
                        }
                    }

                    if (smudges > smudgesAllowed)
                    {
                        break;
                    }
                }

                if (smudges == smudgesAllowed)
                {
                    return col + 1;
                }
            }

            /* lines */
            for (int row = 0; row < matrix.Length - 1; row++)
            {
                int smudges = 0;
                for (int col = 0; col < matrix[0].Length; col++)
                {
                    for (int i = row, j = row + 1; i >= 0 && j < matrix.Length; i--, j++)
                    {
                        if (matrix[i][col] != matrix[j][col])
                        {
                            smudges++;
                            if (smudges > smudgesAllowed)
                            {
                                break;
                            }
                        }
                    }

                    if (smudges > smudgesAllowed)
                    {
                        break;
                    }
                }

                if (smudges == smudgesAllowed)
                {
                    return (row + 1) * 100;
                }
            }

            throw new Exception("Oh NO");
        }
    }
}
