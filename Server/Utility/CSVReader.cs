using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Utility
{
    class CSVReader
    {
        public static List<AbstractCard> ReadCards()
        {
            //using Stream stream = File.OpenRead("Cards.csv");
            StreamReader reader = new StreamReader("Cards.csv");

            var list = new List<AbstractCard>();

            // first line is the header (we already know and store for debugging purpose)
            // ReSharper disable once UnusedVariable
            var header = reader.ReadLine();

            bool isContentOver = false;
            while (!isContentOver)
            {
                string name = null;
                string type = null;
                int element = 0;
                int race = 0;
                int dmg = 0;
                string trap = null;
                int rarity = 0;
                int value = 0;

                for (int i = 0; i < 8; i++)
                {
                    StringBuilder readPart = new StringBuilder();
                    var isPartOver = false;
                    while (!isPartOver)
                    {
                        var character = reader.Read(); // no Async equivalent with no parameters
                        if (character == ';')
                        {
                            isPartOver = true;
                        }
                        else if (character == '\r' || character == '\n')
                        {
                            if (reader.Peek() == '\n')
                            {
                                reader.Read();
                            }

                            isPartOver = true;
                        }
                        else if (character == '\"')
                        {
                            do
                            {
                                character = reader.Read();
                                if (character == -1)
                                {
                                    isPartOver = true;
                                    isContentOver = true;
                                    break;
                                }
                                else if (character == '\"')
                                {
                                    break;
                                }
                                else
                                {
                                    readPart.Append((char)character); // because character is of type int
                                }
                            } while (character != '\"');
                        }
                        else if (character == -1)
                        {
                            isPartOver = true;
                            isContentOver = true;
                        }
                        else
                        {
                            readPart.Append((char)character);
                        }
                    }

                    if (isContentOver)
                    {
                        // last line is not taken over
                        break;
                    }

                    switch (i)
                    {
                        case 0:
                            name = readPart.ToString();
                            break;
                        case 1:
                            type = readPart.ToString();
                            break;
                        case 2:
                            element = readPart.Length == 0 ? 0 : int.Parse(readPart.ToString()); 
                            break;
                        case 3:
                            race = readPart.Length == 0 ? 0 : int.Parse(readPart.ToString());
                            break;
                        case 4:
                            dmg = readPart.Length == 0 ? 0 : int.Parse(readPart.ToString());
                            break;
                        case 5:
                            trap = readPart.ToString();
                            break;
                        case 6:
                            rarity = readPart.Length == 0 ? 0 : int.Parse(readPart.ToString());
                            break;
                        case 7:
                            value = readPart.Length == 0 ? 0 : int.Parse(readPart.ToString());
                            break;
                    }
                }

                if (!isContentOver)
                {
                    if(type == "Monster")
                    {
                        list.Add(new MonsterCard(0, name, (Elements)element, dmg, value, (Rarity)rarity, (Races)race));
                    }else if(type == "Spell")
                    {
                        list.Add(new SpellCard(0, name, (Elements)element, dmg, value, (Rarity)rarity));
                    }
                    else if(type == "Trap")
                    {
                        if(trap == "Thief")
                        {
                            list.Add(new ThiefTrap(0, name, (Elements)element, dmg, value, (Rarity)rarity));
                        }
                        if (trap == "Instigator")
                        {
                            list.Add(new InstigatorTrap(0, name, (Elements)element, dmg, value, (Rarity)rarity));
                        }
                    }                    
                }
            }
            return list;
        }
    }
}
