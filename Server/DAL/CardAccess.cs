using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using Server.Models;
using Server.Utility;

namespace Server.DAL
{
    class CardAccess
    {

        public static void AddCards(List<AbstractCard> cards)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    INSERT INTO mctg.mctg_cards 
                    (cards_name, cards_type, cards_element, cards_race, cards_dmg, cards_trap, cards_rarity, cards_value, cards_location)
                    VALUES (@cards_name, @cards_type, @cards_element, @cards_race, @cards_dmg, @cards_trap, @cards_rarity, @cards_value, @cards_location)              
                    ";

                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("cards_name", NpgsqlDbType.Varchar, 50);
                c.Parameters.Add("cards_type", NpgsqlDbType.Varchar, 50);
                c.Parameters.Add("cards_element", NpgsqlDbType.Integer, 50);
                c.Parameters.Add("cards_race", NpgsqlDbType.Integer, 50);
                c.Parameters.Add("cards_dmg", NpgsqlDbType.Integer);
                c.Parameters.Add("cards_trap", NpgsqlDbType.Varchar, 50);
                c.Parameters.Add("cards_rarity", NpgsqlDbType.Integer, 50);
                c.Parameters.Add("cards_value", NpgsqlDbType.Integer, 50);
                c.Parameters.Add("cards_location", NpgsqlDbType.Varchar, 50);
               

                c.Prepare();

