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

      #region project routes 
      config.Routes.MapHttpRoute(
            name: "GetAllProjects",
            routeTemplate: "projects/all",
            defaults: new { controller = "Project", action = "GetAllProjects" }
            );

      config.Routes.MapHttpRoute(
            name: "GetProjectsByOwner",
            routeTemplate: "projects/getByOwnerId",
            defaults: new { controller = "Project", action = "GetProjectsByOwner" }
            );

      config.Routes.MapHttpRoute(
            name: "GetProjectStatistics",
            routeTemplate: "projects/getstatistics",
            defaults: new { controller = "Project", action = "GetProjectStatistics" }
            );

      config.Routes.MapHttpRoute(
          name: "GetProjectById",
          routeTemplate: "projects/get",
          defaults: new { controller = "Project", action = "GetProjectById" }
      );

      config.Routes.MapHttpRoute(
         name: "GetProjectByName",
         routeTemplate: "projects/getbyname",
         defaults: new { controller = "Project", action = "GetProjectByName" }
     );

      config.Routes.MapHttpRoute(
          name: "CreateProject",
          routeTemplate: "projects/create",
          defaults: new { controller = "Project", action = "CreateProject" }
      );


      config.Routes.MapHttpRoute(
          name: "UpdateProject",
          routeTemplate: "projects/update",
          defaults: new { controller = "Project", action = "UpdateProject" }
      );

      config.Routes.MapHttpRoute(
        name: "GetProjectNotes",
        routeTemplate: "projects/getprojectnotes",
        defaults: new { controller = "Project", action = "GetProjectNotes" }
        );

      config.Routes.MapHttpRoute(
       name: "AddNewNote",
       routeTemplate: "projects/newnote",
       defaults: new { controller = "Project", action = "AddNewNote" }
       );

      #endregion

      #region users routes

      config.Routes.MapHttpRoute(
            name: "AllUsers",
            routeTemplate: "users/all",
            defaults: new { controller = "User", action = "GetAllUsers" }
            );

      config.Routes.MapHttpRoute(
            name: "GetUserByUsername",
            routeTemplate: "users/get",
            defaults: new { controller = "User", action = "GetUserByUsername" }
            );

      config.Routes.MapHttpRoute(
            name: "GetUserById",
            routeTemplate: "users/getbyid",
            defaults: new { controller = "User", action = "GetUserById" }
            );

      config.Routes.MapHttpRoute(
            name: "SearchUsers",
            routeTemplate: "users/search",
            defaults: new { controller = "User", action = "SearchUsers" }
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
            name: "GetUserLanguage",
            routeTemplate: "users/getuserlanguage",
            defaults: new { controller = "User", action = "GetUserLanguage" }
            );

      config.Routes.MapHttpRoute(
            name: "SetUserLanguage",
            routeTemplate: "users/setuserlanguage",
            defaults: new { controller = "User", action = "SetUserLanguage" }
            );
      #endregion

      #region roles routes

      config.Routes.MapHttpRoute(
            name: "AllRoles",
            routeTemplate: "roles/all",
            defaults: new { controller = "User", action = "GetAllRoles" }
            );

      config.Routes.MapHttpRoute(
            name: "AddUserToRole",
            routeTemplate: "roles/adduser",
            defaults: new { controller = "User", action = "AddUserToRole" }
            );

      config.Routes.MapHttpRoute(
            name: "AddNewRole",
            routeTemplate: "roles/new",
            defaults: new { controller = "User", action = "AddNewRole" }
            );
      #endregion

      #region utility routes
      config.Routes.MapHttpRoute(
            name: "GetIssueTypes",
            routeTemplate: "issueTypes/all",
            defaults: new { controller = "Utility", action = "GetIssueTypes" }
            );

      config.Routes.MapHttpRoute(
            name: "GetAllStatuses",
            routeTemplate: "status/all",
            defaults: new { controller = "Utility", action = "GetAllStatuses" }
            );

      config.Routes.MapHttpRoute(
            name: "GetPriorities",
            routeTemplate: "priority/all",
            defaults: new { controller = "Utility", action = "GetPriorities" }
            );

      #endregion

      #region task routes
      config.Routes.MapHttpRoute(
            name: "GetTasksOnProject",
            routeTemplate: "tasks/gettasksonproject",
            defaults: new { controller = "Task", action = "GetTasksOnProject" }
            );

      config.Routes.MapHttpRoute(
           name: "GetAllTasks",
           routeTemplate: "tasks/all",
           defaults: new { controller = "Task", action = "GetAllTasks" }
           );

      config.Routes.MapHttpRoute(
            name: "CreateTask",
            routeTemplate: "tasks/create",
            defaults: new { controller = "Task", action = "CreateTask" }
            );

      config.Routes.MapHttpRoute(
            name: "UpdateTask",
            routeTemplate: "tasks/update",
            defaults: new { controller = "Task", action = "UpdateTask" }
            );



      config.Routes.MapHttpRoute(
            name: "GetAllTasksDetails",
            routeTemplate: "tasks/details",
            defaults: new { controller = "Task", action = "GetAllTasksDetails" }
            );

      config.Routes.MapHttpRoute(
            name: "GetTaskById",
            routeTemplate: "tasks/getbyid",
            defaults: new { controller = "Task", action = "GetTaskById" }
            );


      config.Routes.MapHttpRoute(
            name: "GetTaskByIdCustom",
            routeTemplate: "tasks/getbyidcustom",
            defaults: new { controller = "Task", action = "GetTaskByIdCustom" }
            );

      config.Routes.MapHttpRoute(
            name: "GetTasksForUser",
            routeTemplate: "tasks/gettasksforuser",
            defaults: new { controller = "Task", action = "GetTasksForUser" }
            );


      config.Routes.MapHttpRoute(
            name: "UpdateTaskStatus",
            routeTemplate: "tasks/updatetaskstatus",
            defaults: new { controller = "Task", action = "UpdateTaskStatus" }
            );

      config.Routes.MapHttpRoute(
           name: "GetTaskHistory",
           routeTemplate: "tasks/gettaskhistory",
           defaults: new { controller = "Task", action = "GetTaskHistory" }
           );


      #endregion

    }
  }
}
