using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Day19
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] readText = File.ReadAllLines("indata.txt");

            var workFlows = new Dictionary<string, List<ARule>>();
            foreach (string line in readText)
            {
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var data = line.Split("{");
                var rules = new List<ARule>();

                foreach (var rule in data[1].Split(","))
                {
                    rules.Add(new ARule(rule));
                }

                workFlows.Add(data[0], rules);
            }
            var ratings = new List<Rating>();
            for (int i = workFlows.Count; i < readText.Length; i++)
            {
                if (string.IsNullOrEmpty(readText[i]))
                {
                    continue;
                }

                var data = readText[i].Trim('{').Trim('}').Split(",", StringSplitOptions.RemoveEmptyEntries);

                var rating = new Rating();
                for (int j = 0; j < data.Length; j++)
                {
                    rating.Xmas[j] = int.Parse(data[j].Split("=", StringSplitOptions.RemoveEmptyEntries)[1]);
                }

                ratings.Add(rating);
            }


            var p1 = GetP1(workFlows, ratings);

            Console.WriteLine($"P1={p1} 389114 is right");

            var p2 = GetP2(readText[0..workFlows.Count]);

            Console.WriteLine($"P1={p1} 389114 is right");
            Console.WriteLine($"P2={p2} ?? is right");
        }


        private static long GetP1(Dictionary<string, List<ARule>> workFlows, List<Rating> ratings)
        {
            long returnVal = 0;

            foreach (var rating in ratings)
            {
                var sum = Calc(rating, workFlows);
                returnVal += sum;
            }

            return returnVal;
        }

        private static long Calc(Rating rating, Dictionary<string, List<ARule>> workFlows)
        {
            var ruleLst = workFlows["in"];

            while (ruleLst != null)
            {
                bool IsTimeToBreak = false;
                foreach (var rule in ruleLst)
                {
                    if (rule.RuleType == RuleType.Next)
                    {
                        ruleLst = workFlows[rule.Next];
                        break;
                    }

                    switch (rule.RuleType)
                    {
                        case RuleType.Accept:
                            return rating.Xmas[0] + rating.Xmas[1] + rating.Xmas[2] + rating.Xmas[3];

                        case RuleType.Reject:
                            return 0;

                        case RuleType.Next:
                            break;

                        case RuleType.Less:
                            {
                                // check Max
                                if (rating.Xmas[rule.Index] < rule.Max)
                                {
                                    // take action
                                    if (rule.IsReject)
                                    {
                                        return 0;
                                    }
                                    if (rule.IsAccept)
                                    {
                                        return rating.Xmas[0] + rating.Xmas[1] + rating.Xmas[2] + rating.Xmas[3];
                                    }

                                    ruleLst = workFlows[rule.Next];
                                    IsTimeToBreak = true;
                                }
                            }
                            break;

                        case RuleType.More:
                            {
                                // check Min
                                if (rating.Xmas[rule.Index] > rule.Min)
                                {
                                    // take action
                                    if (rule.IsReject)
                                    {
                                        return 0;
                                    }
                                    if (rule.IsAccept)
                                    {
                                        return rating.Xmas[0] + rating.Xmas[1] + rating.Xmas[2] + rating.Xmas[3];
                                    }

                                    ruleLst = workFlows[rule.Next];
                                    IsTimeToBreak = true;
                                }
                            }
                            break;
                    }

                    if (IsTimeToBreak)
                    {
                        break;
                    }
                }
            };

            return 0;
        }

        private static long GetP2(string[] lines)
        {
            long returnVal = 0;

            var rules = new Dictionary<string, string>();

            foreach (string line in lines)
            {
                var strs = line.Split("{");
                rules.Add(strs[0], strs[1].TrimEnd('}'));
            }

            var dataStr = rules["in"].ToCharArray();

            var resultString = new StringBuilder();

            for (int i = 0, lastPos = 0; i < dataStr.Length; i++)
            {
                var ss = new string(dataStr[lastPos..i]);
                if (rules.TryGetValue(ss, out string cache))
                {

                }
            }


            return returnVal;
        }
    }

    internal class Rating
    {
        public Rating()
        {
            Xmas = new long[4];
        }
        public long[] Xmas { get; set; }
    }

    public enum RuleType
    {
        Reject,
        Accept,
        Next,
        Less,
        More
    }

    internal class ARule
    {

        public ARule(string rule)
        {
            Org = rule;
            Next = rule.TrimEnd('}');

            if (rule.Contains(":", StringComparison.CurrentCulture))
            {
                var data = rule.Split(":");
                if (data[1].Contains("A", StringComparison.CurrentCulture))
                {
                    IsAccept = true;
                }
                else if (data[1].Contains("R", StringComparison.CurrentCulture))
                {
                    IsReject = true;
                }
                Next = data[1];

                if (data[0][1] == '>')
                {
                    RuleType = RuleType.More;
                    Min = int.Parse(data[0][2..]);
                    switch (data[0][0])
                    {
                        case 'x':
                            Index = 0;
                            break;
                        case 'm':
                            Index = 1;
                            break;
                        case 'a':
                            Index = 2;
                            break;
                        case 's':
                            Index = 3;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    RuleType = RuleType.Less;
                    Max = int.Parse(data[0][2..]);
                    switch (data[0][0])
                    {
                        case 'x':
                            Index = 0;
                            break;
                        case 'm':
                            Index = 1;
                            break;
                        case 'a':
                            Index = 2;
                            break;
                        case 's':
                            Index = 3;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                RuleType = RuleType.Next;
                if (rule.Contains("A", StringComparison.CurrentCulture))
                {
                    RuleType = RuleType.Accept;
                    IsAccept = true;
                }
                else if (rule.Contains("R", StringComparison.CurrentCulture))
                {
                    RuleType = RuleType.Reject;
                    IsReject = true;
                }
            }
        }

        public int Index { get; set; }
        public int Max { get; set; } = -1;
        public int Min { get; set; } = -1;
        public RuleType RuleType { get; set; }

        public string Org { get; set; }

        public string Next { get; set; }
        public bool IsAccept { get; set; } = false;
        public bool IsReject { get; set; } = false;
    }
}
