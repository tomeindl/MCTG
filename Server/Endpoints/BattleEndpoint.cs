using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.BusinessLogic;
using Server.DAL;
using Server.HTTP;
using Server.Models;
using Server.Utility;

namespace Server.Endpoints
{
    [EndpointPath("/battle")]
    class BattleEndpoint
    {
        [EndpointMethod("POST")]
        public Response registerForBattle(Request request, Response response)
        {
            try
            {
                if (request.Headers.ContainsKey("Authorization"))
                {
                    string token = request.Headers["Authorization"];
                    BattleLobby.Instance.RegisterForBattle(UserAccess.getUserById(UserAccess.isLoggedIn(token)));                    
                }
                else
                {
                    response.Status = HttpStatusCode.BadRequest;
                    response.Content = "No token sent";
                }

            }
            catch (Npgsql.PostgresException ex)
            {
                Console.WriteLine(ex);
                response.Content = "Postgres Exception";
                response.Status = HttpStatusCode.BadRequest;                
            }
            catch (NotLoggedInException)
            {
                response.Status = HttpStatusCode.Unauthorized;
                response.Content = "Not logged in/ invalid token";
            }
            catch (InvalidInputException e)
            {
                response.Status = HttpStatusCode.BadRequest;
                response.Content = e.Message;
            }
            return response;
        }

    }
}
