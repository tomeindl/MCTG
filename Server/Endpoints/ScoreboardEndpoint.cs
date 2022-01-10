using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Server.DAL;
using Server.HTTP;
using Server.Models;
using Server.Utility;

namespace Server.Endpoints
{
    [EndpointPath("/score")]
    class ScoreboardEndpoint
    {
        [EndpointMethod("GET")]
        public Response getScore(Request request, Response response)
        {
            try
            {
                if (request.Headers.ContainsKey("Authorization"))
                {                    
                    string token = request.Headers["Authorization"];
                    UserAccess.isLoggedIn(token);

                    response.Content = JsonSerializer.Serialize(UserAccess.getScoreboard());
                    response.Status = HttpStatusCode.OK;

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

                if (ex.SqlState == "23505")
                {
                    response.Content = "User already logged in";
                    response.Status = HttpStatusCode.OK;
                }
                else
                {
                    response.Content = "Postgres Exception";
                    response.Status = HttpStatusCode.BadRequest;
                }
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
