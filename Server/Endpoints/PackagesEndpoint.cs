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
    [EndpointPath("/packages")]
    class PackagesEndpoint
    {

        [EndpointMethod("POST")]
        public Response buyPackage(Request request, Response response)
        {
            try
            {
                string packlvl;
                if ((packlvl = request.GetEndpointVar()) == null)
                {
                    response.Status = HttpStatusCode.BadRequest;
                    response.Content = "Invalid Pack";
                    return response;
                }
                if (request.Headers.ContainsKey("Authorization"))
                {
                    string token = request.Headers["Authorization"];
                    string result = PackageFactory.BuyPackage(packlvl, UserAccess.isLoggedIn(token));
                    response.Content = result;
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
                Console.WriteLine("JSON exception at POST /users");
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
