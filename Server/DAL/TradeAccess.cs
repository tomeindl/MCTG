using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using Server.Utility;

namespace Server.DAL
{
    class TradeAccess
    {
        public static Dictionary<int, (int, int)> getOffers()
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    SELECT offers_id, offers_user, offers_card
                    FROM mctg.mctg_offers                    
                    ";

                var reader = command.ExecuteReader();
                Dictionary<int, (int, int)> list = new Dictionary<int, (int, int)>();

                while (reader.Read())
                {
                    list.Add(reader.GetInt32(0), (reader.GetInt32(1), reader.GetInt32(2)));
                }
                reader.Close();
                return list;
            }
        }

        public static void addOffer(int userid, int cardid)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    INSERT INTO mctg.mctg_offers (offers_user, offers_card)
                    VALUES (@userid, @cardid)              
                    ";

                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("userid", NpgsqlDbType.Integer, 50);
                c.Parameters.Add("cardid", NpgsqlDbType.Integer, 50);

                c.Prepare();

                c.Parameters["userid"].Value = userid;
                c.Parameters["cardid"].Value = cardid;

                command.ExecuteNonQuery();
            }
        }
    }
}
