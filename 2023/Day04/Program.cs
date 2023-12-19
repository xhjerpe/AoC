using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Open the file to read from.
            string[] readText = File.ReadAllLines("indata.txt");

            //string[] readText = new string[6];

            //readText[0] = "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53";
            //readText[1] = "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19";
            //readText[2] = "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1";
            //readText[3] = "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83";
            //readText[4] = "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36";
            //readText[5] = "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";

            var cards = GetCards(readText);

            var p1 = GetP1(cards);
            Console.WriteLine($"P1={p1}");

            var p2 = GetP2(cards);

            Console.WriteLine($"P1={p1}");
            Console.WriteLine($"P2={p2}");
        }

        private static List<Card> GetCards(string[] readText)
        {
            var result = new List<Card>();

            foreach (var line in readText)
            {
                var id = int.Parse(line.Split(":")[0].Replace("Card", "").Trim());
                var card = new Card() { Id = id };

                var numbers = line.Split(":")[1].Split("|");

                var wins = numbers[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                var myNo = numbers[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                foreach (var w in wins)
                {
                    card.Drows.Add(int.Parse(w));
                }

                foreach (var m in myNo)
                {
                    card.MyNos.Add(int.Parse(m));
                }

                result.Add(card);
            }

            return result;

        }

        private static int GetP1(List<Card> cards)
        {
            var value = 0;

            foreach (var card in cards)
            {
                var hits = 0;
                var points = 0;
                foreach (var item in card.Drows)
                {
                    if (card.MyNos.Contains(item))
                    {
                        points = points == 0 ? 1 : points * 2;
                        hits += 1;
                    }
                };

                value += points;

                Console.WriteLine($"{card.Id} gives {hits}({points})");
            }

            return value;
        }

        private static int GetP2(List<Card> cards)
        {
            var allCards = new List<Card>(cards);

            allCards = GetMoreCards(allCards, 1, cards);

            return allCards.Count;
        }

        private static List<Card> GetMoreCards(List<Card> allCards, int currentId, List<Card> orgCards)
        {
            var cardsToAdd = new List<Card>();
            foreach (var card in allCards)
            {
                if (card.Id != currentId)
                {
                    continue;
                }

                var points = 0;
                foreach (var item in card.Drows)
                {
                    if (card.MyNos.Contains(item))
                    {
                        points += 1;
                    }
                };

                if (points > 0)
                {
                    var toAdd = orgCards.Where(x => x.Id > card.Id && x.Id < (card.Id + points + 1)).ToList();
                    cardsToAdd.AddRange(toAdd);
                }
            }

            allCards.AddRange(cardsToAdd);

            if (currentId > orgCards.Count)
            {
                return allCards;
            }
            return GetMoreCards(allCards, currentId + 1, orgCards);
        }
    }

    internal class Card
    {
        public int Id { set; get; }

        public List<int> Drows { set; get; } = new List<int>();

        public List<int> MyNos { set; get; } = new List<int>();

        public override string ToString()
        {
            var info = "";
            Drows.ForEach(x => info += x.ToString());
            var info2 = "";
            MyNos.ForEach(x => info2 += x.ToString());
            return $"{Id} : {info} : {info2}";
        }

    }
}