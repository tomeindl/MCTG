using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class SpellCard : AbstractCard
    {
        public SpellCard(int id, string name, Elements element, int dmg, int value, Rarity rarity) : base(id, name, element, dmg, value, rarity)
        {
        }

        public override string ToString()
        {
            return this.Id + " : " + this.Name + " Spell " + this.Rarity;
        }
    }
}
