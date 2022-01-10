using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Server.Models;
using Server.BusinessLogic;
using Server.HTTP;
using Server.DAL;
using System.Net;
using Server.Utility;

namespace Server.Endpoints
{
    [EndpointPath("/users")]
    class UsersEndpoint
    {
        public UsersEndpoint(){}


        [EndpointMethod("GET")]
        public Response GetUsers(Request request, Response response)
        {
            try
            {
                if (request.GetEndpointVar() != null)
                {
                    string token = request.Headers["Authorization"];
                    UserAccess.isLoggedIn(token);
                    User user = UserAccess.getUserByName(request.GetEndpointVar());
                    response.Content = user.getProfile();
                    response.Status = HttpStatusCode.OK;
                }
                else
                {
                    response.Status = HttpStatusCode.BadRequest;
                    response.Content = "No Profile specified";
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

        [EndpointMethod("PUT")]
        public Response UpdateUserProfile(Request request, Response response)
        {
            try
            {
                if (request.GetEndpointVar() != null)
                {
                    string token = request.Headers["Authorization"];
                    int loginuserid = UserAccess.isLoggedIn(token);
                    User user = UserAccess.getUserByName(request.GetEndpointVar());
                    if (loginuserid != user.Id)
                    {
                        response.Status = HttpStatusCode.BadRequest;
                        response.Content = "Token and user dont match";
                    }
                    else
                    {
                        Dictionary<string, string> list = JsonSerializer.Deserialize<Dictionary<string, string>>(request.Content);
                        if (list.ContainsKey("Bio") && list.ContainsKey("Image") && list.ContainsKey("Name"))
                        {
                            user.Bio = list["Bio"];
                            user.Picture = list["Image"];
                            user.Username = list["Name"];
                            UserAccess.updateUser(user);

                            response.Status = HttpStatusCode.OK;
                            response.Content = "User Updated";
                        }
                        else
                        {
                            response.Status = HttpStatusCode.BadRequest;
                            response.Content = "Params in json dont match";
                        }
                    }

                }
                else
                {
                    response.Status = HttpStatusCode.BadRequest;
                    response.Content = "No Profile specified";
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

        [EndpointMethod("POST")]
        public Response PostUsers(Request request, Response response)
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

                UserAdministration.RegisterUser(user);

                response.Content = "Added User";
                response.Status = HttpStatusCode.OK;
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

                if(ex.SqlState == "23505")
                {
                    response.Content = "Username taken";
                    response.Status = HttpStatusCode.BadRequest;
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
