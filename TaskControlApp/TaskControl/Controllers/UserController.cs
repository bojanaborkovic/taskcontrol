﻿using DataModel.UnitOfWork;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using TaskControl.Models;
using TaskControl.ServiceClients;
using TaskControlDTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PagedList;
using BussinesService.Interfaces.Responses.User;

namespace TaskControl.Controllers
{
  [Authorize]
  public class UserController : Controller
  {
    private UnitOfWork unitOfWork = new UnitOfWork();
    private UserServiceClient userServiceClient = new UserServiceClient("users") { DoSerialize = true };
    private ApplicationUserManager _userManager;

    public UserController()
    {

    }

    public UserController(ApplicationUserManager userManager)
    {
      UserManager = userManager;
    }

    public ApplicationUserManager UserManager
    {
      get
      {
        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
      }
      private set
      {
        _userManager = value;
      }
    }

    // GET: User
    public ActionResult Index()
    {
      var responseData = userServiceClient.GetAllUsers();

      //var users = JsonConvert.DeserializeObject<List<UserEntity>>(responseData);
      var mappedUsers = MapToUsersViewModel(responseData.Users);


      return View("Index", mappedUsers.ToPagedList(1, 5));
    }

    [HttpGet]
    public ActionResult Search(string sortOrder, string currentFilter, string searchString, int pageNumber = 1, int pageSize = 5)
    {
      var usersRet = userServiceClient.SearchUsers();
      ViewBag.CurrentFilter = searchString;
      pageNumber = pageNumber > 0 ? pageNumber : 1;
      pageSize = pageSize > 0 ? pageSize : 25;

      ViewBag.UsernameSortParam = sortOrder == "username" ? "username_desc" : "username";
      ViewBag.FirstNameSortParam = sortOrder == "firstname" ? "firstname_desc" : "firstname";
      ViewBag.IdSortParam = sortOrder == "Id" ? "Id_desc" : "Id";

      ViewBag.CurrentSort = sortOrder;

      List<UserEntity> sortedUsers = new List<UserEntity>();

      if (!string.IsNullOrEmpty(searchString))
      {
        usersRet.Users = usersRet.Users.Where(x => x.UserName.Contains(searchString) || x.Email.Contains(searchString)).ToList();
      }

      if (searchString != null)
      {
        pageNumber = 1;
      }
      else
      {
        searchString = currentFilter;
      }


      switch (sortOrder)
      {
        case "username_desc":
          sortedUsers = usersRet.Users.OrderByDescending(x => x.UserName).ToList();
          break;
        case "firstname_desc":
          sortedUsers = usersRet.Users.OrderByDescending(x => x.FirstName).ToList();
          break;
        case "firstname":
          sortedUsers = usersRet.Users.OrderBy(x => x.FirstName).ToList();
          break;
        case "Id_desc":
          sortedUsers = usersRet.Users.OrderByDescending(x => x.Id).ToList();
          break;
        case "Id":
          sortedUsers = usersRet.Users.OrderBy(x => x.Id).ToList();
          break;
        default:
          sortedUsers = usersRet.Users.OrderBy(x => x.UserName).ToList();
          break;

      }

      var mappedUsers = MapToUsersViewModel(sortedUsers);

      return View("Index", mappedUsers.ToPagedList(pageNumber, pageSize));
    }


    [HttpGet]
    public ActionResult Details(long userId)
    {
      var userRet = userServiceClient.GetUserById(userId);
      if (userRet != null)
      {
        UserViewModel userModel = MapUserModel(userRet);
        return View("Details", userModel);
      }
      else
      {
        return RedirectToAction("Index");
      }
    }

    public ActionResult CreateUser()
    {
      return View("CreateUser");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> CreateUser(CreateUserViewModel user)
    {

      if (!ModelState.IsValid)
      { // re-render the view when validation failed.
        return View("CreateUser", user);
      }
      else
      {
        var newUser = new ApplicationUser()
        {
          Email = user.Email,
          PhoneNumber = user.PhoneNumber,
          UserName = user.Email,
          FirstName = user.FirstName,
          LastName = user.LastName

        };
        var appuser = await UserManager.CreateAsync(newUser, user.Password);

        if (appuser.Succeeded)
        {
          //user created successfully , redirect to index
          return RedirectToAction("Index");
        }
        else
        {
          return View("Error", new ErrorModel() { Message = string.Join(",", appuser.Errors) });
        }

      }


    }


    [HttpGet]
    public ActionResult EditUser(long userId)
    {
      var responseData = userServiceClient.GetUserById(userId);
      if (responseData != null && string.IsNullOrEmpty(responseData.ErrorMessage))
      {
        UserViewModel user = new UserViewModel();
        user.FirstName = responseData.FirstName;
        user.LastName = responseData.LastName;
        user.RoleName = responseData.RoleName;
        user.PhoneNumber = responseData.PhoneNumber;
        user.UserName = responseData.UserName;
        user.Email = responseData.Email;

        return View(user);
      }
      else
      {
        return View("Error", new ErrorModel() { Message = responseData != null ? responseData.ErrorMessage : "Error during fetching user!" });
      }

    }

    [HttpPost]
    public ActionResult EditUser(UserViewModel userViewModel)
    {
      var ret = userServiceClient.UpdateUser(MapUserViewModel(userViewModel));
      if (ret != null && !string.IsNullOrEmpty(ret.ErrorMessage))
      {
        return View("Error", ret);
      }
      else
      {
        return RedirectToAction("Index");
      }

    }

    #region mappers
    private UserEntity MapUserEnity(CreateUserViewModel user)
    {
      UserEntity entity = new UserEntity();
      entity.Email = user.Email;
      entity.FirstName = user.FirstName;
      entity.LastName = user.LastName;
      entity.PhoneNumber = user.PhoneNumber;
      entity.UserName = user.UserName;
      entity.Password = user.Password;
      entity.Id = user.UserId;
      return entity;
    }

    private UserEntity MapUserViewModel(UserViewModel user)
    {
      UserEntity entity = new UserEntity();
      entity.Email = user.Email;
      entity.FirstName = user.FirstName;
      entity.LastName = user.LastName;
      entity.PhoneNumber = user.PhoneNumber;
      entity.UserName = user.UserName;
      entity.Id = user.UserId;
      return entity;
    }

    private UserViewModel MapUserModel(BaseUserReturn userEntity)
    {
      UserViewModel userModel = new UserViewModel();
      userModel.Email = userEntity.Email;
      userModel.FirstName = userEntity.FirstName;
      userModel.LastName = userEntity.LastName;
      userModel.PhoneNumber = userEntity.PhoneNumber;
      userModel.UserName = userEntity.UserName;
      userModel.UserId = userEntity.Id;
      userModel.RoleName = userEntity.RoleName;
      return userModel;
    }

    private List<UserViewModel> MapToUsersViewModel(List<UserEntity> users)
    {
      List<UserViewModel> viewMOdel = new List<UserViewModel>();
      foreach (var user in users)
      {
        viewMOdel.Add(new UserViewModel()
        {
          UserId = user.Id,
          UserName = user.UserName,
          FirstName = user.FirstName,
          LastName = user.LastName,
          Email = user.Email,
          RoleName = user.RoleName
        });
      }

      return viewMOdel;
    }

    #endregion
  }
}