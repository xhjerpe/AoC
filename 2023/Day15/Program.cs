using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllLines("indata.txt")[0].Split(',').Select(x => x.ToArray()).ToArray();

            var p1 = GetP1(data);

            Console.WriteLine($"P1={p1} 514025 is right");

            var p2 = GetP2(data);

            Console.WriteLine($"P1={p1} 514025 is right");
            Console.WriteLine($"P2={p2} 244461 is right");
        }

        private static long GetP1(char[][] data)
        {
            long returnVal = 0;

            foreach (char[] x in data)
            {
                returnVal += GetHash(x);
            }

            return returnVal;
        }

        private static long GetP2(char[][] data)
        {
            long returnVal = 0;

            Dictionary<int, List<LabelItem>> boxes = new Dictionary<int, List<LabelItem>>();
            for (int i = 0; i < 255; i++)
            {
                boxes.Add(i, new List<LabelItem>());
            }

            foreach (char[] x in data)
            {
                if (x[^1] == '-')
                {
                    // remove
                    var lbl = x[0..^1];
                    var hash = GetHash(lbl);
                    if (boxes.TryGetValue(hash, out var lst))
                    {
                        for (int i = 0; i < lst.Count; i++)
                        {
                            if (lst[i].Label.SequenceEqual(lbl))
                            {
                                lst.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // add
                    var lbl = x[0..^2];
                    var item = new LabelItem
                    {
                        Label = lbl,
                        FocalLength = int.Parse(x[^1].ToString())
                    };

                    var hash = GetHash(lbl);
                    if (boxes.TryGetValue(hash, out var lst))
                    {
                        var found = false;
                        for (int i = 0; i < lst.Count; i++)
                        {
                            if (lst[i].Label.SequenceEqual(lbl))
                            {
                                lst[i] = item;
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            lst.Add(item);
                        }
                    }
                    else
                    {
                        boxes.Add(hash, new List<LabelItem>() { item });
                    }
                }
            }

            long boxNo = 1;
            foreach (var box in boxes)
            {
                long val = 0;
                long slot = 1;
                foreach (var label in box.Value)
                {
                    val += boxNo * slot * label.FocalLength;
                    slot++;
                }

                returnVal += val;

                boxNo++;
            }

            return returnVal;
        }

        private static int GetHash(char[] x)
        {
            var val = 0;

            foreach (char c in x)
            {
                val += (short) c;
                val *= 17;
                val %= 256;
            }

            return val;
        }
    }

    internal class LabelItem
    {
        public long FocalLength { get; set; } = 0;
        public char[] Label { get; set; }

        public override string ToString()
        {
            return $"{Label} {FocalLength}";
        }
    }
}
