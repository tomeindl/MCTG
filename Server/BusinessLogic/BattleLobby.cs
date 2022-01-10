using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Models;

namespace Server.BusinessLogic
{
    public sealed class BattleLobby
    {
        private static BattleLobby instance = null;
        private static readonly object padlock = new object();
        private static List<User> waitingroom = new List<User>();

        private BattleLobby()
        {
        }

        public static BattleLobby Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new BattleLobby();
                    }
                    return instance;
                }
            }
        }

        public void RegisterForBattle(User user)
        {
            if(waitingroom.Count() < 1)
            {
                waitingroom.Add(user);
                Console.WriteLine("One user in Room");
            }
            else
            {
                Console.WriteLine("Second user joins, battle starts.....");
                BattleLogic battle = 
                    new BattleLogic(waitingroom[0], user, DeckAdministration.getDeck(waitingroom[0].Id).Cards, DeckAdministration.getDeck(user.Id).Cards);
                battle.StartBattle();
                waitingroom.RemoveAt(0);
            }
        }

        public List<User> getWaitingroom()
        {
            List<User> l = waitingroom;
            return l;
        }
    }
}
