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
    private UserServiceClient rolesServiceClient = new UserServiceClient("roles") { DoSerialize = true };
    private UserServiceClient usersServiceClient = new UserServiceClient("users") { DoSerialize = true };
    // GET: Roles
    public ActionResult Index()
    {
      var response = rolesServiceClient.GetAllRoles();
      var mappedRoles = MapToRolesViewModel(response.Roles);

      return View("Index", mappedRoles.ToPagedList(1, 5));
    }

    [HttpPost]
    public ActionResult AddUserToRole(UserInRoleViewModel userInRole)
    {
      UserInRoleEntity userInRoleEntity = new UserInRoleEntity();
      userInRoleEntity.RoleId = userInRole.RoleId;
      userInRoleEntity.UserId = userInRole.UserId;

      var ret = rolesServiceClient.AddUserToRole(userInRoleEntity);
      if (ret == null)
      {
        return RedirectToAction("Index");
      }

      return RedirectToAction("AddUserToRole");
    }

    [HttpGet]
    public ActionResult AddUserToRole()
    {
      UserInRoleViewModel userInRole = new UserInRoleViewModel();
      var responseRoles = rolesServiceClient.GetAllRoles();

      var responseUsers = usersServiceClient.GetAllUsers();
      //var usersRet = JsonConvert.DeserializeObject<List<UserEntity>>(responseUsers);

      var roles = responseRoles.Roles.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name });
      userInRole.UserRoles = roles;
      var users = responseUsers.Users.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.UserName });
      userInRole.Users = users;
      return PartialView("AddUserToRole", userInRole);
    }

    [HttpGet]
    public ActionResult NewRole()
    {
      RoleViewModel newRole = new RoleViewModel();
      return PartialView("NewRole", newRole);
    }

    [HttpPost]
    public ActionResult NewRole(RoleViewModel model)
    {
      if (ModelState.IsValid)
      {
        model.DateCreated = DateTime.UtcNow;
        var ret = rolesServiceClient.AddNewRole(MapRoleModelToEntity(model));
        return RedirectToAction("Index");
      }
      else
      {
        return PartialView("NewRole", model);
      }
    }

    private RoleEntity MapRoleModelToEntity(RoleViewModel model)
    {
      RoleEntity role = new RoleEntity();
      role.Name = model.RoleName;
      role.Description = model.Description;
      role.DateCreated = model.DateCreated;
      return role;
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
          RoleName = role.Name,
          Description = role.Description,
          DateCreated = role.DateCreated

        });
      }

      return viewMOdel;
    }



    #endregion

  }
}