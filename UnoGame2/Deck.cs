using System;
using System.Collections.Generic;
using System.Linq;

namespace UnoGame
{
	/*
	108 cards as follows
	19 Blue cards – 0-9
	19 Green cards – 0-9
	19 Red cards – 0-9
	19 Yellow cards – 0-9
	8 Draw Two cards – 2 each in blue, green, red, and yellow
	8 Reverse cards – 2 each in blue, green, red, and yellow
	8 Skip cards – 2 each in blue, green, red, and yellow
	4 Wild cards
	4 Wild Draw Four cards 
	 */
	public class Deck
	{
		private List<Card> deckOfCards = new List<Card>();
        public List<Card> Cards { get => deckOfCards; set => deckOfCards = value; }

        public Deck(List<Card> cards)
        {
            Cards = cards;
        }

        public Deck()
		{
			// Create cards, one 0 card
			foreach (Card.Color color in (Card.Color[])Enum.GetValues(typeof(Card.Color)))
			{
				if (color == Card.Color.NONE)
				{
					continue;
				}

				for (int number = 0; number <= 9; number++)
				{
					if (number == 0)
					{
						Cards.Add(new Card(number, color));
					}
					else
					{
						// two 1s, two 2s, two 2s, two 3s..and 9s
						Cards.Add(new Card(number, color));
						Cards.Add(new Card(number, color));
					}
				}
			}

			// create other cards, R=Reverse(8), S=Skip(8), D=Draw 2(8) 2 in each color
			foreach (Card.Color color in (Card.Color[])Enum.GetValues(typeof(Card.Color)))
			{
				if (color == Card.Color.NONE)
				{
					continue;
				}

				Cards.Add(new Card(10, color, Card.ActionType.REVERSE));
				Cards.Add(new Card(10, color, Card.ActionType.REVERSE));
				Cards.Add(new Card(11, color, Card.ActionType.SKIP));
				Cards.Add(new Card(11, color, Card.ActionType.SKIP));
				Cards.Add(new Card(12, color, Card.ActionType.DRAW_2));
				Cards.Add(new Card(12, color, Card.ActionType.DRAW_2));
			}

			// W=Wilds(4), 
			Cards.Add(new Card(13, Card.Color.NONE, Card.ActionType.WILD));
			Cards.Add(new Card(13, Card.Color.NONE, Card.ActionType.WILD));
			Cards.Add(new Card(13, Card.Color.NONE, Card.ActionType.WILD));
			Cards.Add(new Card(13, Card.Color.NONE, Card.ActionType.WILD));

			// X-Draw 4 Wilds(4)
			Cards.Add(new Card(14, Card.Color.NONE, Card.ActionType.WILD_DRAW_4));
			Cards.Add(new Card(14, Card.Color.NONE, Card.ActionType.WILD_DRAW_4));
			Cards.Add(new Card(14, Card.Color.NONE, Card.ActionType.WILD_DRAW_4));
			Cards.Add(new Card(14, Card.Color.NONE, Card.ActionType.WILD_DRAW_4));

			// randomize cards
			var rnd = new Random();
			Cards = deckOfCards.OrderBy(item => rnd.Next()).ToList();
		}

		// for testing only
		public Card GetCardFromDeck()
		{
			try
			{
				return GetCardsFromDeck(1).First();
			}
			catch (ArgumentException)
			{
				// need to reset cards
				return null;
			}
		}

		public List<Card> GetCardsFromDeck(int numberOfCards)
		{
			if (numberOfCards <= 0)
			{
				return new List<Card>();
			}

			var cardList = new List<Card>();
			IEnumerable<Card> cardsNotDealt = Cards.Where(c => c.Dealt == false);
			if (cardsNotDealt != null && cardsNotDealt.Count<Card>() > 0)
			{
				for (int i = 0; i < numberOfCards; i++)
				{
					Card card = cardsNotDealt.ElementAt(i);
					//var c = Cards.IndexOf(card);
					cardList.Add(card);
					card.Dealt = true;
				}
			}

			if (cardList.Count == numberOfCards)
			{
				return cardList;
			}

			// all cards have been dealt, resuffle cards	
			Console.WriteLine("Need to reset cards");
			return null;
		}
	}
}
