using BusinessServices;
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

		// GET: users/all
		[HttpGet]
		[ActionName("GetAllUsers")]
		public HttpResponseMessage GetAllUsers()
		{
			_log.DebugFormat("GetAllProjects invoked...");
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
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not create user");
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

  }
}
