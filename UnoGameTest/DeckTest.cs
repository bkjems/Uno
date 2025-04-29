using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnoGame;
using UnoGame2;

namespace UnoGameTest
{
    [TestClass]
    public class DeckTest
    {
        public Game g;

        [TestInitialize]
        public void SetUp()
        {
            g = new Game(7);
        }

        [TestMethod]
        public void DeckInitTest()
        {
            Deck d = new Deck();
            Assert.AreEqual(108, d.Cards.Count);
        }

        [TestMethod]
        public void GetCardsFromDeckTest()
        {
            Deck d = new Deck();
            int numberOfCards = 4;
            List<Card> cards = d.GetCardsFromDeck(numberOfCards);
            Assert.AreEqual(numberOfCards, cards.Count);
        }

       [TestMethod]
        public void GetCardsFromDeckNegativeTest()
        {
            Deck d = new Deck();
            int numberOfCards = -1;
            List<Card> cards = d.GetCardsFromDeck(numberOfCards);
            Assert.AreEqual(0, cards.Count);
        }

        [TestMethod]
        public void GetCardFromDeckDealtCardsCountTest()
        {
            Deck d = new Deck();

            // setup deck
            var c = new Card(9, Card.Color.GREEN, Card.ActionType.NONE);
            c.Dealt = true;
            var c2 = new Card(7, Card.Color.BLACK, Card.ActionType.NONE);
            c2.Dealt = true;
 
            List<Card> cards = new List<Card>
            {
                c,
                new Card(8, Card.Color.BLACK, Card.ActionType.NONE),
                c2,
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(3, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(3, Card.Color.RED, Card.ActionType.NONE),
                new Card(12, Card.Color.YELLOW, Card.ActionType.DRAW_2),
                new Card(12, Card.Color.YELLOW, Card.ActionType.DRAW_2),
             };

            d.Cards = cards;
            List<Card> cardList = d.GetCardsFromDeck(3);
            Assert.AreEqual(3, cardList.Count);
        }

        [TestMethod]
        public void GetCardFromDeckNoMoreCardsTest()
        {
            //reset deck test
            g = new Game(2);

            g.ShuffledDeck = new Deck();

            // setup deck
            var c = new Card(9, Card.Color.GREEN, Card.ActionType.NONE);
            c.Dealt = true;
            var c2 = new Card(7, Card.Color.BLACK, Card.ActionType.NONE);
            c2.Dealt = true;

            List<Card> cards = new List<Card>
            {
                c,
                c2,
             };

            //d.Cards = cards;
            //g.ShuffledDeck = d;
            //List<Card> cardList = d.GetCards(3);
            //Assert.AreEqual(0, cardList.Count);
        }
    
        [TestMethod]
        public void GetCardsFromDeck_ValidNumberOfCards_ReturnsCorrectCount()
        {
            // Arrange
            Deck deck = new Deck();
            int numberOfCards = 5;

            // Act
            List<Card> cards = deck.GetCardsFromDeck(numberOfCards);

            // Assert
            Assert.IsNotNull(cards);
            Assert.AreEqual(numberOfCards, cards.Count);
            Assert.IsTrue(cards.All(c => c.Dealt));
        }

        [TestMethod]
        public void GetCardsFromDeck_ZeroCardsRequested_ReturnsEmptyList()
        {
            // Arrange
            Deck deck = new Deck();

            // Act
            List<Card> cards = deck.GetCardsFromDeck(0);

            // Assert
            Assert.IsNotNull(cards);
            Assert.AreEqual(0, cards.Count);
        }

        [TestMethod]
        public void GetCardsFromDeck_NegativeNumberOfCards_ReturnsEmptyList()
        {
            // Arrange
            Deck deck = new Deck();

            // Act
            List<Card> cards = deck.GetCardsFromDeck(-5);

            // Assert
            Assert.IsNotNull(cards);
            Assert.AreEqual(0, cards.Count);
        }

        [TestMethod]
        public void GetCardsFromDeck_NotEnoughCardsAvailable_ReturnsNull()
        {
            // Arrange
            Deck deck = new Deck();
            foreach (var card in deck.Cards)
            {
                card.Dealt = true; // Mark all cards as dealt
            }

            // Act
            List<Card> cards = deck.GetCardsFromDeck(5);

            // Assert
            Assert.IsNull(cards);
        }

        [TestMethod]
        public void GetCardsFromDeck_PartialCardsAvailable_ReturnsAvailableCards()
        {
            // Arrange
            Deck deck = new Deck();
            deck.Cards[0].Dealt = true; // Mark the first card as dealt
            int numberOfCards = 3;

            // Act
            List<Card> cards = deck.GetCardsFromDeck(numberOfCards);

            // Assert
            Assert.IsNotNull(cards);
            Assert.AreEqual(numberOfCards, cards.Count);
            Assert.IsTrue(cards.All(c => c.Dealt));
        }
    }
}
