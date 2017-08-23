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
      var users = _userService.SearchUsers();
			if (users != null)
			{
				var userEntities = users as List<UserEntity> ?? users.ToList();
				if (userEntities.Any())
				{
					_log.DebugFormat("SearchUsers finished with : {0}", userEntities.ToString());
					return Request.CreateResponse(HttpStatusCode.OK, userEntities);
				}

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
        var userEntities = users as List<UserEntity> ?? users.ToList();
        if (userEntities.Any())
        {
          _log.DebugFormat("GetAllUsers finished with : {0}", userEntities.ToString());
          return Request.CreateResponse(HttpStatusCode.OK, userEntities);
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
			long userId = _userService.CreateUser(user);
			if (userId == 0)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not create user");
			}
			else
			{
				return Request.CreateResponse(HttpStatusCode.OK, userId);
			}

		}

		[HttpPost]
		[ActionName("UpdateUser")]
		public HttpResponseMessage UpdateUser(UserEntity user)
		{
			_log.DebugFormat("CrateUser invoked with : {0}", user.ToString());
			bool created =_userService.UpdateUser(user);
			if (!created)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not update user");
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
      if (roles != null)
      {
        var rolesEntities = roles as List<RoleEntity> ?? roles.ToList();
        if (rolesEntities.Any())
        {
          _log.DebugFormat("GetAllRoles finished with : {0}", rolesEntities.ToString());
          return Request.CreateResponse(HttpStatusCode.OK, rolesEntities);
        }

      }
      return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Users not found");
    }

    [HttpPost]
    [ActionName("AddUserToRole")]
    public HttpResponseMessage AddUserToRole(long roleId, long userId)
    {
      _log.DebugFormat("AddUserToRole invoked...");
      HttpResponseMessage response = new HttpResponseMessage();
      try
      {
        var ret =_userService.AddUserToRole(roleId, userId);
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
      catch(Exception ex)
      {
        _log.ErrorFormat("Error during adding user to role : {0}", ex.Message);
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Error adding user {0} to role {1}", userId, roleId));
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
