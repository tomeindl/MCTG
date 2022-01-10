using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using Server.Models;
using Server.Utility;
using System.Text.Json;

namespace Server.HTTP
{
    class ConnectionHandler
    {
        private TcpClient _socket;
        private Request _request;
        private Response _response;

        public ConnectionHandler(TcpClient socket)
        {
            _socket = socket;
            _request = new Request(_socket);
            _response = new Response(_socket);
        }

        public void HandleConnection()
        {
            try
            {              
                _request.ProcessHttp();

                //Debug info
                Console.WriteLine("RequestPath: " + _request.GetEndpoint());
                if(_request.GetEndpointVar() != null) Console.WriteLine("RequestPathParameters: " + _request.GetEndpointVar());
                Console.WriteLine("RequestMethod: " + _request.Method);

                
                var EndpointClass = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetCustomAttribute<EndpointPathAttribute>()?.Path == _request.GetEndpoint()).Single();
                var EndpointMethod = EndpointClass.GetMethods().Where(method => method.GetCustomAttribute<EndpointMethodAttribute>()?.Method == _request.Method).Single();

                _response = (Response)EndpointMethod.Invoke(Activator.CreateInstance(EndpointClass), new object[] { _request , _response });                
                _response.SendResponse();
            }            
            catch
            {
                _response.Status = HttpStatusCode.NotFound;
                _response.SendResponse();
            }            
            
            

        }

    }
}
