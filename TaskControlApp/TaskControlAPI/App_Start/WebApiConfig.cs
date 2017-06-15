using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace TaskControlAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


			config.Routes.MapHttpRoute(
						name: "AllProject",
						routeTemplate: "projects/all",
						defaults: new { controller = "Project", action = "GetAllProjects" }
						);

			config.Routes.MapHttpRoute(
						name: "AllUsers",
						routeTemplate: "users/all",
						defaults: new { controller = "User", action = "GetAllUsers" }
						);

			config.Routes.MapHttpRoute(
						name: "NewUser",
						routeTemplate: "users/create",
						defaults: new { controller = "User", action = "GetAllUsers" }
						);

			config.Routes.MapHttpRoute(
						name: "AddUser",
						routeTemplate: "account/create",
						defaults: new { controller = "Account", action = "AddUser" }
						);

			config.Routes.MapHttpRoute(
						name: "UpdateUser",
						routeTemplate: "users/update",
						defaults: new { controller = "User", action = "UpdateUser" }
						);

      config.Routes.MapHttpRoute(
            name: "AllRoles",
            routeTemplate: "roles/all",
            defaults: new { controller = "User", action = "GetAllRoles" }
            );
    }
    }
}
