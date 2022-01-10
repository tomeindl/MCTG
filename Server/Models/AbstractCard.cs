using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public abstract class AbstractCard
    {
        public int Id { get; private set; }
        public string Name { get; }
        public Elements Element { get; private set; }
        public int Damage { get; private set; }
        public int Value { get; private set; }
        public Rarity Rarity { get; private set; }

        protected AbstractCard(int id, string name, Elements element, int dmg, int value, Rarity rarity)
        {
            this.Id = id;
            this.Name = name;
            this.Element = element;
            this.Damage = dmg;
            this.Value = value;
            this.Rarity = rarity;
        }

        abstract public override string ToString();
    }
}
