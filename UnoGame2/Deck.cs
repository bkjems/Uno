﻿using System;
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
        public List<Card> Cards { get; set; } = new List<Card>();

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
						// two 1s, two 2s, two 3s..and two 9s
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

            // W=Wilds(4) and X-Draw 4 Wilds(4)
            for (int idx = 0; idx < 4; idx++)
			{
                Cards.Add(new Card(13, Card.Color.NONE, Card.ActionType.WILD));
                Cards.Add(new Card(14, Card.Color.NONE, Card.ActionType.WILD_DRAW_4));
            }

			// randomize cards
			var rnd = new Random();
			Cards = Cards.OrderBy(item => rnd.Next()).ToList();
			if (Cards.Count != 108)
			{
				throw new Exception("Deck should have 108 cards");
			}
		}

        public List<Card> GetCardsFromDeck(int count)
		{
			var cardList = new List<Card>();
			if (count <= 0)
			{
				return cardList;
			}

			IEnumerable<Card> cardsNotDealt = Cards.Where(c => c.Dealt == false);
            if (cardsNotDealt.Any())
            {
                cardList = cardsNotDealt.Take(count).ToList();
                foreach (Card card in cardList)
                {
					// find card in Cards and set dealt to true
					int cardIndex = Cards.IndexOf(card);
					if (cardIndex < 0)
					{
						throw new Exception("Card not found in deck");
					}
					Cards[cardIndex].Dealt = true;
                }
			}

			if (cardList.Count == count)
			{
				return cardList;
			}

			// all cards have been dealt, resuffle cards	
			Console.WriteLine("Need to reset cards");
			return null;
		}
	}
}