                foreach(AbstractCard card in cards)
                {
                    c.Parameters["cards_name"].Value = card.Name;
                    c.Parameters["cards_type"].Value = "";
                    c.Parameters["cards_trap"].Value = "";

                    if (card.GetType() == typeof(MonsterCard))
                    {
                        c.Parameters["cards_type"].Value = "Monster";
                        c.Parameters["cards_race"].Value = (int)((MonsterCard)card).Race;
                        c.Parameters["cards_element"].Value = (int)card.Element;
                    }
                    else if(card.GetType() == typeof(SpellCard))
                    {
                        c.Parameters["cards_type"].Value = "Spell";
                        c.Parameters["cards_element"].Value = (int)card.Element;
                    }
                    else
                    {
                        c.Parameters["cards_type"].Value = "Trap";
                        if(card.GetType() == typeof(InstigatorTrap)) c.Parameters["cards_trap"].Value = "Instigator";
                        else if(card.GetType() == typeof(ThiefTrap)) c.Parameters["cards_trap"].Value = "Thief";
                    }                   
                                       
                    c.Parameters["cards_dmg"].Value = card.Damage;
                    
                    c.Parameters["cards_rarity"].Value = (int)card.Rarity;
                    c.Parameters["cards_value"].Value = card.Value;
                    c.Parameters["cards_location"].Value = "Factory";

                    command.ExecuteNonQuery();
                }

                
            }
        }

        public static List<int> getFactoryCards(int rarity)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {                
                connection.Open();
                IDbCommand command = connection.CreateCommand();

                List<int> list = new List<int>();

                command.CommandText = @"
                    SELECT cards_id
                    FROM mctg.mctg_cards
                    WHERE cards_location = 'Factory' AND cards_rarity = @rarity
                    ";

                var RARITY = command.CreateParameter();
                RARITY.DbType = DbType.Int32;
                RARITY.ParameterName = "rarity";
                RARITY.Value = rarity;
                command.Parameters.Add(RARITY);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(reader.GetInt32(0));
                }
                
                reader.Close();
                return list;
            }   
        }

        public static AbstractCard getCard(int id)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();

                List<int> list = new List<int>();

                command.CommandText = @"
                    SELECT cards_id, cards_name, cards_type, cards_element, cards_race, cards_dmg, cards_trap, cards_rarity, cards_value
                    FROM mctg.mctg_cards
                    WHERE cards_id = @id
                    ";

                var RARITY = command.CreateParameter();
                RARITY.DbType = DbType.Int32;
                RARITY.ParameterName = "id";
                RARITY.Value = id;
                command.Parameters.Add(RARITY);

                var reader = command.ExecuteReader();                

                if (reader.Read())
                {
                    if(reader.GetString(2) == "Spell")
                    {
                        AbstractCard card = new SpellCard(reader.GetInt32(0), reader.GetString(1), (Elements)reader.GetInt32(3), reader.GetInt32(5), reader.GetInt32(8), (Rarity)reader.GetInt32(7));
                        reader.Close();
                        return card;
                    }
                    else if(reader.GetString(2) == "Monster")
                    {
                        AbstractCard card = new MonsterCard(reader.GetInt32(0), reader.GetString(1), (Elements)reader.GetInt32(3), reader.GetInt32(5), reader.GetInt32(8), (Rarity)reader.GetInt32(7), (Races)reader.GetInt32(4));
                        reader.Close();
                        return card;
                    }
                    else if(reader.GetString(2) == "Trap")
                    {
                        if (reader.GetString(6) == "Thief")
                        {
                            AbstractCard card = 
                                new ThiefTrap(reader.GetInt32(0), reader.GetString(1), (Elements)reader.GetInt32(3), reader.GetInt32(5), reader.GetInt32(8), (Rarity)reader.GetInt32(7));
                            reader.Close();
                            return card;
                        }
                            
                        if (reader.GetString(6) == "Instigator")
                        {
                            AbstractCard card = 
                                new InstigatorTrap(reader.GetInt32(0), reader.GetString(1), (Elements)reader.GetInt32(3), reader.GetInt32(5), reader.GetInt32(8), (Rarity)reader.GetInt32(7));
                            reader.Close();
                            return card;
                        }                            
                    }
                //close using
                }
                return null;
            }

        }

        public static void GiveCardToUser(int userid, int cardid)
        {
            Console.WriteLine("Give User card: " + cardid);

            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    UPDATE mctg.mctg_cards
                    SET cards_location = 'User'
                    WHERE cards_id = @cardid
                    ";

                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("cardid", NpgsqlDbType.Integer); c.Prepare();
                c.Parameters.Add("userid", NpgsqlDbType.Integer); c.Prepare();

                c.Prepare();

                c.Parameters["cardid"].Value = cardid;
                c.Parameters["userid"].Value = userid;

                command.ExecuteNonQuery();
            }
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    INSERT INTO mctg.mctg_usercards (usercards_user, usercards_card)
                    VALUES (@userid, @cardid)                    
                ";

                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("cardid", NpgsqlDbType.Integer); 
                c.Parameters.Add("userid", NpgsqlDbType.Integer); 

                c.Prepare();

                c.Parameters["cardid"].Value = cardid;
                c.Parameters["userid"].Value = userid;

                command.ExecuteNonQuery();
            }
        }

        public static List<int> getUserCards(int userid)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();

                List<int> list = new List<int>();

                command.CommandText = @"
                    SELECT usercards_card
                    FROM mctg.mctg_usercards
                    WHERE usercards_user = @userid
                    ";

                var RARITY = command.CreateParameter();
                RARITY.DbType = DbType.Int32;
                RARITY.ParameterName = "userid";
                RARITY.Value = userid;
                command.Parameters.Add(RARITY);

                var reader = command.ExecuteReader();

                while(reader.Read())
                {
                    list.Add(reader.GetInt32(0));
                }
                return list;
            }

        }

        public static bool ownsCard(int userid, int cardid)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    SELECT usercards_card FROM mctg.mctg_usercards WHERE usercards_card = @cardid AND usercards_user = @userid
                    ";

                var USER = command.CreateParameter();
                USER.DbType = DbType.Int32;
                USER.ParameterName = "userid";
                USER.Value = userid;
                command.Parameters.Add(USER);

                var CARD = command.CreateParameter();
                CARD.DbType = DbType.Int32;
                CARD.ParameterName = "cardid";
                CARD.Value = cardid;
                command.Parameters.Add(CARD);

                var reader = command.ExecuteReader();               

                if (reader.Read())
                {
                    reader.Close();
                    return true;
                }
                else
                {
                    reader.Close();
                    throw new InvalidInputException("Dont own all cards");
                }
            }
        }

        public static int countFreeCards()
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();

                List<int> list = new List<int>();

                command.CommandText = @"
                    SELECT
                    COUNT(*)
                    FROM mctg.mctg_cards
                    WHERE cards_location = 'Factory'
                    ";

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return reader.GetInt32(0);
                }
                return 0;
                
            }

        }
    }
}
