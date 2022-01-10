using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Utility;

namespace Server.Models
{
    public class InstigatorTrap : AbstractCard, ITrap
    {
        public InstigatorTrap(int id, string name, Elements element, int dmg, int value, Rarity rarity) : base(id, name, element, dmg, value, rarity)
        {
        }

        public List<AbstractCard> ManipulateDeck(List<AbstractCard> deck)
        {
            if (deck.Count == 0) throw new InvalidInputException("Deck empty at Instigator");
            List<AbstractCard> newdeck = deck;
            int lowestval = 99;
            int lowestindex = 0;
            for(int i = 0;i < deck.Count;i++)
            {
                if((lowestval = deck.ElementAt(i).Damage) < lowestval)
                {
                    lowestindex = i;                    
                }
            }
            newdeck.RemoveAt(lowestindex);
            return newdeck;            
        }

        public override string ToString()
        {
            return this.Id + " : " + this.Name + " Spell " + this.Rarity;
        }

    }

    
}
