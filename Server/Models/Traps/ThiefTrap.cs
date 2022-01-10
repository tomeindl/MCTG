using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Utility;

namespace Server.Models
{
    public class ThiefTrap : AbstractCard, ITrap
    {
        public ThiefTrap(int id, string name, Elements element, int dmg, int value, Rarity rarity) : base(id, name, element, dmg, value, rarity)
        {
        }

        public List<AbstractCard> ManipulateDeck(List<AbstractCard> deck)
        {
            if (deck.Count == 0) throw new InvalidInputException("Deck empty at Thief");
            List<AbstractCard> newdeck = deck;
            int highestval = 0;
            int atindex = 0;
            for (int i = 0; i < deck.Count; i++)
            {
                if ((highestval = deck.ElementAt(i).Value) > highestval)
                {
                    atindex = i;
                }
            }
            newdeck.RemoveAt(atindex);
            return newdeck;
        }

        public override string ToString()
        {
            return this.Id + " : " + this.Name + " Spell " + this.Rarity;
        }
    }
}
