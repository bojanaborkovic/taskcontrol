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
using BussinesService.Interfaces.Responses.Project;

namespace TaskControl.Controllers
{
  [Authorize]
  public class RolesController : Controller
  {
    private UnitOfWork unitOfWork = new UnitOfWork();
    private UserServiceClient rolesServiceClient = new UserServiceClient("roles") { DoSerialize = true };
    private UserServiceClient usersServiceClient = new UserServiceClient("users") { DoSerialize = true };
    private ProjectServiceClient projectServiceClient = new ProjectServiceClient() { DoSerialize = true };
    private long? currentUserId;

    public RolesController()
    {
      if (currentUserId == null)
      {
        string user = System.Web.HttpContext.Current.User.Identity.Name;
        var userRet = usersServiceClient.GetUserByUsername(user);
        if (userRet != null)
        {
          currentUserId = userRet.Id;
        }
      }
    }
    // GET: Roles
    public ActionResult Index()
    {
      var response = rolesServiceClient.GetAllRoles();
      var mappedRoles = MapToRolesViewModel(response.Roles);

      return View("Index", mappedRoles.ToPagedList(1, 10));
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
      var projects = projectServiceClient.GetAllProjects(null);
      if (projects != null && projects.RecordCount > 0)
      {
        newRole.ProjectsAccess = MapProjectsToView(projects);
      }
      return PartialView("NewRole", newRole);
    }

    private List<ProjectViewModel> MapProjectsToView(GetProjectReturn projects)
    {
      List<ProjectViewModel> projectsMapped = new List<ProjectViewModel>();

      foreach (var project in projects.Projects)
      {
        projectsMapped.Add(new ProjectViewModel()
        {
          Id = project.Id,
          Name = project.Name,
          Owner = project.Owner,
          OwnerId = project.OwnerId

        });
      }
      return projectsMapped;
    }

    [HttpPost]
    public JsonResult NewRole(RoleViewModel model)
    {
      if (ModelState.IsValid)
      {
        model.DateCreated = DateTime.UtcNow;
        var ret = rolesServiceClient.AddNewRole(MapRoleModelToEntity(model));
        return Json(new { success = true });
      }
      else
      {
        return Json(new { success = false });
      }
    }

    private RoleEntity MapRoleModelToEntity(RoleViewModel model)
    {
      RoleEntity role = new RoleEntity();
      role.Name = model.RoleName;
      role.Description = model.Description;
      role.DateCreated = model.DateCreated != null ? (DateTime)model.DateCreated : DateTime.UtcNow;
      role.ProjectAccess = new List<long>();

      foreach (string s in model.ProjectList)
      {
        long val;

        if (long.TryParse(s, out val))
        {
          role.ProjectAccess.Add(val);
        }
      }

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