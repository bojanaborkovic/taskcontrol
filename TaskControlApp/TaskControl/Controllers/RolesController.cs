using DataModel.UnitOfWork;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskControl.Models;
using TaskControl.ServiceClients;
using TaskControlDTOs;

namespace TaskControl.Controllers
{
  [Authorize]
  public class RolesController : Controller
  {
    private UnitOfWork unitOfWork = new UnitOfWork();
    private UserServiceClient rolesServiceClient = new UserServiceClient("roles");
    private UserServiceClient usersServiceClient = new UserServiceClient("users");
    // GET: Roles
    public ActionResult Index()
    {
      var response = rolesServiceClient.GetAllRoles();
      var rolesRet = JsonConvert.DeserializeObject<List<RoleEntity>>(response);
      var mappedRoles = MapToRolesViewModel(rolesRet);


      return View("Index", mappedRoles.ToPagedList(1, 5));
    }

    [HttpPost]
    public ActionResult AddUserToRole(long userId, long roleId)
    {

      return Json(new { success = true });
    }

    [HttpGet]
    public ActionResult AddUserToRole()
    {
      UserInRoleViewModel userInRole = new UserInRoleViewModel();
      var response = rolesServiceClient.GetAllRoles();
      var rolesRet = JsonConvert.DeserializeObject<List<RoleEntity>>(response);

      var responseUsers = usersServiceClient.GetAllUsers();
      var usersRet = JsonConvert.DeserializeObject<List<UserEntity>>(responseUsers);

      var roles = rolesRet.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
      userInRole.UserRoles = roles;
      var users = usersRet.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.UserName });
      userInRole.Users = users;
      return PartialView("AddUserToRole", userInRole);
    }


    #region helpers
    private List<RoleViewModel> MapToRolesViewModel(List<RoleEntity> rolesRet)
    {
      List<RoleViewModel> viewMOdel = new List<RoleViewModel>();
      foreach (var role in rolesRet)
      {
        viewMOdel.Add(new RoleViewModel()
        {
          RoleId = role.Id,
          RoleName = role.Name
          
        });
      }

      return viewMOdel;
    }



    #endregion

  }
}