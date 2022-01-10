using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.DAL;
using Server.Utility;
using Server.Models;

namespace Server.BusinessLogic
{
    class TradeLogic
    {
        public static List<TradeOffer> getTrades()
        {
            List<TradeOffer> tradeoffers = new List<TradeOffer>();
            Dictionary<int, (int, int)> offers = TradeAccess.getOffers();
            foreach(KeyValuePair<int, (int, int)> offer in offers)
            {
                Console.WriteLine(offer.Key);
                User user = UserAccess.getUserById(offer.Value.Item1);
                user.Password = "hidden";
                tradeoffers.Add(new TradeOffer(offer.Key, CardAccess.getCard(offer.Value.Item2), user));

            }
            return tradeoffers;
        }

        public static string addTrade(int userid, int cardid)
        {
            if (CardAccess.ownsCard(userid, cardid))
            {
                TradeAccess.addOffer(userid, cardid);
                return "success!";
            }
            else
            {
                throw new InvalidInputException("Dont own card");
            }
        }
    }
}
