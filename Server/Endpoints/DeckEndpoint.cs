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
    [EndpointPath("/deck")]
    class DeckEndpoint
    {
        [EndpointMethod("GET")]
        public Response getDeck(Request request, Response response)
        {
            try
            {
                if (request.Headers.ContainsKey("Authorization"))
                {
                    string token = request.Headers["Authorization"];                    
                    if(request.GetParam.ContainsKey("format") && request.GetParam["format"] == "plain")
                    {
                        response.Content = DeckAdministration.getDeckPlain(UserAccess.isLoggedIn(token));
                    }
                    else
                    {
                        response.Content = JsonSerializer.Serialize(DeckAdministration.getDeck(UserAccess.isLoggedIn(token)));
                    }
                    
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
        public Response ChangeDeck(Request request, Response response)
        {
            try
            {
                Console.WriteLine(request.Content);
                List<int> list = JsonSerializer.Deserialize<List<int>>(request.Content);

                if (list.Count < 4) throw new InvalidInputException("need 4 cards!");

                if (request.Headers.ContainsKey("Authorization"))
                {
                    string token = request.Headers["Authorization"];
                    DeckAdministration.changeDeck(UserAccess.isLoggedIn(token), list);
                    response.Content = "changed deck!";
                    response.Status = HttpStatusCode.OK;
                }
                else
                {
                    response.Status = HttpStatusCode.BadRequest;
                    response.Content = "No token sent";
                }

            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("JSON was NULL at POST /users");
                response.Status = HttpStatusCode.BadRequest;
                response.Content = "JSON was NULL";
            }
            catch (JsonException)
            {
                Console.WriteLine("JSON exception at POST /deck");
                response.Status = HttpStatusCode.BadRequest;
                response.Content = "Invalid JSON";
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
