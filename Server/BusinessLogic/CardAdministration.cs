using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Server.DAL;
using Server.Models;

namespace Server.BusinessLogic
{
    class CardAdministration
    {
        public static string getCardsOfUser(int userid)
        {
            List<AbstractCard> cards = new List<AbstractCard>();           

            List<int> CardList = CardAccess.getUserCards(userid);

            foreach (int id in CardList)
            {
                cards.Add(CardAccess.getCard(id));
            }
            return JsonSerializer.Serialize(cards);
        }
    }
}
