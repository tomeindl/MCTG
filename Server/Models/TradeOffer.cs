using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    class TradeOffer
    {
        public int Id { get; }        
        public AbstractCard Card { get; }

        public User User { get; }

        public TradeOffer(int id, AbstractCard card, User user)
        {
            this.Id = id;
            this.Card = card;
            this.User = user;
        }
    }
}
