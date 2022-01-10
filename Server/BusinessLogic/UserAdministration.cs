using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Models;
using Server.DAL;

namespace Server.BusinessLogic
{
    class UserAdministration
    {
        public static void RegisterUser(User user)
        {
            UserAccess.AddUser(user);
            User currUser = UserAccess.getUserByName(user.Username);
            DeckAccess.addDeck(currUser.Id);
        }

        public static string LoginUser(User user)
        {
            User refuser = UserAccess.getUserByName(user.Username);
            if (refuser.Password == user.Password)
            {                
                return UserAccess.AddSession(refuser.Id); 
            }

            return null;
        }
    }
}
