using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            var games = GetGames(readText);

            var gamesFiltered = GamesFiltering(games);

            var p1 = 0;
            foreach (var game in gamesFiltered )
            {
                Console.WriteLine($"use game {game.ToString()}");
                p1 += game.Id;
            }

            Console.WriteLine($"P1={p1}");

            var p2 = GetP2(games);

            Console.WriteLine($"P1={p1}");
            Console.WriteLine($"P2={p2}");

        }

        private static int GetP2(List<Game> games)
        {
            var returnVal = 0;
            foreach (var game in games)
            {
                returnVal += AnalyzeGame(game);
            }

            return returnVal;
        }

        private static int AnalyzeGame(Game game)
        {
            var power = 0;

            var maxRed = 0;
            var maxGreen = 0;
            var maxBlue = 0;

            foreach (var item in game.Reveals)
            {
                if (item.Red > maxRed)
                {
                    maxRed = item.Red;
                }
                if (item.Green > maxGreen)
                {
                    maxGreen = item.Green;
                }
                if (item.Blue > maxBlue)
                {
                    maxBlue = item.Blue;
                }
            }

            power = maxRed * maxGreen * maxBlue;

            return power;
        }

        private static List<Game> GamesFiltering(List<Game> games)
        {
            var result = new List<Game>();

            foreach (var game in games)
            {
                var valid = true;
                foreach (var rev in game.Reveals)
                {
                    if (rev.Red > 12 || rev.Green > 13 || rev.Blue > 14)
                    {
                        Console.WriteLine($"remove game {game.ToString()}");
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    result.Add(game);
                }
            }

            return result;
        }

        private static List<Game> GetGames(string[] readText)
        {
            var games = new List<Game>();

            foreach (var line in readText)
            {
                var id = int.Parse(line.Split(":")[0].Replace("Game", "").Trim());
                var game = new Game() { Id = id };

                var reveals = line.Split(":")[1].Split(";");
                Console.WriteLine($"{id} {reveals.Count()}");

                foreach (var block in reveals)
                {
                    var reveal = new Reveal();
                    var counts = block.Split(",");
                    foreach (var count in counts)
                    {
                        if (count.Contains("red"))
                        {
                            reveal.Red = int.Parse(count.Replace("red", "").Trim());
                        }
                        if (count.Contains("green"))
                        {
                            reveal.Green = int.Parse(count.Replace("green", "").Trim());
                        }
                        if (count.Contains("blue"))
                        {
                            reveal.Blue = int.Parse(count.Replace("blue", "").Trim());
                        }
                    }
                    game.Reveals.Add(reveal);
                }

                games.Add(game);
            }

            return games;
        }
    }

    public class Game
    {
        public int Id { set; get; }

        public List<Reveal> Reveals { set; get; } = new List<Reveal>();

        public string ToString()
        {
            var info = "";
            Reveals.ForEach(x => info += x.ToString());
            return $"{Id} : {info}";
        }
    }

    public class Reveal
    {
        public int Red { set; get; }
        public int Green { set; get; }
        public int Blue { set; get; }

        public string ToString()
        {
            return $" {Red}:{Green}:{Blue} /";
        }
    }
}