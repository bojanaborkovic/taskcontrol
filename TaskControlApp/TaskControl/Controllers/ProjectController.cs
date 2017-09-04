using BussinesService.Interfaces.Responses.Project;
using DataModel.UnitOfWork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskControl.Models;
using TaskControl.ServiceClients;
using TaskControlDTOs;

namespace TaskControl.Controllers
{
  [Authorize]
  public class ProjectController : Controller
  {

    private UnitOfWork unitOfWork = new UnitOfWork();
    private ProjectServiceClient serviceClient = new ProjectServiceClient() { DoSerialize = true };
    private UserServiceClient userServiceClient = new UserServiceClient("users") { DoSerialize = true };

    public ProjectController()
    {
    }


    // GET: Project
    [HttpGet]
    public ActionResult Index()
    {
      GetProjectReturn responseData = serviceClient.GetAllProjects();

      if (responseData != null && responseData.Projects.Count > 0)
      {
        return View("Index", MapToProjectsViewModel(responseData.Projects));
      }
      else
      {
        return View("Index", new ProjectViewModel());
      }


    }

    [HttpGet]
    public ActionResult Create()
    {
      var usernames = userServiceClient.GetAllUsers();
      //var users = JsonConvert.DeserializeObject<List<UserEntity>>(usernames);
      var userNamesList = usernames.Users.Select(x => x.UserName).ToList();
      ViewBag.Usernames = JsonConvert.SerializeObject(userNamesList);

      return View("New");
    }

    [HttpPost]
    public ActionResult Create(ProjectViewModel model)
    {
      if (ModelState.IsValid)
      {
        var create = MapToProjectEntity(model);
        BaseProjectReturn ret = serviceClient.CreateProject(create);

        if (ret != null && string.IsNullOrEmpty(ret.ErrorMessage))
        {
          return View("New", model);
        }
        else
        {
          return RedirectToAction("Index");
        }
      }
      else
      {
        return View("New");
      }
    }

    [HttpGet]
    public ActionResult EditProject(long projectId)
    {
      var responseData = serviceClient.GetProjectById(projectId);
      if (responseData != null && string.IsNullOrEmpty(responseData.ErrorMessage))
      {
        ProjectViewModel project = new ProjectViewModel();
        project.Id = responseData.Id;
        project.Name = responseData.Name;
        project.Description = responseData.Description;
        var ownerRet = userServiceClient.GetUserById(responseData.OwnerId);
        project.Owner = ownerRet.UserName;

        var usernames = userServiceClient.GetAllUsers();
        //var users = JsonConvert.DeserializeObject<List<UserEntity>>(usernames);
        var userNamesList = usernames.Users.Select(x => x.UserName).ToList();
        ViewBag.Usernames = JsonConvert.SerializeObject(userNamesList);
        return View(project);
      }
      else
      {
        return View("Error", new ErrorModel() { Message = responseData.ErrorMessage });
      }

      
    }

    [HttpPost]
    public ActionResult EditProject(ProjectViewModel projectViewModel)
    {
      var ret = serviceClient.UpdateProject(MapToProjectEntity(projectViewModel));
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
    private ProjectEntity MapToProjectEntity(ProjectViewModel projectViewModel)
    {
      ProjectEntity model = new ProjectEntity();
      model.Id = projectViewModel.Id;
      model.Name = projectViewModel.Name;
      model.Description = projectViewModel.Description;
      var userRet = userServiceClient.GetUserByUsername(projectViewModel.Owner);
      //var user = JsonConvert.DeserializeObject<UserEntity>(userRet);
      model.OwnerId = userRet.Id;
      return model;
    }

    private List<ProjectViewModel> MapToProjectsViewModel(List<ProjectEntity> projects)
    {
      List<ProjectViewModel> viewMOdel = new List<ProjectViewModel>();
      foreach (var project in projects)
      {
        viewMOdel.Add(new ProjectViewModel() { Id = project.Id, Name = project.Name, Owner = project.Owner, Description = project.Description });
      }

      return viewMOdel;
    }

    #endregion
  }
}