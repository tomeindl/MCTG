using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class MonsterCard : AbstractCard
    {
        public Races Race { get; private set; }

        public MonsterCard(int id, string name, Elements element, int dmg, int value, Rarity rarity, Races race) : base(id, name, element, dmg, value, rarity)
        {
            this.Race = race;
        }

        public override string ToString()
        {
            return this.Id + " : " + this.Name + " Spell " + this.Race + " " + this.Rarity;
        }
    }
}
