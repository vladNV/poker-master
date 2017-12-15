using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemClient
{
    public class CardImage
    {
        public const string PATH = "Bee Premium Red/";
        public const string PNG = ".png";
        public const string HEART = "Hearts";
        public const string DIAMOND = "Diamonds";
        public const string CLUB = "Clubs";
        public const string SPADE = "Spades";
        public const string BACK = "_Back";
        //type&suit
        // A,  K,  Q,  J, 10, 9, 8, 7, 6, 5, 4, 3, 2;
        //12, 11, 10,  9,  8, 7, 6, 5, 4, 3, 2, 1, 0;

        // heart - 0, tile (diamond) - 1, clover (club) - 2, pike (spade) - 3

        private static string getSuit(int suit)
        {
            switch (suit)
            {
                case 0:
                    return HEART;
                case 1:
                    return DIAMOND;
                case 2:
                    return CLUB;
                case 3:
                    return SPADE;
            }
            throw new Exception("Such suit doesn't exist");
        }

        public static string getBack()
        {
            return PATH + BACK + PNG;
        }

        public static string getResource(string cardType)
        {
            string[] args = cardType.Split('&');
            int type = int.Parse(args[0]);
            int suit = int.Parse(args[1]);
            if (type == 12)
            {
                return PATH + getSuit(suit) + " " + 1 + PNG;
            } else
            {
                return PATH + getSuit(suit) + " " + (type + 2) + PNG;
            }
        }
    }
}
