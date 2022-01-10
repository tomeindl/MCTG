using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server.Utility;
using Server.DAL;
using System.Text.Json;

namespace Server.BusinessLogic
{
    public class PackageFactory
    {
        public static string BuyPackage(string type, int user)
        {
            switch (type)
            {
                case "bronze":
                    return buyBronzePack(user);
                case "silver":
                    return buySilverPack(user);
                case "gold":
                    return buyGoldPack(user);
                default:
                    throw new InvalidInputException("Pack type doesnt exist");  
            }
        }

        private static string buyBronzePack(int user)
        {
            int usergold = UserAccess.getUserGold(user);
            if (usergold - 5 < 0) throw new InvalidInputException("Not enough Gold");

            UserAccess.changeUserGold(user, usergold - 5);            

            List<int> CommonCards = CardAccess.getFactoryCards(0);
            List<int> RareCards = CardAccess.getFactoryCards(1);            

            Random rnd = new Random();
            List<int> cards = new List<int>();

            int r;

            for (int i = 0; i<4; i++)
            {
                r = rnd.Next(0, CommonCards.Count());
                cards.Add(CommonCards[r]);
                CardAccess.GiveCardToUser(user, CommonCards[r]);
                CommonCards.RemoveAt(r);
            }
            r = rnd.Next(0, RareCards.Count());
            cards.Add(RareCards[r]);
            CardAccess.GiveCardToUser(user, RareCards[r]);
            RareCards.RemoveAt(r);

            return JsonSerializer.Serialize(cards);
        }

        private static string buySilverPack(int user)
        {
            int usergold = UserAccess.getUserGold(user);
            if (usergold - 10 < 0) throw new InvalidInputException("Not enough Gold");

            UserAccess.changeUserGold(user, usergold - 10);

            List<int> CommonCards = CardAccess.getFactoryCards(0);
            List<int> RareCards = CardAccess.getFactoryCards(1);
            List<int> EpicCards = CardAccess.getFactoryCards(2);
            

            Random rnd = new Random();
            List<int> cards = new List<int>();

            int r;

            for (int i = 0; i < 2; i++)
            {
                r = rnd.Next(0, CommonCards.Count());
                cards.Add(CommonCards[r]);
                CardAccess.GiveCardToUser(user, CommonCards[r]);
                CommonCards.RemoveAt(r);
            }
            for (int i = 0; i < 2; i++)
            {
                r = rnd.Next(0, RareCards.Count());
                cards.Add(RareCards[r]);
                CardAccess.GiveCardToUser(user, RareCards[r]);
                RareCards.RemoveAt(r);
            }
            r = rnd.Next(0, EpicCards.Count());
            cards.Add(EpicCards[r]);
            CardAccess.GiveCardToUser(user, EpicCards[r]);
            EpicCards.RemoveAt(r);

            return JsonSerializer.Serialize(cards);
        }

        private static string buyGoldPack(int user)
        {
            int usergold = UserAccess.getUserGold(user);
            if (usergold - 20 < 0) throw new InvalidInputException("Not enough Gold");

            UserAccess.changeUserGold(user, usergold - 20);
            
            List<int> RareCards = CardAccess.getFactoryCards(1);
            List<int> EpicCards = CardAccess.getFactoryCards(2);
            List<int> LegendaryCards = CardAccess.getFactoryCards(3);


            Random rnd = new Random();
            List<int> cards = new List<int>();

            int r;

            for (int i = 0; i < 2; i++)
            {
                r = rnd.Next(0, EpicCards.Count());
                cards.Add(EpicCards[r]);
                CardAccess.GiveCardToUser(user, EpicCards[r]);
                EpicCards.RemoveAt(r);
            }
            for (int i = 0; i < 2; i++)
            {
                r = rnd.Next(0, RareCards.Count());
                cards.Add(RareCards[r]);
                CardAccess.GiveCardToUser(user, RareCards[r]);
                RareCards.RemoveAt(r);
            }
            r = rnd.Next(0, LegendaryCards.Count());
            cards.Add(LegendaryCards[r]);
            CardAccess.GiveCardToUser(user, LegendaryCards[r]);
            LegendaryCards.RemoveAt(r);

            return JsonSerializer.Serialize(cards);
        }
    }
}
