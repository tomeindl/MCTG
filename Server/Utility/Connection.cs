using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace Server.Utility
{
    public class Connection
    {
        public static IDbConnection GetConnection()
        {
            return new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1;Database=mctg_db");
        }
    }
}
