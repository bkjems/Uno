using System.Collections.Generic;
using UnoGame;
using System;
using System.Linq;

namespace UnoGame2
{
    public class Game
    {
        public enum Rotation
        {
            RIGHT, LEFT
        };

        private int numberOfPlayers;
        private static int numberOfCards = 7;
        private Deck shuffledDeck = new(); // random order
        private List<Player> players = new();
        private Card _flippedCard = new();
        private List<Card> flippedCardCount = new();
        private Rotation rotation;

        public Game(int numberOfPlayers)
        {
            NumberOfPlayers = numberOfPlayers;

            for (int i = 1; i <= NumberOfPlayers; i++)
            {
                Players.Add(new Player($"Player_{i}"));
            }

            rotation = Game.Rotation.RIGHT;
        }

        public Deck ShuffledDeck { get => shuffledDeck; set => shuffledDeck = value; }
        public Card FlippedCard 
        { 
            get => _flippedCard; 
            set
            {
                _flippedCard = value;
                flippedCardCount.Insert(0, _flippedCard);
            }
        }
        public List<Player> Players { get => players; set => players = value; }
        public int NumberOfCards { get => numberOfCards; set => numberOfCards = value; }
        public int NumberOfPlayers { get => numberOfPlayers; set => numberOfPlayers = value; }

        private Card GetCard()
        {
            return GetCards(1)[0];
        }

        public List<Card> GetCards(int numberOfCards)
        {
            var cards = ShuffledDeck.GetCardsFromDeck(numberOfCards);
            if (cards == null)
            {
                ResetDeck();
                cards = ShuffledDeck.GetCardsFromDeck(numberOfCards);
            }

            return cards;
        }

        private void ResetDeck()
        {
            ShuffledDeck = new Deck();

            flippedCardCount.Clear();

            foreach (Player p in Players)
            {
                foreach (Card playerCard in p.Cards)
                {
                    var cardFound = ShuffledDeck.Cards.Find(
                        delegate (Card shuffleCard)
                        {
                            var color = shuffleCard.GetCardColor();
                            var number = shuffleCard.Number;
                            var action = shuffleCard.Action;

                            bool rv = color == playerCard.GetCardColor() &&
                            number == playerCard.Number &&
                            action == playerCard.Action;

                            bool rv2 = color == FlippedCard.GetCardColor() &&
                            number == FlippedCard.Number &&
                            action == FlippedCard.Action;

                            return rv || rv2;
                        }
                    );

                    if (cardFound != null)
                    {
                        cardFound.Dealt = true;
                    }
                }
            }
        }

