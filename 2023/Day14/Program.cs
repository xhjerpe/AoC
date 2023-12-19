using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("indata.txt");

            var p1 = GetP1(lines.Select(str => str.ToArray()).ToArray());

            Console.WriteLine($"P1={p1} 108614 is right");

            var p2 = GetP2(lines.Select(str => str.ToArray()).ToArray());

            Console.WriteLine($"P1={p1} 108614 is right");
            Console.WriteLine($"P2={p2} 96447 is right");
        }

        private static long GetP1(char[][] indata)
        {
            long returnVal = 0;

            var matrix = FlipMatrix(indata);

            for (var row = 0; row < matrix.Length; row++)
            {
                returnVal += Calc(Shuffle(matrix[row]));
            }

            return returnVal;
        }

        private static long GetP2(char[][] indata)
        {
            long returnVal = 0;

            Dictionary<long, long> perm = new Dictionary<long, long>();

            var matrix = indata;
            Dump(matrix, 0, "");
            long length = 0;
            for (var cycle = 0; cycle < 300; cycle++)
            {
                // flip and shuffle 4 times
                for (var flip = 0; flip < 4; flip++)
                {
                    matrix = FlipMatrix(matrix);
                    for (var row = 0; row < matrix.Length; row++)
                    {
                        matrix[row] = Shuffle(matrix[row], flip > 1);
                    }
                }

                var hsh = ComputeHash(matrix);
                if (perm.ContainsKey(hsh))
                {
                    length = cycle - perm[hsh];
                    break;

                }
                else
                {
                    var matrix2 = FlipMatrix(matrix);
                    long sum = 0;
                    for (var row = 0; row < matrix2.Length; row++)
                    {
                        sum += Calc(matrix2[row]);
                    }

                    Console.WriteLine($"cycle={cycle} add hash={hsh} sum={sum}");
                    perm.Add(hsh, cycle);
                }
            }

            long rem = 1000000000 % length;

            for (var cycle = 0; cycle < ((2 * length) + rem - 173 - 1); cycle++)
            {
                // flip and shuffle 4 times
                for (var flip = 0; flip < 4; flip++)
                {
                    matrix = FlipMatrix(matrix);
                    for (var row = 0; row < matrix.Length; row++)
                    {
                        matrix[row] = Shuffle(matrix[row], flip > 1);
                    }
                }
            }
            Dump(matrix, 1000000000, "");

            var matrixFinal = FlipMatrix(matrix);
            for (var row = 0; row < matrixFinal.Length; row++)
            {
                returnVal += Calc(matrixFinal[row]);
            }

            return returnVal;
        }

        private static long ComputeHash(char[][] matrix)
        {
            int hash = 0;
            for (int row = 0; row < matrix.Length; row++)
            {
                for (int col = 0; col < matrix[0].Length; col++)
                {
                    hash += ((short) matrix[row][col]) * (row + 1) * (col + 1);
                }
            }

            return hash;
        }

        private static void Dump(char[][] matrix, long ix, string v)
        {
            Console.WriteLine($"{v} ix={ix}");
            for (var row = 0; row < matrix.Length; row++)
            {
                Console.Write(v);
                Console.Write(matrix[row]);
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        private static long Calc(char[] chars)
        {
            var sum = 0;

            for (var i = 0; i < chars.Length; i++)

            {
                if (chars[i] == 'O')
                {
                    sum += chars.Length - i;
                }
            }

            return sum;
        }

        private static char[] Shuffle(char[] chars, bool reverse = false)
        {
            bool swch = true;
            while (swch)
            {
                swch = false;
                for (var i = 0; i + 1 < chars.Length; i++)
                {
                    if (reverse)
                    {
                        if (chars[i] == 'O' && chars[i + 1] == '.')
                        {
                            chars[i] = '.';
                            chars[i + 1] = 'O';
                            swch = true;
                        }

                    }
                    else
                    {
                        if (chars[i] == '.' && chars[i + 1] == 'O')
                        {
                            chars[i] = 'O';
                            chars[i + 1] = '.';
                            swch = true;
                        }
                    }
                }
            }

            return chars;
        }

        private static char[][] FlipMatrix(char[][] rows)
        {
            var flipped = new char[rows[0].Length][];

            for (var col = 0; col < rows[0].Length; col++)
            {
                flipped[col] = new char[rows.Length];
                for (var row = 0; row < rows.Length; row++)
                {
                    flipped[col][row] = rows[row][col];
                }
            }

            return flipped;
        }

    }
}
