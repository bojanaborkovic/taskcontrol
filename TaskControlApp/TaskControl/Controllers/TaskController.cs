using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TaskControl.Models;
using TaskControl.ServiceClients;
using TaskControl.ViewDataPreparers;
using TaskControlDTOs;
using System;

namespace TaskControl.Controllers
{
  [Authorize]
  public class TaskController : Controller
  {
    private TaskServiceClient taskServiceClient = new TaskServiceClient("tasks");
    private UserServiceClient userServiceClient = new UserServiceClient("users");
    private ProjectServiceClient projectServiceClient = new ProjectServiceClient();

    // GET: Tasks
    public ActionResult Index()
    {
      var ret = taskServiceClient.GetAllTasksDetails();
      var tasks = JsonConvert.DeserializeObject<List<TaskEntityExtended>>(ret);
      List<TaskSearchViewModel> viewModel = MapToViewModel(tasks);

      return View(viewModel);
    }

    [IssueTypePreparer, StatusPreparer, PriorityPreparer]
    public ActionResult Create()
    {
      var usernames = userServiceClient.GetAllUsers();
      var users = JsonConvert.DeserializeObject<List<UserEntity>>(usernames);
      var userNamesList = users.Select(x => x.UserName).ToList();
      ViewBag.Usernames = JsonConvert.SerializeObject(userNamesList);
      //var projectNames = projectServiceClient.GetAllProjects();
      //var projects = JsonConvert.DeserializeObject<List<ProjectEntity>>(projectNames);
      // ViewBag.ProjectNames = JsonConvert.SerializeObject(projects.Select(x => x.Name));
      return View("New");
    }

    [HttpPost]
    public ActionResult Create(TaskViewModel model)
    {
      if (ModelState.IsValid)
      {

        TaskEntity taskMapped = MapToEntity(model);
        var ret = taskServiceClient.CreateTask(taskMapped);

        if (string.IsNullOrEmpty(ret))
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
      var task = JsonConvert.DeserializeObject<TaskEntityExtended>(retTask);

      //var projects = projectServiceClient.GetAllProjects();
      //var projectList = JsonConvert.DeserializeObject<List<ProjectEntity>>(projects);

      var users = userServiceClient.GetAllUsers();
      var usersList = JsonConvert.DeserializeObject<List<UserEntity>>(users);

      // var projectNamesList = projectList.Select(x => x.Name).ToList();
      //ViewBag.ProjectNames = JsonConvert.SerializeObject(projectNamesList);
      ViewBag.UserNames = JsonConvert.SerializeObject(usersList.Select(x => x.UserName).ToList());
      TaskViewModel taskModel = MapToViewModel(task);
      return View("Edit", taskModel);
    }

    [HttpPost]
    public ActionResult Edit(TaskViewModel model)
    {
      if (ModelState.IsValid)
      {
        var updateTask = MapToEntity(model);
        var ret = taskServiceClient.UpdateTask(updateTask);
        return RedirectToAction("Index");
      }
      else
      {
        return View("Edit", model.TaskId);
      }
    }

    private TaskEntity MapToEntity(TaskViewModel model)
    {
      var asigneeRet = userServiceClient.GetUserByUsername(model.Asignee);
      var asignee = JsonConvert.DeserializeObject<UserEntity>(asigneeRet);

      var reporterRet = userServiceClient.GetUserByUsername(model.Reporter);
      var reporter = JsonConvert.DeserializeObject<UserEntity>(reporterRet);

      TaskEntity task = new TaskEntity();
      task.Asignee = asignee.Id;
      task.DateCreated = DateTime.UtcNow;
      task.Description = model.Description;
      task.DueDate = model.DueDate;
      task.Reporter = reporter.Id;
      task.IssueType = model.IssueType;
      task.Priority = model.Priority;
      task.Status = model.Status;
      task.ProjectId = model.Project;
      task.Title = model.Title;
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
          TaskId = item.TaskId
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
      taskModel.TaskId = taskEntity.TaskId;

      return taskModel;
    }
    #endregion
  }
}