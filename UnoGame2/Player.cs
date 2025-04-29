using System;
using System.Collections.Generic;
using System.Linq;
using UnoGame2;

namespace UnoGame
{
    public class Player : IPrint
    {
        private string name;
        private List<Card> cards = new List<Card>();
        private SortedList<Card.Color, int> highestColorValues = new SortedList<Card.Color, int>();

        public Player()
        {
        }
        
        public Player(string name)
        {
            Name = name;
        }

        public string Name { get => name; set => name = value; }
        public SortedList<Card.Color, int> HighestColorValues { get => highestColorValues; set => highestColorValues = value; }
        public List<Card> Cards
        {
            get => cards;

            set
            {
                cards = value;
                SortPlayerCardsbyValue();
            }
        }

        public Card TryPlayCard(Card flippedCard)
        {
            Card highCard = null;
            var colorFound = false;
            var cardsFound = new Dictionary<Card, int>();
            var wildExists = new List<Card>();
            var wild4Exists = new List<Card>();

            SortPlayerCardsbyValue();

            foreach (Card card in Cards)
            {
                if (card.Action == Card.ActionType.WILD)
                {
                    wildExists.Add(card);
                }
                else if (card.Action == Card.ActionType.WILD_DRAW_4)
                {
                    wild4Exists.Add(card);
                }
                else
                {
                    // find possible matches based on color or number
                    bool colorMatch = card.IsColorMatch(flippedCard.GetCardColor());
                    if (colorMatch || card.IsNumberMatch(flippedCard.Number))
                    {
                        if (colorMatch)
                        {
                            colorFound = true;
                        }

                        cardsFound.Add(card, card.Number);
                    }
                }
            }

            if (wildExists.Count > 0)
            {
                // if we played the wild card and the wild color we would pick
                // is the same as the flipped card color and we have cards of that
                // color, don't play the wild card
                if (flippedCard.GetCardColor() != GetWildColor())
                {
                    var card = wildExists.First<Card>();
                    cardsFound.TryAdd(card, card.Number);
                }
            }

            // color wasn't found but wild4 exists
            if (!colorFound && wild4Exists.Count > 0)
            {
                var card = wild4Exists.First<Card>();
                cardsFound.TryAdd(card, card.Number);
            }

            if (cardsFound.Count > 0)
            {
                // return cardFound with the highest value
                highCard = cardsFound.OrderByDescending(key => key.Value).First().Key;
                return highCard;
            }

            return null;
        }

        public void PrintRemoveCard(Card card)
        {
            card.PrintCard(true, "*Played");
            Cards.Remove(card);
            SortPlayerCardsbyValue();

            if (Cards.Count == 1)
            {
                Console.WriteLine(Name + " Uno!");
            }
        }

        public void SortPlayerCardsbyValue()
        {
            HighestColorValues.Clear();
            foreach (Card card in Cards)
            {
                if (card.GetCardColor() != Card.Color.NONE)
                {
                    Card.Color color = card.GetCardColor();
                    if (HighestColorValues.TryGetValue(color, out _))
                    {
                        HighestColorValues[color] += card.Number;
                    }
                    else
                    {
                        HighestColorValues.Add(color, card.Number);
                    }
                }
            }
        }
        public Card.Color GetWildColor()
        {
            SortPlayerCardsbyValue();

            var color = Card.Color.BLACK;
            if(HighestColorValues.Count > 0)
            {
                color = HighestColorValues.OrderByDescending(key => key.Value).First().Key;
            }

            return color;
        }

        public string PrintCard(bool print = false, string p = "")
        {
            var playerCards = "";
            foreach (Card card in Cards.
                OrderBy(o => o.GetCardColor()).
                ThenByDescending(o => o.Number).ToList())
            {
                if (playerCards.Length == 0)
                {
                    playerCards = string.Format("{0} [", Name);
                }
                else
                {
                    playerCards += ", ";
                }

                playerCards += card.PrintCard();
            }

            if (playerCards != "")
            {
                Console.WriteLine(playerCards + "]");
            }

            return playerCards;
        }
    }
}
