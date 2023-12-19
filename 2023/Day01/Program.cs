using System;
using System.IO;

namespace day1
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
            Console.WriteLine($"P1={p1} (54667 is correct)");
            Console.WriteLine($"P2={p2} (53623 is too low)");
        }

        private static string GetP1(string[] readText, bool includeText)
        {
            var total = 0;

            foreach (string s in readText)
            {
                int a = 0;
                int b = 0;

                string work = "";

                for (int i = 0; i < s.Length; i++)
                {
                    if (((byte)s[i]) < 58)
                    {
                        work += s[i];
                    }

                    if (includeText)
                    {
                        if ((s.Length - i + 1) > 3 && s.Substring(i, 3) == "one")
                        {
                            work += "1";
                        }
                        if ((s.Length - i + 1) > 3 && s.Substring(i, 3) == "two")
                        {
                            work += "2";
                        }
                        if ((s.Length - i + 1) > 5 && s.Substring(i, 5) == "three")
                        {
                            work += "3";
                        }
                        if ((s.Length - i + 1) > 4 && s.Substring(i, 4) == "four")
                        {
                            work += "4";
                        }
                        if ((s.Length - i + 1) > 4 && s.Substring(i, 4) == "five")
                        {
                            work += "5";
                        }
                        if ((s.Length - i + 1) > 3 && s.Substring(i, 3) == "six")
                        {
                            work += "6";
                        }
                        if ((s.Length - i + 1) > 5 && s.Substring(i, 5) == "seven")
                        {
                            work += "7";
                        }
                        if ((s.Length - i + 1) > 5 && s.Substring(i, 5) == "eight")
                        {
                            work += "8";
                        }
                        if ((s.Length - i + 1) > 4 && s.Substring(i, 4) == "nine")
                        {
                            work += "9";
                        }
                    }
                }

                for (int i = 0; i < work.Length; i++)
                {
                    if (int.TryParse($"{work[i]}", out int result))
                    {
                        a = result;
                        break;
                    }
                }
                for (int i = work.Length - 1; i > -1; i--)
                {
                    if (int.TryParse($"{work[i]}", out int result))
                    {
                        b = result;
                        break;
                    }
                }

                total += a * 10 + b;
                Console.WriteLine($"{s} -> {work} -> {a}{b} -> {total}");
            }

            Console.WriteLine($"{total}");

            return total.ToString();
        }
    }
}