        public void DealCards(int numberOfCards)
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                foreach (Player player in Players)
                {
                    Card card = GetCard();
                    player.Cards.Add(card);
                }
            }
        }

        public void SetRotation(Rotation rotation)
        {
            this.rotation = rotation;
        }

        public Rotation GetRotation()
        {
            return rotation;
        }

        private Rotation ChangeRotation()
        {
            Rotation currentRotation = GetRotation();

            if (currentRotation == Rotation.LEFT)
            {
                SetRotation(Rotation.RIGHT);
            }
            else
            {
                SetRotation(Rotation.LEFT);
            }

            Console.WriteLine("Rotation is " + currentRotation);

            return currentRotation;
        }

        /* p1, p2, p3, p4 */
        public Player GetNextPlayer(Player currentPlayer)
        {
            var numberOfPlayers = Players.Count;
            var index = Players.IndexOf(currentPlayer);

            if (GetRotation() == Rotation.RIGHT)
            {
                if (index + 1 >= numberOfPlayers)
                {
                    index = 0;
                }
                else
                {
                    index += 1;
                }
            }
            else
            {
                if (index <= 0)
                {
                    index = numberOfPlayers - 1;
                }
                else
                {
                    index--;
                }
            }

            Player nextPlayer = Players[index];
            nextPlayer.PrintCard();
            return nextPlayer;
        }

        public Card GetStartCard()
        {
            /* If the top card is a Wild or Wild Draw 4, return it to the 
             * deck and pick another card. */
            Card card = GetCard();
            List<Card> wildCards = new List<Card>();

            while (card.Action == Card.ActionType.WILD || card.Action == Card.ActionType.WILD_DRAW_4)
            {
                wildCards.Add(card);
                card = GetCard();
            }

            // return wild cards to the deck
            foreach (Card wildCard in wildCards)
            {
                wildCard.Dealt = false;
                ShuffledDeck.Cards.Add(wildCard);
            }
            
            FlippedCard = card;
            if (FlippedCard.Dealt == false)
            {
                Console.WriteLine("FlippedCard.Dealt is false");
            }

            if (card.Action == Card.ActionType.REVERSE)
            {
                ChangeRotation();
            }

            return card;
        }

        /* The player that draws the highest card goes first. 
         * If the player draws a Wild or Wild Draw 4, return it to the deck
         * and pick another card. 
         * Afterward, return those cards to the deck, reshuffle, 
         * and deal the hands. */

        private bool HasPlayerWon()
        {
            foreach (Player player in Players)
            {
                if (player.Cards.Count == 0)
                {
                    Console.WriteLine(player.Name + " Won");
                    return true;
                }
            }

            return false;
        }

        public void PrintCards(List<Card> drawCards, String label="")
        {
            if (label != "")
            {
                Console.Write(label);
            }

            if (drawCards == null)
            {
                return;
            }

            var cardText = "";
            foreach (Card card in drawCards)
            {
                if (cardText != "")
                {
                    cardText += ", ";
                }
                cardText += card.PrintCard();
            }

            if (cardText != "")
            {
                Console.WriteLine(cardText);
            }
        }

        private Player HandleActionCards(Card flippedCard, Player currentPlayer)
        {
            if (flippedCard.Action != Card.ActionType.NONE)
            {
                List<Card> drawCards;
                switch (FlippedCard.Action)
                {
                    case Card.ActionType.DRAW_2:
                        currentPlayer = GetNextPlayer(currentPlayer);
                        drawCards = ShuffledDeck.GetCardsFromDeck(2);
                        currentPlayer.Cards.AddRange(drawCards);
                        PrintCards(drawCards, " draw 2: ");
                        break;
                    case Card.ActionType.WILD_DRAW_4:
                        flippedCard.SetCardColor(currentPlayer.GetWildColor());
                        currentPlayer = GetNextPlayer(currentPlayer);
                        drawCards = ShuffledDeck.GetCardsFromDeck(4);
                        currentPlayer.Cards.AddRange(drawCards);
                        PrintCards(drawCards, " draw 4: ");
                        break;
                    case Card.ActionType.WILD:
                        flippedCard.SetCardColor(currentPlayer.GetWildColor());
                        break;
                    case Card.ActionType.REVERSE:
                        ChangeRotation();
                        break;
                    case Card.ActionType.SKIP:
                        currentPlayer = GetNextPlayer(currentPlayer);
                        Console.WriteLine("skipped");
                        break;
                }
            }

            return currentPlayer;
        }

        public void PrintPlayers()
        {
            string playerList = string.Join(", ", Players.Select(player => player.Name));
            Console.WriteLine("Players: [" + playerList + "]");
        }

        public Player PlaysCard(Player currentPlayer)
        {
            Card cardPlayed = currentPlayer.TryPlayCard(_flippedCard);

            if (cardPlayed == null)
            {
                return HandleNoCardPlayed(currentPlayer);
            }

            /*if (cardPlayed.Dealt == false)
            {
                throw new Exception("Card played is not dealt: " + cardPlayed.PrintCard());
            }*/
            FlippedCard = cardPlayed;
            currentPlayer.PrintRemoveCard(cardPlayed);

            currentPlayer = HandleActionCards(FlippedCard, currentPlayer);
            currentPlayer = GetNextPlayer(currentPlayer);
            return currentPlayer;
        }

        private Player HandleNoCardPlayed(Player currentPlayer)
        {
            Card cardPlayed = GetCard();
            currentPlayer.Cards.Add(cardPlayed);
            cardPlayed.PrintCard(true, "Picked up ");

            if (cardPlayed.IsMatchOrWild(FlippedCard))
            {
                cardPlayed = currentPlayer.TryPlayCard(_flippedCard);
                if (cardPlayed.Action == Card.ActionType.WILD ||
                    cardPlayed.Action == Card.ActionType.WILD_DRAW_4)
                {
                    _flippedCard.SetCardColor(currentPlayer.GetWildColor());
                }
            }
            else
            {
                return GetNextPlayer(currentPlayer);
            }

            FlippedCard = cardPlayed;
            currentPlayer.PrintRemoveCard(cardPlayed);

            currentPlayer = HandleActionCards(FlippedCard, currentPlayer);
            currentPlayer = GetNextPlayer(currentPlayer);
            return currentPlayer;
        }

        public bool ValidateCardCount()
        {
            int totalCards = 0;
            int totalPlayerCards = 0;
            int totalUnDealtCards = 0;
            int totalDealtCards = 0;
            foreach (Player player in Players)
            {
                // count all dealt cards
                foreach (Card playerCard in player.Cards)
                {
                    if (playerCard.Dealt == false)
                    {
                        throw new Exception("Dealt is false: " + playerCard.PrintCard());
                    }
                }
                totalPlayerCards += player.Cards.Count;
            }

            // count all dealt = false in shuffled deck
            foreach (Card card in ShuffledDeck.Cards)
            {
                if (card.Dealt == false)
                {
                    totalUnDealtCards++;
                }
                else
                {
                    totalDealtCards++;
                }
            }

            var totalFlippedCards = flippedCardCount.Count;
            totalCards = totalPlayerCards + totalUnDealtCards + totalFlippedCards;
            
            if (totalCards != 108)
            {
                Console.WriteLine(totalCards+" = "+totalPlayerCards+ "+" + totalUnDealtCards + "+" + totalFlippedCards);
                throw new Exception("Card count is off: "+ totalCards);
            }

            return true;
        }

        public void StartGame()
        {
            PrintPlayers();
            DealCards(NumberOfCards);

            Card card = GetStartCard();
            Console.WriteLine("Flipped Card : {0}", card.PrintCard(false));

            //Player currentPlayer = GetStartPlayer();
            Player currentPlayer = Players[0];
            currentPlayer.PrintCard();

            // if top card is Draw2 first player must draw 2 cards
            if (card.Action == Card.ActionType.DRAW_2)
            {
                List<Card> drawCards = ShuffledDeck.GetCardsFromDeck(2);
                Console.Write(" Draw 2: ");
                PrintCards(drawCards);
                currentPlayer = GetNextPlayer(currentPlayer);
            }

            while (true)
            {
                if (!ValidateCardCount())
                {
                    break;
                }

                if (HasPlayerWon())
                {
                    break;
                }

                currentPlayer = PlaysCard(currentPlayer);
            }
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            Game game = new Game(7);
            game.StartGame();
        }
    }
}
