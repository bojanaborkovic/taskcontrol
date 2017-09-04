using BusinessServices;
using BusinessServices.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskControlDTOs;

namespace TaskControlAPI.Controllers
{
  public class UserController : ApiController
  {
    internal static readonly ILog _log = log4net.LogManager.GetLogger(typeof
    (ProjectController));
    private readonly IUserService _userService;

    public UserController()
    {
      _userService = new UserService();
    }

    // GET: users/search
    [HttpGet]
    [ActionName("SearchUsers")]
    public HttpResponseMessage SearchUsers()
    {
      _log.DebugFormat("SearchUsers invoked...");
      var usersRet = _userService.SearchUsers();
      if (usersRet != null && usersRet.RecordCount > 0)
      {
        _log.DebugFormat("SearchUsers finished with : {0}", usersRet.Users.ToString());
        return Request.CreateResponse(HttpStatusCode.OK, usersRet);
      }

      
      return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Users not found");
    }

    [HttpGet]
    [ActionName("GetAllUsers")]
    public HttpResponseMessage GetAllUsers()
    {
      _log.DebugFormat("GetAllUsers invoked...");
      var users = _userService.GetAllUsers();

      if (users != null)
      {
        if (users.Users.Any())
        {
          _log.DebugFormat("GetAllUsers finished with : {0}", users.ToString());
          return Request.CreateResponse(HttpStatusCode.OK, users);
        }

      }
      return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Users not found");
    }

    [HttpGet]
    [ActionName("GetUserByUsername")]
    public HttpResponseMessage GetUserByUsername([FromUri] string userName)
    {
      _log.DebugFormat("GetUserByUsername invoked for {0}...", userName);

      var user = _userService.GetUserByUsername(userName);
      if (user != null)
      {

        _log.DebugFormat("GetUserByUsername finished with : {0}", user.ToString());
        return Request.CreateResponse(HttpStatusCode.OK, user);

      }
      return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "User not found");

    }

    [HttpGet]
    [ActionName("GetUserById")]
    public HttpResponseMessage GetUserById([FromUri] long userId)
    {
      _log.DebugFormat("GetUserByUsername invoked for userId : {0}...", userId);

      var user = _userService.GetUserById(userId);
      if (user != null)
      {

        _log.DebugFormat("GetUserById finished with : {0}", user.ToString());
        return Request.CreateResponse(HttpStatusCode.OK, user);

      }
      return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "User not found");

    }

    [HttpPost]
    [ActionName("CreateUser")]
    public HttpResponseMessage CreateUser(UserEntity user)
    {
      _log.DebugFormat("CrateUser invoked with : {0}", user.ToString());
      var userRet = _userService.CreateUser(user);
      if (userRet == null)
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not create user");
      }
      else
      {
        return Request.CreateResponse(HttpStatusCode.OK, userRet.Id);
      }

    }

    [HttpPost]
    [ActionName("UpdateUser")]
    public HttpResponseMessage UpdateUser(UserEntity user)
    {
      _log.DebugFormat("CrateUser invoked with : {0}", user.ToString());
      var ret = _userService.UpdateUser(user);
      if (!string.IsNullOrEmpty(ret.ErrorMessage))
      {
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Could not update user. Error : {0}", ret.ErrorMessage));
      }
      else
      {
        return Request.CreateResponse(HttpStatusCode.OK);
      }

    }


    [HttpGet]
    [ActionName("GetAllRoles")]
    public HttpResponseMessage GetRoles()
    {
      _log.DebugFormat("GetAllRoles invoked...");
      var roles = _userService.GetAllRoles();
      if (roles != null && roles.RecordCount > 0)
      {
          _log.DebugFormat("GetAllRoles finished with : {0}", roles.StatusCode.ToString());
          return Request.CreateResponse(HttpStatusCode.OK, roles);
      }
      return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Roles not found");
    }

    [HttpPost]
    [ActionName("AddUserToRole")]
    public HttpResponseMessage AddUserToRole(UserInRoleEntity userInRole)
    {
      _log.DebugFormat("AddUserToRole invoked...");
      HttpResponseMessage response = new HttpResponseMessage();
      try
      {
        var ret = _userService.AddUserToRole(userInRole);
        if (ret == null)
        {
          _log.DebugFormat("AddUserToRole finished with");
          return Request.CreateResponse(HttpStatusCode.OK);
        }
        else
        {
          return Request.CreateResponse(HttpStatusCode.InternalServerError, ret.ErrorMessage);
        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during adding user to role : {0}", ex.Message);
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Error adding user {0} to role {1}", userInRole.UserId, userInRole.RoleId));
      }


    }

    [HttpPost]
    [ActionName("AddNewRole")]
    public HttpResponseMessage AddNewRole(RoleEntity role)
    {
      _log.DebugFormat("AddNewRole invoked...");
      HttpResponseMessage response = new HttpResponseMessage();
      try
      {
        var ret = _userService.AddNewRole(role);
        if (string.IsNullOrEmpty(ret.StatusCode) && string.IsNullOrEmpty(ret.ErrorMessage))
        {
          _log.DebugFormat("AddUserToRole finished with");
          return Request.CreateResponse(HttpStatusCode.OK);
        }
        else
        {
          return Request.CreateResponse(HttpStatusCode.InternalServerError, ret.ErrorMessage);
        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during creating new role : {0}", ex.Message);
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Error adding role with name {0}", role.Name));
      }


    }

  }
}
