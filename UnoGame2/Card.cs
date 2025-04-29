using System;
using UnoGame2;

namespace UnoGame
{
    public class Card : IPrint
    {
        public enum Color 
        {
            NONE, BLACK, RED, GREEN, YELLOW
        }

        public enum ActionType
        {
            NONE, DRAW_2, REVERSE, SKIP, WILD, WILD_DRAW_4
        }

        private int number;
        private ActionType action;
        private bool dealt;
        private Color color;

        public Card()
        {
        }

        public Card(int num, Color color)
        {
            Number = num;
            this.color = color;
            Action = ActionType.NONE;
            Dealt = false;
            
        }
        public Card(int num, Color color=Color.NONE, ActionType special = ActionType.NONE)
        {
            Number = num;
            this.color = color;
            Action = special;
            Dealt = false;
        }
        public int Number { get => number; set => number = value; }
        public ActionType Action { get => action; set => action = value; }
        public bool Dealt { get => dealt; set => dealt = value; }

        public Color GetCardColor()
        {
            return this.color;
        }
        public void SetCardColor(Card.Color color)
        {
            this.color = color;
            Console.WriteLine("Color is " + color);
        }

        public bool IsColorMatch(Card.Color cardColor)
        {
            return (GetCardColor() == cardColor);
        }
        public bool IsNumberMatch(int cardNumber)
        {
            return (Number == cardNumber);
        }
        public bool IsMatchOrWild(Card flippedCard)
        {
            return (IsColorMatch(flippedCard.GetCardColor()) ||
                   IsNumberMatch(flippedCard.Number) ||
                   Action == Card.ActionType.WILD ||
                   Action == Card.ActionType.WILD_DRAW_4);
        }
        public string PrintCard(bool print = false, string player = "")
        {
            var cardText = "";
            if (player != "")
            {
                cardText = string.Format("{0} ", player);

            }
            // its a number
            if (Action == ActionType.NONE)
            {
                cardText += string.Format("{0} {1}", GetCardColor(), Number);
            }
            // its a special card
            else
            {
                var spec = Action.ToString();
                if (Action == ActionType.WILD || Action == ActionType.WILD_DRAW_4)
                {
                    cardText += string.Format("{0}", spec);
                }
                else
                {
                    cardText += string.Format("{0} {1}" , GetCardColor(), spec);
                }
            }

            if (print)
            {
                Console.WriteLine(cardText);
            }

            return cardText;
        }
    }
}
