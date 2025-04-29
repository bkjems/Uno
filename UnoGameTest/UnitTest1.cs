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

        //[TestMethod]
        public void GetStartPlayerTest()
        {
            Game game = new Game(7);
            game.ShuffledDeck.Cards[0].Number = 1;
            game.ShuffledDeck.Cards[1].Number = 2;
            game.ShuffledDeck.Cards[2].Number = 5;
            game.ShuffledDeck.Cards[3].Number = 5;
            game.ShuffledDeck.Cards[4].Number = 3;
            game.ShuffledDeck.Cards[5].Number = 12;
            game.ShuffledDeck.Cards[6].Number = 12;
            //game.DealCards(game.NumberOfCards);

            Player startPlayer = game.Players[0];
            Assert.IsNotNull(startPlayer);

            if (startPlayer.Name == "Player_6")
            {
                Assert.AreEqual("Player_6", startPlayer.Name);
            }
            else if (startPlayer.Name == "Player_7")
            {
                Assert.AreEqual("Player_7", startPlayer.Name);
            }
            else
            {
                Assert.IsTrue(false);
            }
            
            Assert.AreEqual(1,startPlayer.Cards.Count);
        }

        [TestMethod]
        public void GetStartCardTest()
        {
            Game game = new Game(7);

            game.ShuffledDeck.Cards[0].Action = Card.ActionType.WILD;
            game.ShuffledDeck.Cards[1].Action = Card.ActionType.WILD_DRAW_4;
            game.ShuffledDeck.Cards[2].Number = 5;

            Card startCard = game.GetStartCard();
            Assert.AreEqual(5, startCard.Number);

            // verify the card.dealt is false of card 0 and 1
            Assert.IsFalse(game.ShuffledDeck.Cards[0].Dealt);
            Assert.IsFalse(game.ShuffledDeck.Cards[1].Dealt);
        }
    }
}