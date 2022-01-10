using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Server.BusinessLogic;
using Server.HTTP;
using Server.Models;

namespace Server.Endpoints
{
    [EndpointPath("/sessions")]
    class SessionsEndpoint
    {
        [EndpointMethod("POST")]
        public Response LoginUser(Request request, Response response)
        {
            Console.WriteLine("Users POST request content: " + request.Content);

            try
            {
                User user = JsonSerializer.Deserialize<User>(request.Content);

                if (user.Username == "" || user.Username == null || user.Password == null)
                {
                    response.Status = HttpStatusCode.BadRequest;
                    response.Content = "Invalid JSON";
                    return response;
                }

                string token = UserAdministration.LoginUser(user);

                if (token != null)
                {
                    response.Content = token;
                    response.Status = HttpStatusCode.OK;
                }
                else
                {
                    response.Status = HttpStatusCode.Unauthorized;
                    response.Content = "Username or password wrong";
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
            return response;
        }
    }
}
