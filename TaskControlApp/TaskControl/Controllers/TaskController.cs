using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TaskControl.Models;
using TaskControl.ServiceClients;
using TaskControl.ViewDataPreparers;
using TaskControlDTOs;
using System;
using Microsoft.AspNet.Identity;

namespace TaskControl.Controllers
{
  [Authorize]
  public class TaskController : Controller
  {
    private TaskServiceClient taskServiceClient = new TaskServiceClient("tasks") { DoSerialize = true };
    private UserServiceClient userServiceClient = new UserServiceClient("users") { DoSerialize = true };
    private ProjectServiceClient projectServiceClient = new ProjectServiceClient();

    // GET: Tasks
    public ActionResult Index()
    {
      string user = System.Web.HttpContext.Current.User.Identity.Name;


      var ret = taskServiceClient.GetAllTasksDetails();
      List<TaskSearchViewModel> viewModel = MapToViewModel(ret.Tasks);

      return View(viewModel);
    }

    [IssueTypePreparer, StatusPreparer, PriorityPreparer]
    public ActionResult Create()
    {
      var usernames = userServiceClient.GetAllUsers();
      //var users = JsonConvert.DeserializeObject<List<UserEntity>>(usernames);
      var userNamesList = usernames.Users.Select(x => x.UserName).ToList();
      ViewBag.Usernames = JsonConvert.SerializeObject(userNamesList);
      var projects = projectServiceClient.GetAllProjects();
      //var projects = JsonConvert.DeserializeObject<List<ProjectEntity>>(projectNames);
      ViewBag.ProjectNames = JsonConvert.SerializeObject(projects.Projects.Select(x => x.Name));
      return View("New");
    }

    [HttpPost]
    public ActionResult Create(TaskViewModel model)
    {
      string user = System.Web.HttpContext.Current.User.Identity.Name;
      var userRet = userServiceClient.GetUserByUsername(user);
      model.CreatedBy = userRet != null ? userRet.Id : 0;

      if (ModelState.IsValid)
      {

        TaskEntity taskMapped = MapToEntity(model);
        var ret = taskServiceClient.CreateTask(taskMapped);

        if (ret != null && !string.IsNullOrEmpty(ret.ErrorMessage))
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
        return View("New", model);
      }

    }

    [HttpGet]
    [IssueTypePreparer, StatusPreparer, PriorityPreparer]
    public ActionResult Edit(long taskId)
    {
      var retTask = taskServiceClient.GetTaskByIdCustom(taskId);

      var projects = projectServiceClient.GetAllProjects();
      var users = userServiceClient.GetAllUsers();

      ViewBag.ProjectNames = JsonConvert.SerializeObject(projects.Projects);
      ViewBag.UserNames = JsonConvert.SerializeObject(users.Users.Select(x => x.UserName).ToList());
      TaskViewModel taskModel = MapToViewModel(retTask);
      return View("Edit", taskModel);
    }

    [HttpGet]
    [IssueTypePreparer, StatusPreparer, PriorityPreparer]
    public ActionResult Preview(long taskId)
    {
      var retTask = taskServiceClient.GetTaskByIdCustom(taskId);
      TaskViewModel model = MapToViewModel(retTask);
      return View("Preview", model);

    }

    [HttpPost]
    [IssueTypePreparer, StatusPreparer, PriorityPreparer]
    public ActionResult Edit(TaskViewModel model)
    {
      var projects = projectServiceClient.GetAllProjects();
      var users = userServiceClient.GetAllUsers();

      ViewBag.ProjectNames = JsonConvert.SerializeObject(projects.Projects);
      ViewBag.UserNames = JsonConvert.SerializeObject(users.Users.Select(x => x.UserName).ToList());

      if (ModelState.IsValid)
      {
        var updateTask = MapToEntity(model);
        var ret = taskServiceClient.UpdateTask(updateTask);
        return RedirectToAction("Index");
      }
      else
      {
        return View("Edit", model);
      }
    }

    private TaskEntity MapToEntity(TaskViewModel model)
    {
      var asigneeRet = userServiceClient.GetUserByUsername(model.Asignee);
      //var asignee = JsonConvert.DeserializeObject<UserEntity>(asigneeRet);

      var reporterRet = userServiceClient.GetUserByUsername(model.Reporter);
      //var reporter = JsonConvert.DeserializeObject<UserEntity>(reporterRet);

     // var projectID = projectServiceClient.GetProjectByName(model.ProjectName);

      TaskEntity task = new TaskEntity();
      task.Asignee = asigneeRet != null ? asigneeRet.Id : 0;
      task.DateCreated = DateTime.UtcNow;
      task.CreatedBy = model.CreatedBy;
      task.Description = model.Description;
      task.DueDate = model.DueDate;
      task.Reporter = reporterRet != null ? reporterRet.Id : 0;
      task.IssueType = model.IssueType;
      task.Priority = model.Priority;
      task.Status = model.Status;
      task.ProjectId = model.Project;
      task.Title = model.Title;
      task.Id = model.Id;
      return task;
    }

    #region mappers

    private List<TaskSearchViewModel> MapToViewModel(List<TaskEntityExtended> tasks)
    {
      List<TaskSearchViewModel> viewmodel = new List<TaskSearchViewModel>();
      foreach (var item in tasks)
      {
        viewmodel.Add(new TaskSearchViewModel()
        {
          Asignee = item.Asignee,
          Description = item.Description,
          Status = item.Status,
          Project = item.Project,
          Title = item.Title,
          IssueType = item.IssueType,
          DateCreated = item.DateCreated,
          DueDate = item.DueDate,
          Reporter = item.Reporter,
          Priority = item.Priority,
          TaskId = item.Id
        });
      }
      return viewmodel;
    }

    private TaskViewModel MapToViewModel(TaskEntityExtended taskEntity)
    {
      TaskViewModel taskModel = new TaskViewModel();
      taskModel.Asignee = taskEntity.Asignee;
      taskModel.DateCreated = DateTime.UtcNow;
      taskModel.Description = taskEntity.Description;
      taskModel.DueDate = taskEntity.DueDate;
      taskModel.Reporter = taskEntity.Reporter;
      taskModel.Project = taskEntity.ProjectId;
      taskModel.ProjectName = taskEntity.Project;
      taskModel.Title = taskEntity.Title;
      taskModel.Priority = taskEntity.PriorityId;
      taskModel.IssueType = taskEntity.IssueTypeId;
      taskModel.Status = (int)taskEntity.StatusId;
      taskModel.Id = taskEntity.Id;

      return taskModel;
    }
    #endregion
  }
}