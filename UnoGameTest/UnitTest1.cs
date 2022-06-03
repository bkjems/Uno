//using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnoGame;
using UnoGame2;

namespace UnoGameTest
{
    [TestClass]
    public class UnitTest1
    {
        public Game g;

        [TestInitialize]
        public void SetUp()
        {
            g = new Game(7);
        }

        [TestMethod]
        public void DealCardsTest()
        {
            int numberOfCards = 7;
            Game game = new Game(4);
            g.DealCards(numberOfCards);

            //game.StartGame(null);
            foreach(Player p in g.Players)
            {
                Assert.AreEqual(numberOfCards, p.Cards.Count);
            }
        }

        /*[TestMethod]
        public void GetCardFromDeck1CardTest()
        {
            List<Card> cards = g.GetCards(1);
            Assert.AreEqual(1, cards.Count);
        }

        [TestMethod]
        public void GetCardFromDeckMultipleCardsTest()
        {
            List<Card> cards = g.GetCards(2);
            Assert.AreEqual(2, cards.Count);
        }*/

        [TestMethod]
        public void GetNextPlayerRightRotationTest()
        {
            int idx = 1;
            var currentPlayer = g.Players[idx]; 
            g.SetRotation(Game.Rotation.RIGHT);

            Player nextPlayer = g.GetNextPlayer(currentPlayer);

            Assert.AreEqual(g.Players[idx+1], nextPlayer);
        }

        [TestMethod]
        public void GetNextPlayerLeftRotationTest()
        {
            int idx = 2;
            var currentPlayer = g.Players[idx]; 
            g.SetRotation(Game.Rotation.LEFT);

            Player nextPlayer = g.GetNextPlayer(currentPlayer);

            Assert.AreEqual(g.Players[idx-1], nextPlayer);
        }

        [TestMethod]
        public void GetNextPlayerRightRotationLastPositionTest()
        {
            int idx = g.Players.Count-1;
            var currentPlayer = g.Players[idx]; 
            g.SetRotation(Game.Rotation.RIGHT);

            Player nextPlayer = g.GetNextPlayer(currentPlayer);

            Assert.AreEqual(g.Players[0], nextPlayer);
        }

        [TestMethod]
        public void GetNextPlayerLeftRotationLastPositionTest()
        {
            int idx = 0;
            var currentPlayer = g.Players[idx]; 
            g.SetRotation(Game.Rotation.LEFT);

            Player nextPlayer = g.GetNextPlayer(currentPlayer);

            Assert.AreEqual(g.Players[g.Players.Count-1], nextPlayer);
        }
    }
}