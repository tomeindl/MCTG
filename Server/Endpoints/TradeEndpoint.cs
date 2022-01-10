using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Server.BusinessLogic;
using Server.DAL;
using Server.HTTP;
using Server.Models;
using Server.Utility;

namespace Server.Endpoints
{
    [EndpointPath("/tradings")]
    class TradeEndpoint
    {
        [EndpointMethod("GET")]
        public Response getDeals(Request request, Response response)
        {
            try
            {
                if (request.Headers.ContainsKey("Authorization"))
                {
                    string token = request.Headers["Authorization"];
                    UserAccess.isLoggedIn(token);

                    response.Content = JsonSerializer.Serialize(TradeLogic.getTrades());                    
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

        [EndpointMethod("POST")]
        public Response addOffer(Request request, Response response)
        {
            try
            {
                if (request.Headers.ContainsKey("Authorization"))
                {
                    string token = request.Headers["Authorization"];                    
                    int cardid = JsonSerializer.Deserialize<int>(request.Content);

                    response.Content = TradeLogic.addTrade(UserAccess.isLoggedIn(token), cardid);
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
