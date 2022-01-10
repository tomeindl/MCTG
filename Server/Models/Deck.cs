using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Deck
    {
        public int Id { get;}
        public string Name { get; private set; }

        public List<AbstractCard> Cards { get; private set; }

        public Deck(int id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.Cards = new List<AbstractCard>();
        }

        public bool addCard(AbstractCard card)
        {
            if(Cards.Count() < 4)
            {
                Cards.Add(card);
                return true;
            }
            return false;
        }
    }
}
