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
        private Deck shuffledDeck = new Deck(); // random order
        private List<Player> players = new List<Player>();
        private Card flippedCard = new Card();
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
        public Card FlippedCard { get => flippedCard; set => flippedCard = value; }
        public List<Player> Players { get => players; set => players = value; }
        public int NumberOfCards { get => numberOfCards; set => numberOfCards = value; }
        public int NumberOfPlayers { get => numberOfPlayers; set => numberOfPlayers = value; }

        private Card GetCard()
        {
            List<Card> cards = GetCards(1);
            if (cards.Count > 0)
            {
                return cards[0];
            }

            return null;
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

                            bool rv2 = color == flippedCard.GetCardColor() &&
                            number == flippedCard.Number &&
                            action == flippedCard.Action;

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
            Rotation rotation = this.rotation;

            if (rotation == Rotation.RIGHT)
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

        private Card GetStartCard()
        {
            /* If the top card is a Wild or Wild Draw 4, return it to the 
             * deck and pick another card. */
            Card card = GetCard();

            while (card.Action == Card.ActionType.WILD || card.Action == Card.ActionType.WILD_DRAW_4)
            {
                card.Dealt = false;
                card = GetCard();
            }

            FlippedCard = card;

            if (card.Action == Card.ActionType.REVERSE)
            {
                ChangeRotation();
            }

            return card;
        }

        private Player GetStartPlayer()
        {
            // person that draws highest card goes first
            int maxNum = 0;
            Player startPlayer = null;

            foreach (Player p in Players)
            {
                Card c = GetCard();
                if (c.Number > maxNum)
                {
                    maxNum = c.Number;
                    startPlayer = p;
                }
            }

            Console.WriteLine("Starting Player: " + startPlayer.Name);
            return startPlayer;
        }

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

        public void PrintCards(List<Card> drawCards)
        {
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
                        PrintCards(drawCards);
                        break;
                    case Card.ActionType.WILD_DRAW_4:
                        flippedCard.SetCardColor(currentPlayer.GetWildColor());
                        currentPlayer = GetNextPlayer(currentPlayer);
                        drawCards = ShuffledDeck.GetCardsFromDeck(4);
                        currentPlayer.Cards.AddRange(drawCards);
                        PrintCards(drawCards);
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
            Card cardPlayed = currentPlayer.TryPlayCard(flippedCard);

            if (cardPlayed == null)
            {
                return HandleFailedCardPlay(currentPlayer);
            }

            FlippedCard = cardPlayed;
            currentPlayer.PrintRemoveCard(cardPlayed);

            currentPlayer = HandleActionCards(FlippedCard, currentPlayer);
            currentPlayer = GetNextPlayer(currentPlayer);
            return currentPlayer;
        }

        private Player HandleFailedCardPlay(Player currentPlayer)
        {
            Card cardPlayed = GetCard();
            currentPlayer.Cards.Add(cardPlayed);
            cardPlayed.PrintCard(true, "Picked up ");

            if (cardPlayed.IsMatchOrWild(FlippedCard))
            {
                cardPlayed = currentPlayer.TryPlayCard(flippedCard);
                if (cardPlayed.Action == Card.ActionType.WILD ||
                    cardPlayed.Action == Card.ActionType.WILD_DRAW_4)
                {
                    flippedCard.SetCardColor(currentPlayer.GetWildColor());
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

        public void StartGame()
        {
            PrintPlayers();
            DealCards(NumberOfCards);

            Card card = GetStartCard();
            Console.WriteLine("Flipped Card : {0}", card.PrintCard(false));

            Player currentPlayer = GetStartPlayer();
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
