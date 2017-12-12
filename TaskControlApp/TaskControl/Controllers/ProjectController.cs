using BussinesService.Interfaces.Responses.Project;
using DataModel.UnitOfWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
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
    private long? currentUserId;

    public ProjectController()
    {
      if (currentUserId == null)
      {
        string user = System.Web.HttpContext.Current.User.Identity.Name;
        var userRet = userServiceClient.GetUserByUsername(user);
        if (userRet != null)
        {
          currentUserId = userRet.Id;
        }
      }
    }


    // GET: Project
    [HttpGet]
    public ActionResult Index()
    {
      GetProjectReturn responseData = serviceClient.GetAllProjects((long)currentUserId);

      if (responseData != null && responseData.Projects.Count > 0)
      {
        return View("Index", MapToProjectsViewModel(responseData.Projects));
      }
      else
      {
        return View("Index", new List<ProjectViewModel>());
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
        return View("Error", new ErrorModel() { Message = responseData != null ? responseData.ErrorMessage : "Error during fetching project!" });
      }


    }

    [HttpGet]
    public ActionResult ViewProject(long projectId)
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

      var responseData = serviceClient.GetProjectById(projectId);
      if (responseData != null && string.IsNullOrEmpty(responseData.ErrorMessage))
      {
        ProjectViewModel project = new ProjectViewModel();
        project.Name = responseData.Name;
        project.Description = responseData.Description;
        project.Id = projectId;
        var ownerRet = userServiceClient.GetUserById(responseData.OwnerId);
        project.Owner = ownerRet.UserName;

        var statistics = serviceClient.GetProjectStatistics(projectId);
        int toDoCount = 0;
        int completedCount = 0;
        int inProgressCount = 0;
        if (statistics != null && statistics.Tasks.Count > 0)
        {
          toDoCount = statistics.Tasks.Where(x => x.Status == (int)Status.ToDo).ToList().Count;
          completedCount = statistics.Tasks.Where(x => x.Status == (int)Status.Done).ToList().Count;
          inProgressCount = statistics.Tasks.Where(x => x.Status == (int)Status.InProgress).ToList().Count;
        }
        project.ToDoCount = toDoCount;
        project.CompletedCount = completedCount;
        project.InProgressCount = inProgressCount;

        if (toDoCount == 0 && completedCount == 0 && inProgressCount == 0)
        {
          project.TotalProgress = 0M;
        }
        else
        {
          project.TotalProgress = (decimal)((decimal)(completedCount) / (decimal)(toDoCount + inProgressCount + completedCount)) * 100;
        }

        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberDecimalSeparator = ".";

        project.TotalProgress.ToString(nfi);


        ProjectNotesReturn notesRet = serviceClient.GetProjectNotes(projectId);
        if (notesRet != null && notesRet.RecordCount > 0)
        {
          project.Notes = MapProjectNotes(notesRet.Notes);
        }

        return View("View", project);
      }
      else
      {
        return View("Error", new ErrorModel() { Message = responseData != null ? responseData.ErrorMessage : "Error during fetching project!" });
      }
    }

    private List<ProjectNoteViewModel> MapProjectNotes(List<Note> notes)
    {
      List<ProjectNoteViewModel> projectNotes = new List<ProjectNoteViewModel>();

      foreach (var item in notes)
      {
        projectNotes.Add(new ProjectNoteViewModel()
        {
          AuthorId = item.AuthorId,
          AuthorName = item.AuthorName,
          CommentDate = item.DateCreated,
          Note = item.Content
        });
      }

      return projectNotes;
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

    [HttpGet]
    public ActionResult AddNewNote()
    {
      ProjectNoteViewModel projectNote = new ProjectNoteViewModel();
      string userName = System.Web.HttpContext.Current.User.Identity.Name;
      var user = userServiceClient.GetUserByUsername(userName);
      if (user != null)
      {
        projectNote.AuthorId = user.Id;
        projectNote.AuthorName = user.UserName;
      }
      return PartialView("AddNewNote", projectNote);
    }

    [HttpPost]
    public JsonResult AddNewNote(AddNewNote model)
    {
      string userName = System.Web.HttpContext.Current.User.Identity.Name;
      var user = userServiceClient.GetUserByUsername(userName);
      if (user != null && model != null)
      {
        Note noteToAdd = new Note();
        noteToAdd.AuthorId = user.Id;
        noteToAdd.AuthorName = user.UserName;
        noteToAdd.Content = model.Note;
        noteToAdd.DateCreated = DateTime.UtcNow;
        noteToAdd.ProjectId = long.Parse(model.ProjectId);
        var ret = serviceClient.AddNewNote(noteToAdd);
      }
      long projectId = long.Parse(model.ProjectId);
      var records = serviceClient.GetProjectNotes(projectId);
      var serializedRecords = JsonConvert.SerializeObject(records, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      });

      return Json(new { success = true });
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
      Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

      List<ProjectViewModel> viewMOdel = new List<ProjectViewModel>();
      foreach (var project in projects)
      {
        var statistics = serviceClient.GetProjectStatistics(project.Id);
        int toDoCount = 0;
        int completedCount = 0;
        int inProgressCount = 0;
        if (statistics != null && statistics.Tasks.Count > 0)
        {
          toDoCount = statistics.Tasks.Where(x => x.Status == (int)Status.ToDo).ToList().Count;
          completedCount = statistics.Tasks.Where(x => x.Status == (int)Status.Done).ToList().Count;
          inProgressCount = statistics.Tasks.Where(x => x.Status == (int)Status.InProgress).ToList().Count;
        }


        var projectViewModel = new ProjectViewModel()
        {
          Id = project.Id,
          Name = project.Name,
          Owner = project.Owner,
          Description = project.Description,
          ToDoCount = toDoCount,
          CompletedCount = completedCount,
          InProgressCount = inProgressCount
        };

        if (toDoCount == 0 && completedCount == 0 && inProgressCount == 0)
        {
          projectViewModel.TotalProgress = 0M;
        }
        else
        {
          projectViewModel.TotalProgress = (decimal)((decimal)(completedCount) / (decimal)(toDoCount + inProgressCount + completedCount)) * 100;
        }

        viewMOdel.Add(projectViewModel);
      }

      return viewMOdel;
    }

    #endregion
  }
}