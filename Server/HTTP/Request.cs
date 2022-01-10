using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace Server.HTTP
{
    public class Request
    {
        private TcpClient _socket;

        public string Method { get; private set; }
        public string Path { get; private set; }        
        public string Version { get; private set; }
        public string Content { get; private set; }
        
        public Dictionary<string, string> GetParam { get; private set; }
        public Dictionary<string, string> Headers { get; }

        public Request(TcpClient socket)
        {
            _socket = socket;
            Headers = new Dictionary<string, string>();
            GetParam = new Dictionary<string, string>();
            Method = null;            
        }


        private static string Readline(Stream stream)
        {
            int next_char;
            string data = "";
            while (true)
            {
                next_char = stream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }

        public void ProcessHttp()
        {
            Stream s = _socket.GetStream();            

            //read the full HTTP - request
            string line = null;

            while ((line = Readline(s)) != null)
            {
                Console.WriteLine(line);
                if (line.Length == 0) break;

                // handle first line of HTTP
                if (Method == null)
                {
                    var parts = line.Split(' ');
                    Method = parts[0];
                    Path = parts[1];
                    Version = parts[2];
                }
                // handle HTTP headers
                else
                {
                    var parts = line.Split(": ");
                    Headers.Add(parts[0], parts[1]);
                }

            }
            if (Headers.ContainsKey("Content-Length"))
            {
                int totalBytes = Convert.ToInt32(Headers["Content-Length"]);
                int bytesLeft = totalBytes;
                byte[] bytes = new byte[totalBytes];
                
                while (bytesLeft > 0)
                {
                    byte[] buffer = new byte[bytesLeft > 1024 ? 1024 : bytesLeft];
                    int n = s.Read(buffer, 0, buffer.Length);
                    buffer.CopyTo(bytes, totalBytes - bytesLeft);

                    bytesLeft -= n;
                }

                Content = Encoding.ASCII.GetString(bytes);

                Console.WriteLine(Content);
            }
            else
            {
                Content = null;
            }

            //extract parameters from path
            if (Path.Contains("?"))
            {
                var temppatharray = Path.Split("?");
                Path = temppatharray[0];

                var getarray = temppatharray[1].Split("&");
                foreach(string getpair in getarray)
                {
                    var getparts = getpair.Split("=");
                    GetParam.Add(getparts[0], getparts[1]);
                }                
            }

            Console.WriteLine(Path);
            foreach(KeyValuePair<string, string> entry in GetParam)
            {
                Console.WriteLine(entry.Key + "=" + entry.Value);
            }
        } 

        public string GetEndpoint()
        { 
            if(Path.Count(x => x == '/') > 1) return Path.Substring(0, Path.LastIndexOf("/"));
            return Path;
        }

        public string GetEndpointVar()
        {
            if (Path.Contains("/"))
            {
                return Path.Substring(Path.LastIndexOf("/") + 1);
            }
            return null;            
        }

    }
}
