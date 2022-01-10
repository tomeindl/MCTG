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
    class DeckAccess
    {
        public static Deck getDeck(int userid)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();

                List<int> list = new List<int>();

                command.CommandText = @"
                    SELECT decks_id, decks_name
                    FROM mctg.mctg_decks
                    WHERE decks_user = @userid
                    ";

                var USER = command.CreateParameter();
                USER.DbType = DbType.Int32;
                USER.ParameterName = "userid";
                USER.Value = userid;
                command.Parameters.Add(USER);

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Deck(reader.GetInt32(0), reader.GetString(1));
                }
                return null;
            }
        }

        public static void addDeck(int userid)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    INSERT INTO mctg.mctg_decks (decks_user, decks_name)
                    VALUES (@userid, @name)              
                    ";

                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("userid", NpgsqlDbType.Integer);
                c.Parameters.Add("name", NpgsqlDbType.Varchar, 50);

                c.Prepare();

                c.Parameters["userid"].Value = userid;
                c.Parameters["name"].Value = "Deck-" + userid;

                command.ExecuteNonQuery();
            }
        }        

        public static List<int> getCardsInDeck(int deckid)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                List<int> list = new List<int>();
                connection.Open();
                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    SELECT usercards_card
                    FROM mctg.mctg_usercards
                    WHERE usercards_deck = @deckid
                    ";

                var DECK = command.CreateParameter();
                DECK.DbType = DbType.Int32;
                DECK.ParameterName = "deckid";
                DECK.Value = deckid;
                command.Parameters.Add(DECK);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(reader.GetInt32(0));
                }
                return list;
            }

        }

        public static void removeCardsFromDeck(int userid)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    UPDATE mctg.mctg_usercards 
                    SET usercards_deck = NULL
                    WHERE usercards_user = @userid       
                    ";

                NpgsqlCommand c = command as NpgsqlCommand;
              
                c.Parameters.Add("userid", NpgsqlDbType.Integer);

                c.Prepare();
                
                c.Parameters["userid"].Value = userid;

                command.ExecuteNonQuery();
            }
        }

        public static void addCardToDeck(int deckid, int cardid, int userid)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    UPDATE mctg.mctg_usercards 
                    SET usercards_deck = @deckid
                    WHERE usercards_card = @cardid AND usercards_user = @userid   
                    ";

                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("deckid", NpgsqlDbType.Integer);
                c.Parameters.Add("cardid", NpgsqlDbType.Integer);
                c.Parameters.Add("userid", NpgsqlDbType.Integer);

                c.Prepare();

                c.Parameters["deckid"].Value = deckid;
                c.Parameters["cardid"].Value = cardid;
                c.Parameters["userid"].Value = userid;

                command.ExecuteNonQuery();
            }
        }
    }
}
