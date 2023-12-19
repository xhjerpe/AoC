using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var games = GetGames(readText, false);

            var p1 = GetP1(games, false);

            Console.WriteLine($"P1={p1} 251927063 is right");

            games = GetGames(readText, true);
            var p2 = GetP1(games, true);

            Console.WriteLine($"P1={p1} 251927063 is right");
            Console.WriteLine($"P2={p2} 255644661 is too high 255641745 is too high");

        }

        private static List<Game> GetGames(string[] readText, bool joker)
        {
            var result = new List<Game>();

            foreach (var line in readText)
            {
                var game = new Game();
                var gameTxt = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                game.Cards = gameTxt[0].Trim();
                game.Sum = int.Parse(gameTxt[1].Trim());
                game.Type = GetType(game.Cards, joker);

                result.Add(game);
            }

            return result.OrderByDescending(X => X.Type).ToList();
        }

        private static int GetType(string cards, bool joker)
        {
            var sorted = cards.OrderBy(x => x.ToString());

            int[] res = joker ? new int[12] : new int[13];
            int jokers = 0;

            foreach (var card in sorted)
            {
                switch (card)
                {
                    case 'J':
                        if (joker)
                        {
                            jokers++;
                        }
                        else
                        {
                            res[12]++;
                        }
                        break;
                    case 'A':
                        res[11]++;
                        break;
                    case 'K':
                        res[10]++;
                        break;
                    case 'Q':
                        res[9]++;
                        break;
                    case 'T':
                        res[8]++;
                        break;
                    case '9':
                        res[7]++;
                        break;
                    case '8':
                        res[6]++;
                        break;
                    case '7':
                        res[5]++;
                        break;
                    case '6':
                        res[4]++;
                        break;
                    case '5':
                        res[3]++;
                        break;
                    case '4':
                        res[2]++;
                        break;
                    case '3':
                        res[1]++;
                        break;
                    case '2':
                        res[0]++;
                        break;

                }
            }

            var type = 0;

            var list = res.ToList().OrderByDescending(x => x).Where(x => x > 0).ToList();
            switch (list.Count)
            {
                case 0: // five of a kind 
                    type = 1;
                    break;

                case 1: // five of a kind
                    type = 1;
                    break;

                case 2:
                    if (joker)
                    {
                        // +0; 4-1 3-2  +1: 3-1 2-2  +2: 2-1
                        if (jokers == 0)
                        {
                            // four of a kind (2) OR full house (3)
                            type = list[0] > 3 ? 2 : 3;
                        }
                        else if (jokers == 1)
                        {
                            // can make four of a kind (2) OR full house (3)
                            type = list[0] > 2 ? 2 : 3;
                        }
                        else
                        {
                            // can make four of a kind (2)
                            type = 2;
                        }
                    }
                    else
                    {
                        // four of a kind (2) OR full house (3)
                        type = list[0] > 3 ? 2 : 3;
                    }
                    break;

                case 3:
                    if (joker)
                    {
                        // +0; 3-1-1, 2-2-1   +1: 2-1-1  +2: 1-1-1
                        if (jokers == 0)
                        {
                            // two pair (5) OR three of kind (4)
                            type = list[0] > 2 ? 4 : 5;
                        }
                        else
                        {
                            // can make three of a kind (4)
                            type = 4;
                        }
                    }
                    else
                    {
                        // two pair (5) OR three of kind (4)
                        type = list[0] > 2 ? 4 : 5;
                    }
                    break;

                case 4:
                    if (joker)
                    {
                        // +0; 2-1-1-1   +1: 1-1-1-1
                        // one pair (6)
                        type = 6;
                    }
                    else
                    {
                        // one pair (6)
                        type = 6;
                    }
                    break;

                case 5:
                    // high
                    type = 7;
                    break;
                default:
                    break;
            }

            return type;
        }

        private static int GetP1(List<Game> games, bool joker)
        {
            var returnVal = 0;

            var gamesSorted = games
                .OrderBy(x => x.Cards, new MyComparer(joker))
                .OrderByDescending(x => x.Type)
                .ToList();

            int i = 1;

            foreach (var game in gamesSorted)
            {
                Console.WriteLine($"{game.ToString()}");
                returnVal += i * game.Sum;
                i++;
            }

            return returnVal;
        }
    }

    public class MyComparer : IComparer<string>
    {
        private bool joker;

        public MyComparer(bool joker)
        {
            this.joker = joker;
        }

        public int Compare(string? cardsA, string? cardsB)
        {
            int i = 0;
            var x = cardsA ?? "";
            var y = cardsB ?? "";
            foreach (var card in x)
            {
                if (char.IsNumber(card))
                {
                    if (char.IsNumber(y[i]))
                    {
                        int a = int.Parse(card.ToString());
                        int b = int.Parse(y[i].ToString());
                        if (a != b)
                        {
                            return a - b;
                        }   
                    }
                    else
                    {
                        if (joker && y[i] == 'J')
                        {
                            return 1;
                        }
                        return -1;
                    }
                }
                else
                {
                    if (char.IsNumber(y[i]))
                    {
                        if (joker && card == 'J')
                        {
                            return -1;
                        }
                        return 1;
                    }
                    else
                    {
                        if (y[i] != card)
                        {
                            // both non number and unequal
                            switch (card)
                            {
                                case 'A':
                                    return 1;

                                case 'K':
                                    return y[i] == 'A' ? -1 : 1;

                                case 'Q':
                                    return y[i] == 'A' || y[i] == 'K' ? -1 : 1;

                                case 'J':
                                    return joker ? -1 : y[i] == 'T' ? 1 : -1;

                                case 'T':
                                    return joker ? y[i] == 'J' ? 1 : -1 : -1;
                            }
                        }
                    }
                }
                i++;
            }

            return 0;
        }

    }

    public class Game
    {
        public string Cards { set; get; }
        public int Type { set; get; }
        public int Sum { set; get; }

        public string ToString()
        {
            return $"{Cards} is {Type} sum={Sum}";
        }
    }

}