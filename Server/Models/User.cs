using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string Picture { get; set; }
        public string Password { get; set; }
        public int Elo { get; set; }
        public int Gold { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public User()
        {            
        }

        public string getProfile()
        {
            List<Object> data = new List<object>();
            data.Add(Username);
            data.Add(Elo);            
            data.Add(Bio);
            data.Add(Picture);
            return JsonSerializer.Serialize(data);
        }

        public string getStats()
        {
            List<Object> data = new List<object>();
            data.Add(Username);
            data.Add(Elo);
            data.Add(Wins);
            data.Add(Losses);
            return JsonSerializer.Serialize(data);
        }
    }
}
