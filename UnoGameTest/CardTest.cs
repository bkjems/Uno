using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnoGame;
using UnoGame2;

namespace UnoGameTest
{
    [TestClass]
    public class CardTest
    {
        public Game g;

        [TestInitialize]
        public void SetUp()
        {
            g = new Game(7);
        }

        [TestMethod]
        public void IsMatchOrWildTest()
        {
            Card c = new Card(3, Card.Color.GREEN);
            Assert.AreEqual(true, c.IsMatchOrWild(new Card(3, Card.Color.BLACK, Card.ActionType.NONE)));

            c = new Card(4, Card.Color.BLACK);
            Assert.AreEqual(true, c.IsMatchOrWild(new Card(3, Card.Color.BLACK, Card.ActionType.NONE)));

            c = new Card(1, Card.Color.RED, Card.ActionType.WILD);
            Assert.AreEqual(true, c.IsMatchOrWild(new Card(3, Card.Color.BLACK, Card.ActionType.WILD)));

            c = new Card(3, Card.Color.GREEN,Card.ActionType.WILD_DRAW_4);
            Assert.AreEqual(true, c.IsMatchOrWild(new Card(3, Card.Color.BLACK, Card.ActionType.WILD_DRAW_4)));
        }

        [TestMethod]
        public void SetColorMatchTest()
        {
            Card c = new Card();
            c.SetCardColor(Card.Color.BLACK);
            Assert.AreEqual(Card.Color.BLACK, c.GetCardColor());
        }

        [TestMethod]
        public void IsColorMatchTest()
        {
            Card c = new Card(3, Card.Color.GREEN);
            Assert.AreEqual(true, c.IsColorMatch(Card.Color.GREEN));
            Assert.AreEqual(false, c.IsColorMatch(Card.Color.RED));
        }
    }
}