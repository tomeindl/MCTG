using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.DAL;
using Server.HTTP;
using Server.Models;
using Server.Utility;

namespace Server.Endpoints
{
    [EndpointPath("/stats")]
    class StatsEndpoint
    {
        [EndpointMethod("GET")]
        public Response getStats(Request request, Response response)
        {
            try
            {                
                if (request.Headers.ContainsKey("Authorization"))
                {
                    string s = "";
                    string token = request.Headers["Authorization"];

                    User user = UserAccess.getUserById(UserAccess.isLoggedIn(token));                    
                    response.Content = user.getStats();
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
