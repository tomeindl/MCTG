using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Server.HTTP
{
    class Response
    {
        private TcpClient _socket;

        public string Version { get; private set; }
        public HttpStatusCode Status { get; set; }
        public string Content { get; set; }

        public Response(TcpClient socket)
        {
            _socket = socket;
        }


        public void SendResponse()
        {
            var writer = new StreamWriter(_socket.GetStream()) { AutoFlush = true };

            writer.WriteLine("HTTP/1.1 " + (int)Status + " " + Status);
            writer.WriteLine("Server: MCTG Server");
            writer.WriteLine($"Current Time: {DateTime.Now}");

            if(Content != null)
            {
                writer.WriteLine($"Content-Length: {Content.Length}");
                writer.WriteLine("Content-Type: text/html; charset=utf-8");

                writer.WriteLine("");
                writer.WriteLine(Content);
            }           

            writer.WriteLine();
            writer.Flush();
            writer.Close();
        }

        



    }
}
