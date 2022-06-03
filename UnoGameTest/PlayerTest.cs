using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnoGame;
using UnoGame2;


namespace UnoGameTest
{
    [TestClass]
    public class PlayerTest
    {
        public Game g;

        [TestInitialize]
        public void SetUp()
        {
            g = new Game(7);
        }

        /* 
         * flipped card Black 3
         * found: Red 2, Green 1, Black 1
         * should return Red 3 (highest color count)
         */
        [TestMethod]
        public void PlayCardReturnHighCardTest()
        {
            Deck d = new Deck();
            Player p = new Player();
            List<Card> pcards = new System.Collections.Generic.List<Card>
            {
                new Card(3, Card.Color.RED, Card.ActionType.NONE),
                new Card(3, Card.Color.RED, Card.ActionType.NONE),
                new Card(3, Card.Color.GREEN, Card.ActionType.NONE),
                new Card(1, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(7, Card.Color.YELLOW, Card.ActionType.NONE),
                new Card(6, Card.Color.YELLOW, Card.ActionType.NONE)
             };

            p.Cards = pcards;

            g.FlippedCard = new Card(3, Card.Color.BLACK, Card.ActionType.NONE);
            Card c = p.TryPlayCard(g.FlippedCard);

            Assert.AreEqual(4, p.HighestColorValues.Count);
            Assert.AreEqual(3, c.Number);
            Assert.AreEqual(Card.Color.RED, c.GetCardColor());
        }

        [TestMethod]
        public void PlayCardReturnHighRedTest()
        {
            Deck d = new Deck();
            Player p = new Player();
            List<Card> pcards = new List<Card>
            {
                new Card(3, Card.Color.RED, Card.ActionType.NONE),
                new Card(4, Card.Color.RED, Card.ActionType.NONE),
                new Card(6, Card.Color.RED, Card.ActionType.NONE),
                new Card(3, Card.Color.GREEN, Card.ActionType.NONE),
                new Card(1, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(7, Card.Color.YELLOW, Card.ActionType.NONE),
                new Card(1, Card.Color.YELLOW, Card.ActionType.NONE)
             };

            p.Cards = pcards;

            g.FlippedCard = new Card(1, Card.Color.RED, Card.ActionType.NONE);
            Card c = p.TryPlayCard(g.FlippedCard);

            Assert.AreEqual(6, c.Number);
            Assert.AreEqual(Card.Color.RED, c.GetCardColor());
        }

        [TestMethod]
        public void PlayCardReturnWildCardTest()
        {
            Deck d = new Deck();
            Player p = new Player();
            List<Card> pcards = new List<Card>
            {
                new Card(3, Card.Color.RED, Card.ActionType.NONE),
                new Card(4, Card.Color.RED, Card.ActionType.NONE),
                new Card(6, Card.Color.RED, Card.ActionType.NONE),
                new Card(13, Card.Color.NONE, Card.ActionType.WILD),
                new Card(1, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(7, Card.Color.YELLOW, Card.ActionType.NONE),
                new Card(1, Card.Color.YELLOW, Card.ActionType.NONE)
             };

            p.Cards = pcards;

            g.FlippedCard = new Card(1, Card.Color.GREEN, Card.ActionType.NONE);
            Card c = p.TryPlayCard(g.FlippedCard);

            Assert.AreEqual(13, c.Number);
            Assert.AreEqual(Card.ActionType.WILD, c.Action);
        }

        /*Flipped Card : GREEN 12 
   Player_2 [GREEN 9, BLACK 8, BLACK 6, BLACK 3, RED 3, YELLOW 12, YELLOW 12]
    Draw 2: BLACK 2 RED 8 */
        [TestMethod]
        public void PlayCardReturnWildCardTest2()
        {
            Deck d = new Deck();
            Player p = new Player();
            List<Card> pcards = new List<Card>
            {
                new Card(9, Card.Color.GREEN, Card.ActionType.NONE),
                new Card(8, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(3, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(3, Card.Color.RED, Card.ActionType.NONE),
                new Card(12, Card.Color.YELLOW, Card.ActionType.DRAW_2),
                new Card(12, Card.Color.YELLOW, Card.ActionType.DRAW_2),
             };

            p.Cards = pcards;

            g.FlippedCard = new Card(12, Card.Color.GREEN, Card.ActionType.DRAW_2);
            Card c = p.TryPlayCard(g.FlippedCard);

            Assert.AreEqual(12, c.Number);
            Assert.AreEqual(Card.ActionType.DRAW_2, c.Action);
        }

        /**Played WILD
Color is YELLOW
Player_7 [YELLOW 2, BLACK 3, BLACK 1, BLACK 9, WILD, BLACK 6, BLACK 4]
*Played WILD*/
        [TestMethod]
        public void WildPlayedColorYellowNxtPlayerHasAYellowMustPlayitTest()
        {
            Deck d = new Deck();
            Player p = new Player();
            List<Card> pcards = new List<Card>
            {
                new Card(2, Card.Color.YELLOW, Card.ActionType.NONE),
                new Card(3, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(1, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(9, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(13, Card.Color.NONE, Card.ActionType.WILD),
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(4, Card.Color.BLACK, Card.ActionType.NONE)
            };

            p.Cards = pcards;

            g.FlippedCard = new Card(13, Card.Color.YELLOW, Card.ActionType.WILD);
            Card c = p.TryPlayCard(g.FlippedCard);

            Assert.AreEqual(Card.ActionType.WILD, c.Action);
            Assert.AreEqual(13, c.Number);
        }

        /**Played BLACK 2
Player_3 [WILD, BLACK 3, BLACK 1, RED 5, GREEN DRAW_2, GREEN 9, GREEN 4]
    *Played WILD
Color is GREEN*/
        [TestMethod]
        public void FlippedCardGreen_PlayWildOrGreen_GreenTest()
        {
            Deck d = new Deck();
            Player p = new Player();
            List<Card> pcards = new List<Card>
            {
                new Card(13, Card.Color.NONE, Card.ActionType.WILD),
                new Card(3, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(1, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(5, Card.Color.RED, Card.ActionType.NONE),
                new Card(12, Card.Color.GREEN, Card.ActionType.DRAW_2),
                new Card(9, Card.Color.GREEN, Card.ActionType.NONE),
                new Card(4, Card.Color.GREEN, Card.ActionType.NONE)
            };

            p.Cards = pcards;

            g.FlippedCard = new Card(2, Card.Color.GREEN, Card.ActionType.NONE);
            Card c = p.TryPlayCard(g.FlippedCard);

            Assert.AreEqual(Card.Color.GREEN, c.GetCardColor());
            Assert.AreEqual(Card.ActionType.DRAW_2, c.Action);
        }

        /**Played BLACK 2
Player_3 [WILD, BLACK 3, BLACK 1, RED 5, GREEN DRAW_2, GREEN 9, GREEN 4]
    *Played WILD
Color is RED*/
        [TestMethod]
        public void FlippedCardGreen_PlayWildOrGreen_WildTest()
        {
            Deck d = new Deck();
            Player p = new Player();
            List<Card> pcards = new List<Card>
            {
                new Card(13, Card.Color.NONE, Card.ActionType.WILD),
                new Card(3, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(1, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(5, Card.Color.RED, Card.ActionType.NONE),
                new Card(12, Card.Color.GREEN, Card.ActionType.DRAW_2),
                new Card(9, Card.Color.GREEN, Card.ActionType.NONE),
                new Card(4, Card.Color.GREEN, Card.ActionType.NONE)
            };

            p.Cards = pcards;

            g.FlippedCard = new Card(2, Card.Color.RED, Card.ActionType.NONE);
            Card c = p.TryPlayCard(g.FlippedCard);

            Assert.AreEqual(13, c.Number);
            Assert.AreEqual(Card.ActionType.WILD, c.Action);
        }

        /*Color is YELLOW
Player_3 [BLACK 7, BLACK 6, BLACK 5, BLACK 2, RED 6, RED 2]
Picked up  WILD_DRAW_4
Player_4 [BLACK 3, RED 8, GREEN 2, GREEN 1, YELLOW 1]
draw a wild4
play a wild4
        */
        [TestMethod]
        public void PickupWild4PlayItTest()
        {
            Deck d = new Deck();
            Player p = g.Players[0];
            List<Card> pcards = new List<Card>
            {
                new Card(7, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(5, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(2, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.RED, Card.ActionType.NONE),
                new Card(2, Card.Color.RED, Card.ActionType.NONE),
            };

            foreach(Card c in p.Cards)
            {
                c.Dealt = false;
            }

            p.Cards = pcards;
            foreach (Card card in p.Cards)
            {
                foreach (Card shuffleCard in g.ShuffledDeck.Cards)
                {
                    if(card.GetCardColor() == shuffleCard.GetCardColor() &&
                        card.Number == shuffleCard.Number)
                    {
                        shuffleCard.Dealt = true;
                    }
                }
            }

            g.FlippedCard = new Card(1, Card.Color.YELLOW, Card.ActionType.NONE);
            g.FlippedCard.Dealt = true;

            Card topCard = new Card(14, Card.Color.NONE, Card.ActionType.WILD_DRAW_4);
            g.ShuffledDeck.Cards.Insert(0, topCard);

            Player cp = g.PlaysCard(p);

            Assert.AreEqual("Player_3", cp.Name);
            Assert.AreEqual(14, g.FlippedCard.Number);
            Assert.AreEqual(Card.ActionType.WILD_DRAW_4, g.FlippedCard.Action);
            Assert.AreEqual("Player_2", g.Players[1].Name);
            Assert.IsTrue(g.Players[1].Cards.Count >= 4);
        }
        [TestMethod]
        public void PickupWildPlayItTest()
        {
            Deck d = new Deck();
            Player p = g.Players[0];
            List<Card> pcards = new List<Card>
            {
                new Card(7, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(5, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(2, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.RED, Card.ActionType.NONE),
                new Card(2, Card.Color.RED, Card.ActionType.NONE),
            };

            foreach (Card c in p.Cards)
            {
                c.Dealt = false;
            }

            p.Cards = pcards;
            foreach (Card card in p.Cards)
            {
                foreach (Card shuffleCard in g.ShuffledDeck.Cards)
                {
                    if (card.GetCardColor() == shuffleCard.GetCardColor() &&
                        card.Number == shuffleCard.Number)
                    {
                        shuffleCard.Dealt = true;
                    }
                }
            }

            g.FlippedCard = new Card(1, Card.Color.YELLOW, Card.ActionType.NONE);
            g.FlippedCard.Dealt = true;

            Card topCard = new Card(13, Card.Color.NONE, Card.ActionType.WILD);
            g.ShuffledDeck.Cards.Insert(0, topCard);

            Player cp = g.PlaysCard(p);

            Assert.AreEqual("Player_2", cp.Name);
            Assert.AreEqual(13, g.FlippedCard.Number);
            Assert.IsTrue(Card.Color.NONE != g.FlippedCard.GetCardColor());
            Assert.AreEqual(Card.ActionType.WILD, g.FlippedCard.Action);
        }

        [TestMethod]
        public void PickupDraw2PlayItTest()
        {
            Deck d = new Deck();
            Player p = g.Players[0];
            List<Card> pcards = new List<Card>
            {
                new Card(7, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(5, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(2, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.RED, Card.ActionType.NONE),
                new Card(2, Card.Color.RED, Card.ActionType.NONE),
            };

            foreach (Card c in p.Cards)
            {
                c.Dealt = false;
            }

            p.Cards = pcards;
            foreach (Card card in p.Cards)
            {
                foreach (Card shuffleCard in g.ShuffledDeck.Cards)
                {
                    if (card.GetCardColor() == shuffleCard.GetCardColor() &&
                        card.Number == shuffleCard.Number)
                    {
                        shuffleCard.Dealt = true;
                    }
                }
            }

            g.FlippedCard = new Card(1, Card.Color.YELLOW, Card.ActionType.NONE);
            g.FlippedCard.Dealt = true;

            Card topCard = new Card(12, Card.Color.YELLOW, Card.ActionType.DRAW_2);
            g.ShuffledDeck.Cards.Insert(0, topCard);

            Player cp = g.PlaysCard(p);

            Assert.AreEqual("Player_3", cp.Name);
            Assert.AreEqual("Player_2", g.Players[1].Name);
            Assert.IsTrue(g.Players[1].Cards.Count >= 2);
            Assert.AreEqual(12, g.FlippedCard.Number);
            Assert.AreEqual(Card.ActionType.DRAW_2, g.FlippedCard.Action);
        }

        [TestMethod]
        public void PickupReversePlayItTest()
        {
            Deck d = new Deck();
            Player p = g.Players[0];
            List<Card> pcards = new List<Card>
            {
                new Card(7, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(5, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(2, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.RED, Card.ActionType.NONE),
                new Card(2, Card.Color.RED, Card.ActionType.NONE),
            };

            foreach (Card c in p.Cards)
            {
                c.Dealt = false;
            }

            p.Cards = pcards;
            foreach (Card card in p.Cards)
            {
                foreach (Card shuffleCard in g.ShuffledDeck.Cards)
                {
                    if (card.GetCardColor() == shuffleCard.GetCardColor() &&
                        card.Number == shuffleCard.Number)
                    {
                        shuffleCard.Dealt = true;
                    }
                }
            }

            g.FlippedCard = new Card(1, Card.Color.YELLOW, Card.ActionType.NONE);
            g.FlippedCard.Dealt = true;

            Card topCard = new Card(10, Card.Color.YELLOW, Card.ActionType.REVERSE);
            g.ShuffledDeck.Cards.Insert(0, topCard);

            Player cp = g.PlaysCard(p);

            Assert.AreEqual("Player_7", cp.Name);
            Assert.AreEqual(10, g.FlippedCard.Number);
            Assert.AreEqual(Card.ActionType.REVERSE, g.FlippedCard.Action);
        }

        [TestMethod]
        public void PickupSkipPlayItTest()
        {
            Deck d = new Deck();
            Player p = g.Players[0];
            List<Card> pcards = new List<Card>
            {
                new Card(7, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(5, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(2, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.RED, Card.ActionType.NONE),
                new Card(2, Card.Color.RED, Card.ActionType.NONE),
            };

            foreach (Card c in p.Cards)
            {
                c.Dealt = false;
            }

            p.Cards = pcards;
            foreach (Card card in p.Cards)
            {
                foreach (Card shuffleCard in g.ShuffledDeck.Cards)
                {
                    if (card.GetCardColor() == shuffleCard.GetCardColor() &&
                        card.Number == shuffleCard.Number)
                    {
                        shuffleCard.Dealt = true;
                    }
                }
            }

            g.FlippedCard = new Card(1, Card.Color.YELLOW, Card.ActionType.NONE);
            g.FlippedCard.Dealt = true;

            Card topCard = new Card(11, Card.Color.YELLOW, Card.ActionType.SKIP);
            g.ShuffledDeck.Cards.Insert(0, topCard);

            Player cp = g.PlaysCard(p);

            Assert.AreEqual("Player_3", cp.Name);
            Assert.AreEqual(11, g.FlippedCard.Number);
            Assert.AreEqual(Card.ActionType.SKIP, g.FlippedCard.Action);
        }

        [TestMethod]
        public void PickupNonActionCardPlayItTest()
        {
            Deck d = new Deck();
            Player p = g.Players[0];
            List<Card> pcards = new List<Card>
            {
                new Card(7, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(5, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(2, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.RED, Card.ActionType.NONE),
                new Card(2, Card.Color.RED, Card.ActionType.NONE),
            };

            foreach (Card c in p.Cards)
            {
                c.Dealt = false;
            }

            p.Cards = pcards;
            foreach (Card card in p.Cards)
            {
                foreach (Card shuffleCard in g.ShuffledDeck.Cards)
                {
                    if (card.GetCardColor() == shuffleCard.GetCardColor() &&
                        card.Number == shuffleCard.Number)
                    {
                        shuffleCard.Dealt = true;
                    }
                }
            }

            g.FlippedCard = new Card(1, Card.Color.YELLOW, Card.ActionType.NONE);
            g.FlippedCard.Dealt = true;

            Card topCard = new Card(7, Card.Color.YELLOW, Card.ActionType.NONE);
            g.ShuffledDeck.Cards.Insert(0, topCard);

            Player cp = g.PlaysCard(p);

            Assert.AreEqual("Player_2", cp.Name);
            Assert.AreEqual(7, g.FlippedCard.Number);
            Assert.AreEqual(Card.ActionType.NONE, g.FlippedCard.Action);
        }

        [TestMethod]
        public void PlayCardTest()
        {
            Deck d = new Deck();
            Player p = g.Players[0];
            List<Card> pcards = new List<Card>
            {
                new Card(7, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(5, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(2, Card.Color.YELLOW, Card.ActionType.NONE),
                new Card(6, Card.Color.RED, Card.ActionType.NONE),
                new Card(2, Card.Color.RED, Card.ActionType.NONE),
            };

            foreach (Card c in p.Cards)
            {
                c.Dealt = false;
            }

            p.Cards = pcards;
            foreach (Card card in p.Cards)
            {
                foreach (Card shuffleCard in g.ShuffledDeck.Cards)
                {
                    if (card.GetCardColor() == shuffleCard.GetCardColor() &&
                        card.Number == shuffleCard.Number)
                    {
                        shuffleCard.Dealt = true;
                    }
                }
            }

            g.FlippedCard = new Card(1, Card.Color.YELLOW, Card.ActionType.NONE);
            g.FlippedCard.Dealt = true;

            Card topCard = new Card(7, Card.Color.YELLOW, Card.ActionType.NONE);
            g.ShuffledDeck.Cards.Insert(0, topCard);

            Player cp = g.PlaysCard(p);

            Assert.AreEqual("Player_2", cp.Name);
            Assert.AreEqual(2, g.FlippedCard.Number);
            Assert.AreEqual(Card.ActionType.NONE, g.FlippedCard.Action);
        }

        [TestMethod]
        public void PlayCardReverseTest()
        {
            Deck d = new Deck();
            Player p = g.Players[0];
            List<Card> pcards = new List<Card>
            {
                new Card(7, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(5, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(10, Card.Color.YELLOW, Card.ActionType.REVERSE),
                new Card(6, Card.Color.RED, Card.ActionType.NONE),
                new Card(2, Card.Color.RED, Card.ActionType.NONE),
            };

            foreach (Card c in p.Cards)
            {
                c.Dealt = false;
            }

            p.Cards = pcards;
            foreach (Card card in p.Cards)
            {
                foreach (Card shuffleCard in g.ShuffledDeck.Cards)
                {
                    if (card.GetCardColor() == shuffleCard.GetCardColor() &&
                        card.Number == shuffleCard.Number)
                    {
                        shuffleCard.Dealt = true;
                    }
                }
            }

            g.FlippedCard = new Card(1, Card.Color.YELLOW, Card.ActionType.NONE);
            g.FlippedCard.Dealt = true;

            //Card topCard = new Card(7, Card.Color.YELLOW, Card.ActionType.NONE);
            //g.ShuffledDeck.Cards.Insert(0, topCard);

            Player cp = g.PlaysCard(p);

            Assert.AreEqual("Player_7", cp.Name);
            Assert.AreEqual(10, g.FlippedCard.Number);
            Assert.AreEqual(Card.Color.YELLOW, g.FlippedCard.GetCardColor());
            Assert.AreEqual(Card.ActionType.REVERSE, g.FlippedCard.Action);
        }

        [TestMethod]
        public void PlayCardWild4Test()
        {
            Deck d = new Deck();
            Player p = g.Players[0];
            List<Card> pcards = new List<Card>
            {
                new Card(7, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(5, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(14, Card.Color.NONE, Card.ActionType.WILD_DRAW_4),
                new Card(6, Card.Color.RED, Card.ActionType.NONE),
                new Card(2, Card.Color.RED, Card.ActionType.NONE),
            };

            foreach (Card c in p.Cards)
            {
                c.Dealt = false;
            }

            p.Cards = pcards;
            foreach (Card card in p.Cards)
            {
                foreach (Card shuffleCard in g.ShuffledDeck.Cards)
                {
                    if (card.GetCardColor() == shuffleCard.GetCardColor() &&
                        card.Number == shuffleCard.Number)
                    {
                        shuffleCard.Dealt = true;
                    }
                }
            }

            g.FlippedCard = new Card(1, Card.Color.YELLOW, Card.ActionType.NONE);
            g.FlippedCard.Dealt = true;

            //Card topCard = new Card(7, Card.Color.YELLOW, Card.ActionType.NONE);
            //g.ShuffledDeck.Cards.Insert(0, topCard);

            Player cp = g.PlaysCard(p);

            Assert.AreEqual("Player_3", cp.Name);
            Assert.AreEqual(14, g.FlippedCard.Number);
            Assert.AreEqual(Card.Color.BLACK, g.FlippedCard.GetCardColor());
            Assert.AreEqual(Card.ActionType.WILD_DRAW_4, g.FlippedCard.Action);
            Assert.IsTrue(g.Players[1].Cards.Count >= 4);
        }

        [TestMethod]
        public void PlayCardWildTest()
        {
            Deck d = new Deck();
            Player p = g.Players[0];
            List<Card> pcards = new List<Card>
            {
                new Card(7, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(5, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(13, Card.Color.NONE, Card.ActionType.WILD),
                new Card(6, Card.Color.RED, Card.ActionType.NONE),
                new Card(2, Card.Color.RED, Card.ActionType.NONE),
            };

            foreach (Card c in p.Cards)
            {
                c.Dealt = false;
            }

            p.Cards = pcards;
            foreach (Card card in p.Cards)
            {
                foreach (Card shuffleCard in g.ShuffledDeck.Cards)
                {
                    if (card.GetCardColor() == shuffleCard.GetCardColor() &&
                        card.Number == shuffleCard.Number)
                    {
                        shuffleCard.Dealt = true;
                    }
                }
            }

            g.FlippedCard = new Card(1, Card.Color.YELLOW, Card.ActionType.NONE);
            g.FlippedCard.Dealt = true;

            Player cp = g.PlaysCard(p);

            Assert.AreEqual("Player_2", cp.Name);
            Assert.AreEqual(13, g.FlippedCard.Number);
            Assert.AreEqual(Card.Color.BLACK, g.FlippedCard.GetCardColor());
            Assert.AreEqual(Card.ActionType.WILD, g.FlippedCard.Action);
        }

        [TestMethod]
        public void PlaysCardDrawCardCantPlay()
        {
            Deck d = new Deck();
            Player p = g.Players[0];
            List<Card> pcards = new List<Card>
            {
                new Card(7, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(5, Card.Color.BLACK, Card.ActionType.NONE),
                new Card(6, Card.Color.RED, Card.ActionType.NONE),
                new Card(2, Card.Color.RED, Card.ActionType.NONE),
            };

            foreach (Card c in p.Cards)
            {
                c.Dealt = false;
            }

            p.Cards = pcards;
            foreach (Card card in p.Cards)
            {
                foreach (Card shuffleCard in g.ShuffledDeck.Cards)
                {
                    if (card.GetCardColor() == shuffleCard.GetCardColor() &&
                        card.Number == shuffleCard.Number)
                    {
                        shuffleCard.Dealt = true;
                    }
                }
            }

            g.FlippedCard = new Card(1, Card.Color.YELLOW, Card.ActionType.NONE);
            g.FlippedCard.Dealt = true;

            Card topCard = new Card(8, Card.Color.GREEN, Card.ActionType.NONE);
            g.ShuffledDeck.Cards.Insert(0, topCard);

            Player cp = g.PlaysCard(p);

            Assert.AreEqual("Player_2", cp.Name);
            Assert.AreEqual(1, g.FlippedCard.Number);
            Assert.AreEqual(Card.Color.YELLOW, g.FlippedCard.GetCardColor());
        }

        /**
Played BLACK 4
Player_1 [RED 0, GREEN 1, GREEN 0, YELLOW 8, YELLOW 5]
Picked up  WILD
Color is YELLOW
*Played WILD
Color is YELLOW*/
        [TestMethod]
        public void PlayCardWildColorYellowTest()
        {
            Deck d = new Deck();
            Player p = g.Players[0];
            List<Card> pcards = new List<Card>
            {
                new Card(0, Card.Color.RED, Card.ActionType.NONE),
                new Card(1, Card.Color.GREEN, Card.ActionType.NONE),
                new Card(0, Card.Color.GREEN, Card.ActionType.NONE),
                new Card(8, Card.Color.YELLOW, Card.ActionType.NONE),
                new Card(5, Card.Color.YELLOW, Card.ActionType.NONE),
            };

            foreach (Card c in p.Cards)
            {
                c.Dealt = false;
            }

            p.Cards = pcards;
            foreach (Card card in p.Cards)
            {
                foreach (Card shuffleCard in g.ShuffledDeck.Cards)
                {
                    if (card.GetCardColor() == shuffleCard.GetCardColor() &&
                        card.Number == shuffleCard.Number)
                    {
                        shuffleCard.Dealt = true;
                    }
                }
            }

            g.FlippedCard = new Card(4, Card.Color.BLACK, Card.ActionType.NONE);
            g.FlippedCard.Dealt = true;

            Card topCard = new Card(13, Card.Color.NONE, Card.ActionType.WILD);
            g.ShuffledDeck.Cards.Insert(0, topCard);

            Player cp = g.PlaysCard(p);

            Assert.AreEqual("Player_2", cp.Name);
            Assert.AreEqual(13, g.FlippedCard.Number);
            Assert.AreEqual(Card.Color.YELLOW, g.FlippedCard.GetCardColor());
        }

        [TestMethod]
        public void ResetCardsTest()
        {
            foreach (Card c in g.ShuffledDeck.Cards)
            {
                c.Dealt = true;
            }

            // force card reset
            List<Card> c1 = g.GetCards(1);

            var count = 0;
            foreach (Card c in g.ShuffledDeck.Cards)
            {
                if (c.Dealt == true)
                {
                    count++;
                }
            }

            Assert.AreEqual(1, count);
        }
    }
}