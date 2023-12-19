using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var partNumbers = GetPartNo(ReplaceNonNumbers(readText));

            var partNumbersFiltered = GetPartNoFiltered(partNumbers, ReplaceNumbers(readText));

            var p1 = 0;
            foreach (var partNo in partNumbersFiltered)
            {
                //Console.WriteLine($"use partNo {partNo.ToString()}");
                p1 += partNo.Id;
            }

            Console.WriteLine($"P1={p1}");

            var p2 = GetP2(partNumbers, GetConnections(ReplaceNumbers(readText)));

            Console.WriteLine($"P1={p1}");
            Console.WriteLine($"P2={p2}");

        }

        private static string[] ReplaceNumbers(string[] readText)
        {
            var result = readText;
            int i = 0;
            foreach (var row in readText)
            {
                result[i++] = row
                    .Replace('0', '.')
                    .Replace('1', '.')
                    .Replace('2', '.')
                    .Replace('3', '.')
                    .Replace('4', '.')
                    .Replace('5', '.')
                    .Replace('6', '.')
                    .Replace('7', '.')
                    .Replace('8', '.')
                    .Replace('9', '.');
            }

            return result;
        }

        private static string[] ReplaceNonNumbers(string[] readText)
        {
            var result = new string[readText.Length];
            int i = 0;
            foreach (var row in readText)
            {
                result[i++] = row
                    .Replace('#', '.')
                    .Replace('$', '.')
                    .Replace('%', '.')
                    .Replace('&', '.')
                    .Replace('*', '.')
                    .Replace('+', '.')
                    .Replace('-', '.')
                    .Replace('/', '.')
                    .Replace('=', '.')
                    .Replace('@', '.');
            }

            return result;
        }

        private static List<PartNo> GetPartNoFiltered(List<PartNo> partNumbers, string[] readText)
        {
            var result = new List<PartNo>();

            var connections = GetConnections(readText);

            foreach (var partNo in partNumbers)
            {
                var conns = connections.Where(c => c.Row == partNo.Row && c.X1 <= (partNo.X2 + 1) && c.X1 >= (partNo.X1 - 1)).ToList();
                conns.AddRange(connections.Where(c => c.Row == (partNo.Row + 1) && c.X1 <= (partNo.X2 + 1) && c.X1 >= (partNo.X1 - 1)).ToList());
                conns.AddRange(connections.Where(c => c.Row == (partNo.Row - 1) && c.X1 <= (partNo.X2 + 1) && c.X1 >= (partNo.X1 - 1)).ToList());

                if (conns.Any())
                {
                    result.Add(partNo);
                }
            }

            return result;
        }

        private static List<PartNo> GetConnections(string[] readText)
        {
            var connections = new List<PartNo>();

            var row = 1;
            foreach (var line in readText)
            {
                var posX = 1;
                var blocks = line.Split('.');
                foreach (var item in blocks)
                {
                    if (item.Length > 0)
                    {
                        //Console.WriteLine($"{item} at {posX} {row} {item.Length}");
                        var conn = new PartNo() { Id = item.Contains("*") ? 199 : 0, Row = row, X1 = posX, X2 = posX + item.Length - 1 };
                        connections.Add(conn);

                        posX += item.Length + 1;
                    }
                    else
                    {
                        posX++;
                    }
                }

                row++;
            }

            return connections;
        }

        private static int GetP2(List<PartNo> partNumbers, List<PartNo> connections)
        {
            var returnVal = 0;

            var gears = connections.Where(x => x.Id > 0).ToList();

            foreach (var gear in gears)
            {
                var parts = partNumbers.Where(part => part.Row == gear.Row && gear.X1 <= (part.X2 + 1) && gear.X1 >= (part.X1 - 1)).ToList();
                parts.AddRange(partNumbers.Where(part => part.Row == (gear.Row + 1) && gear.X1 <= (part.X2 + 1) && gear.X1 >= (part.X1 - 1)).ToList());
                parts.AddRange(partNumbers.Where(part => part.Row == (gear.Row - 1) && gear.X1 <= (part.X2 + 1) && gear.X1 >= (part.X1 - 1)).ToList());

                if (parts.Any())
                {
                    if (parts.Count() == 2)
                    {
                        returnVal += parts[0].Id * parts[1].Id;
                    }
                }
            }

            return returnVal;
        }

        private static List<PartNo> GetPartNo(string[] readText)
        {
            var partNumbers = new List<PartNo>();

            var row = 1;
            foreach (var line in readText)
            {
                var posX = 1;
                var blocks = line.Split('.');
                foreach (var item in blocks)
                {
                    if (item.Length > 0)
                    {
                        if (int.TryParse(item, out int id))
                        {
                            var partNo = new PartNo() { Id = id, Row = row, X1 = posX, X2 = posX + item.Length - 1 };
                            partNumbers.Add(partNo);
                        }
                        else
                        {
                            throw new Exception("oh no"); 
                        }

                        posX += item.Length + 1;
                    }
                    else
                    {
                        posX++;
                    }
                }

                row++;
            }

            return partNumbers;
        }
    }

    public class PartNo
    {
        public int Id { set; get; }

        public int X1 { set; get; }
        public int X2 { set; get; }
        public int Row { set; get; }

        public string ToString()
        {
            return $"{Id} at {X1}-{X2} on {Row}";
        }
    }

}