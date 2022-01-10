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
    class UserAccess
    {
        public static void AddUser(User user)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    INSERT INTO mctg.mctg_users (users_name, users_pw)
                    VALUES (@users_name, @users_pw)              
                    ";

                NpgsqlCommand c = command as NpgsqlCommand;                
               
                c.Parameters.Add("users_name", NpgsqlDbType.Varchar, 50);
                c.Parameters.Add("users_pw", NpgsqlDbType.Varchar, 50);

                c.Prepare();
                
                c.Parameters["users_name"].Value = user.Username;
                c.Parameters["users_pw"].Value = user.Password;

                command.ExecuteNonQuery();
            }
        }

        public static User getUserByName(string username)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    SELECT users_id, users_name,users_pw, users_gold, users_elo, users_bio, users_picture, users_wins, users_losses
                    FROM mctg.mctg_users
                    WHERE users_name = @Username
                    ";

                var USERNAME = command.CreateParameter();
                USERNAME.DbType = DbType.String;
                USERNAME.ParameterName = "Username";
                USERNAME.Value = username;
                command.Parameters.Add(USERNAME);

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    User user = new User();
                    user.Id = reader.GetInt32(0);
                    user.Username = reader.GetString(1);
                    user.Password = reader.GetString(2);
                    user.Gold = reader.GetInt32(3);
                    user.Elo = reader.GetInt32(4);
                    user.Bio = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    user.Picture = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    user.Wins = reader.GetInt32(7);
                    user.Losses = reader.GetInt32(8);

                    reader.Close();
                    return user;
                }
                else
                {
                    throw new InvalidInputException("no such user");
                }
                
                
            }            
        }

        public static string AddSession(int id)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    INSERT INTO mctg.mctg_sessions (sessions_user, sessions_token, sessions_time)
                    VALUES (@user, @token, @time)              
                    ";

                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("user", NpgsqlDbType.Integer);
                c.Parameters.Add("token", NpgsqlDbType.Varchar, 100);
                c.Parameters.Add("time", NpgsqlDbType.Timestamp, 50);

                c.Prepare();

                string token = new string(Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Take(100).ToArray());

                c.Parameters["user"].Value = id;
                c.Parameters["token"].Value = token;
                c.Parameters["time"].Value = DateTime.Now;

                command.ExecuteNonQuery();
                return token;
            }            
        }

        public static int isLoggedIn(string token)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    SELECT sessions_user FROM mctg.mctg_sessions WHERE sessions_token = @token
                    ";

                var USERNAME = command.CreateParameter();
                USERNAME.DbType = DbType.String;
                USERNAME.ParameterName = "token";
                USERNAME.Value = token;
                command.Parameters.Add(USERNAME);

                var reader = command.ExecuteReader();
                int result;

                if (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
                else
                {
                    throw new NotLoggedInException();
                }               

                reader.Close();
               
                return result;
            }
        }

        public static int getUserGold(int userid)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                int gold = 0; 
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    SELECT users_gold
                    FROM mctg.mctg_users
                    WHERE users_id = @userid
                    ";

                var USERID = command.CreateParameter();
                USERID.DbType = DbType.Int32;
                USERID.ParameterName = "userid";
                USERID.Value = userid;
                command.Parameters.Add(USERID);

                var reader = command.ExecuteReader();
                reader.Read();

                gold = reader.GetInt32(0);
                
                reader.Close();
                return gold;
            }

        }

        public static void changeUserGold(int userid, int gold)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    UPDATE mctg.mctg_users 
                    SET users_gold = @gold
                    WHERE users_id = @userid       
                    ";

                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("gold", NpgsqlDbType.Integer);
                c.Parameters.Add("userid", NpgsqlDbType.Integer);

                c.Prepare();

                
                c.Parameters["gold"].Value = gold;
                c.Parameters["userid"].Value = userid;

                command.ExecuteNonQuery();
            }

        }

        public static void changeUserElo(int userid, int elo)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    UPDATE mctg.mctg_users 
                    SET users_elo = @elo
                    WHERE users_id = @userid       
                    ";

                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("elo", NpgsqlDbType.Integer);
                c.Parameters.Add("userid", NpgsqlDbType.Integer);

                c.Prepare();


                c.Parameters["elo"].Value = elo;
                c.Parameters["userid"].Value = userid;

                command.ExecuteNonQuery();
            }

        }

        public static User getUserById(int userid)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    SELECT users_id, users_name,users_pw, users_gold, users_elo, users_bio, users_picture, users_wins, users_losses
                    FROM mctg.mctg_users
                    WHERE users_id = @userid
                    ";

                var USERNAME = command.CreateParameter();
                USERNAME.DbType = DbType.Int32;
                USERNAME.ParameterName = "userid";
                USERNAME.Value = userid;
                command.Parameters.Add(USERNAME);

                var reader = command.ExecuteReader();
                reader.Read();

                User user = new User();
                user.Id = reader.GetInt32(0);
                user.Username = reader.GetString(1);
                user.Password = reader.GetString(2);
                user.Gold = reader.GetInt32(3);
                user.Elo = reader.GetInt32(4);
                user.Bio = reader.IsDBNull(5) ? "" : reader.GetString(5);
                user.Picture = reader.IsDBNull(6) ? "" : reader.GetString(6);
                user.Wins = reader.GetInt32(7);
                user.Losses = reader.GetInt32(8);

                reader.Close();
                return user;
            }

        }

        public static void updateUser(User user)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    UPDATE mctg.mctg_users 
                    SET users_name = @name, users_bio = @bio, users_picture = @pic
                    WHERE users_id = @userid
                    ";

                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("name", NpgsqlDbType.Varchar, 50);
                c.Parameters.Add("bio", NpgsqlDbType.Text);
                c.Parameters.Add("pic", NpgsqlDbType.Varchar, 50);
                c.Parameters.Add("userid", NpgsqlDbType.Integer);

                c.Prepare();

                c.Parameters["name"].Value = user.Username;
                c.Parameters["bio"].Value = user.Bio;
                c.Parameters["pic"].Value = user.Picture;
                c.Parameters["userid"].Value = user.Id;

                command.ExecuteNonQuery();
            }
        }

        public static Dictionary<string, int> getScoreboard()
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    SELECT users_name, users_elo
                    FROM mctg.mctg_users
                    ORDER BY users_elo DESC
                    ";

                var reader = command.ExecuteReader();
                Dictionary<string, int> list = new Dictionary<string, int>();

                while (reader.Read())
                {
                    list.Add(reader.GetString(0), reader.GetInt32(1));
                }
                reader.Close();
                return list;
            }
        }

        public static void addLoss(int userid)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    UPDATE mctg.mctg_users 
                    SET users_losses = users_losses + 1
                    WHERE users_id = @userid
                    ";

                NpgsqlCommand c = command as NpgsqlCommand;
                
                c.Parameters.Add("userid", NpgsqlDbType.Integer);

                c.Prepare();
                
                c.Parameters["userid"].Value = userid;

                command.ExecuteNonQuery();
            }

        }

        public static void addWin(int userid)
        {
            using (IDbConnection connection = Connection.GetConnection())
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();

                command.CommandText = @"
                    UPDATE mctg.mctg_users 
                    SET users_wins = users_wins + 1
                    WHERE users_id = @userid
                    ";

                NpgsqlCommand c = command as NpgsqlCommand;

                c.Parameters.Add("userid", NpgsqlDbType.Integer);

                c.Prepare();

                c.Parameters["userid"].Value = userid;

                command.ExecuteNonQuery();
            }

        }


    }
}
