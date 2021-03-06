﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BussinesService.Interfaces.Responses.Task;
using TaskControl.Models;
using TaskControl.ServiceClients;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TaskControlDTOs;
using TaskControl.ViewDataPreparers;
using BussinesService.Interfaces.Responses.Project;
using Newtonsoft.Json.Converters;

namespace TaskControl.Controllers
{
  //[Authorize(Roles = "Admin, IT Admin, User")]
  public class DashboardController : Controller
  {
    private TaskServiceClient taskServiceClient = new TaskServiceClient("tasks") { DoSerialize = true };
    private UserServiceClient userServiceClient = new UserServiceClient("users") { DoSerialize = true };
    private ProjectServiceClient projectServiceClient = new ProjectServiceClient() { DoSerialize = true };

    // GET: Dashboard
    [StatusPreparer]
    public ActionResult Index()
    {
      DashboardViewModel model = new DashboardViewModel();

      string userName = System.Web.HttpContext.Current.User.Identity.Name;
      var user = userServiceClient.GetUserByUsername(userName);


      if (user != null)
      {
        ViewBag.UserId = user.Id.ToString();
      }

      var retTaskDetails = taskServiceClient.GetAllTasksDetails(GetUserId());

      if (user != null)
      {
        var tasksForUser = taskServiceClient.GetTasksForUser(user.Id, null);
        if (tasksForUser != null && tasksForUser.RecordCount > 0)
        {
          model.TaskViewModel = MapTasksToDashboard(tasksForUser);
        }
      }

      var taskHistory = taskServiceClient.GetTaskHistory(null);
      if (taskHistory != null && taskHistory.RecordCount > 0)
      {
        model.TaskAuditViewModel = new List<TaskAudit>();
        if (taskHistory != null)
        {
          var take = taskHistory.TasksAudit.Take(50).ToList();
          model.TaskAuditViewModel = take;
        }
      }

      var projectsForOwner = projectServiceClient.GetProjectsByOwner(user.Id);


      if (projectsForOwner != null)
      {
        model.OwnersProjects = MapOwnersProjects(projectsForOwner);
      }


      ViewBag.TaskList = JsonConvert.SerializeObject(model.TaskViewModel, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      });
      return View("Index", model);
    }

    [HttpGet]
    public ActionResult GetAllTasks()
    {
      var ret = taskServiceClient.GetAllTasksDetails(GetUserId());

      var records = MapTasksToDashboard(ret);
      var serializedRecords = JsonConvert.SerializeObject(records, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      });

      return Content(serializedRecords, "application/json");
    }

    [HttpGet]
    [StatusPreparer]
    public ActionResult GetTaskForUser(long userId)
    {
      var ret = taskServiceClient.GetTasksForUser(userId, null);
      var records = MapTasksToDashboard(ret);
      var serializedRecords = JsonConvert.SerializeObject(records, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      });

      return Content(serializedRecords, "application/json");
    }

    [HttpGet]
    [StatusPreparer]
    public ActionResult GetTasksForUserOnProject(long? projectId)
    {
      string userName = System.Web.HttpContext.Current.User.Identity.Name;
      var user = userServiceClient.GetUserByUsername(userName);
      SearchTasksReturn ret = new SearchTasksReturn();
      if (user != null)
      {
        ret = taskServiceClient.GetTasksForUser(user.Id, projectId);
      }
      DashboardViewModel model = new DashboardViewModel();
      model.TaskViewModel = new List<DashboardTaskViewModel>();

      if (ret != null && ret.RecordCount > 0)
      {
        var records = MapTasksToDashboard(ret);
        model.TaskViewModel = records;
      }


      return PartialView("TasksOnProject", model);
    }


    [HttpGet]
    public ActionResult GetTaskInfoById(long taskId)
    {
      var ret = taskServiceClient.GetTaskByIdCustom(taskId);
      var records = MapTaskToDashboard(ret);

      var language = HttpContext.Request.RequestContext.RouteData.Values["lang"];

      ViewBag.CurrentLanguage = language;

      var serializedRecords = JsonConvert.SerializeObject(records, new IsoDateTimeConverter() { DateTimeFormat = "dd-MM-yyyy HH:mm:ss" });

      return Content(serializedRecords, "application/json");
    }

    [HttpPost]
    [StatusPreparer]
    public JsonResult UpdateTaskStatus(UpdateTaskStatusModel update)
    {
      var statuses = ViewData[StatusPreparer.ViewDataKey] as List<StatusEntity>;
      var statusToUpdate = statuses.Where(x => x.Name == update.StatusName).FirstOrDefault();
      long TaskID = long.Parse(update.TaskId);

      taskServiceClient.UpdateTaskStatus(new UpdateTaskStatus() { TaskId = TaskID, StatusId = statusToUpdate.Id });
      return Json(new { success = true });
    }

    [HttpGet]
    public ActionResult Calendar()
    {
      string userName = System.Web.HttpContext.Current.User.Identity.Name;
      long userId = userServiceClient.GetUserByUsername(userName).Id;
      var ret = taskServiceClient.GetAllTasksDetails(GetUserId());
      ViewBag.UserId = userId.ToString();

      var projectsForOwner = projectServiceClient.GetProjectsByOwner(userId);

      var currentLanguage = HttpContext.Request.RequestContext.RouteData.Values["lang"];
      ViewBag.CurrentLanguage = currentLanguage;

      DashboardViewModel model = new DashboardViewModel();
      model.OwnersProjects = new List<ProjectViewModel>();

      model.TaskViewModel = new List<DashboardTaskViewModel>();

      if (ret != null)
      {
        model.TaskViewModel = MapTasksToDashboard(ret);
      }

      if (projectsForOwner != null)
      {
        model.OwnersProjects = MapOwnersProjects(projectsForOwner);
      }

      //get user defined language
      string language = userServiceClient.GetUserLanguage(userId).LanguageCode;
      string languageCode = language + "_cyrl";
      ViewBag.Language = languageCode;

      ViewBag.TaskList = JsonConvert.SerializeObject(model.TaskViewModel, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      });
      return View("Calendar", model);
    }


    [HttpGet]
    [StatusPreparer]
    public ActionResult GetTasksOnProject(long projectId)
    {
      var ret = taskServiceClient.GetTasksOnProject(projectId);
      var records = MapTasksToDashboard(ret);

      var serializedRecords = JsonConvert.SerializeObject(records, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      });

      return Content(serializedRecords, "application/json");
    }


    public ActionResult Details(int id)
    {
      return View();
    }


    [HttpGet]
    public ActionResult GetAllHistory()
    {
      var taskHistory = taskServiceClient.GetTaskHistory(null);
      List<TaskAudit> model = new List<TaskAudit>();

      if (taskHistory != null && taskHistory.RecordCount > 0)
      {
        model = taskHistory.TasksAudit;
      }

      return View("TaskHistory", model);
    }

    #region mappers

    private List<DashboardTaskViewModel> MapTasksToDashboard(TasksDetailsReturn ret)
    {
      List<DashboardTaskViewModel> tasksDashboard = new List<DashboardTaskViewModel>();

      if (ret != null && ret.RecordCount > 0)
      {
        foreach (var task in ret.Tasks)
        {
          tasksDashboard.Add(new DashboardTaskViewModel()
          {
            Id = task.Id,
            Title = task.Title,
            Start = task.DateCreated,
            End = task.DueDate,
            Color = "#5cb85c"

          });
        }
      }

      return tasksDashboard;
    }

    private List<ProjectViewModel> MapOwnersProjects(GetProjectReturn projectsForOwner)
    {
      List<ProjectViewModel> projectsOnDashboard = new List<ProjectViewModel>();

      if (projectsForOwner != null && projectsForOwner.RecordCount > 0)
      {
        foreach (var project in projectsForOwner.Projects)
        {
          projectsOnDashboard.Add(new ProjectViewModel()
          {
            Id = project.Id,
            Name = project.Name,
            OwnerId = project.OwnerId,
            Owner = project.Owner,
            Description = project.Description

          });
        }
      }

      return projectsOnDashboard;
    }


    //private List<TaskAuditViewModel> MapTasksAudit(List<TaskAudit> tasksAudit)
    //{
    //  List<TaskAuditViewModel> auditList = new List<TaskAuditViewModel>();
    //  var statuses = ViewData[StatusPreparer.ViewDataKey] as List<StatusEntity>;



    //  if (tasksAudit != null && tasksAudit.Count > 0)
    //  {
    //    foreach (var item in tasksAudit)
    //    {
    //      TaskAuditViewModel auditItem = new TaskAuditViewModel();
    //      auditItem.AsigneeAfter = item.AsigneeAfter;
    //      auditItem.AsigneeBefore = item.AsigneeBefore;
    //      auditItem.AsigneeBeforeUsername = item.AsigneeBefore != null ? userServiceClient.GetUserById((long)item.AsigneeBefore).UserName : string.Empty;

    //      auditItem.AsigneeChangedOn = item.AsigneeChangedOnDate;
    //      auditItem.AsigneeAfterUsername = item.AsigneeAfter != null ? userServiceClient.GetUserById((long)item.AsigneeAfter).UserName : string.Empty;

    //      auditItem.StatusBefore = item.StatusBefore;
    //      auditItem.StatusAfter = item.StatusAfter;

    //      auditItem.StatusBeforeName = item.StatusBefore != null ? statuses.Where(x => x.Id == item.StatusBefore).FirstOrDefault().Name : string.Empty;
    //      auditItem.StatusAfterName = item.StatusAfter != null ? statuses.Where(x => x.Id == item.StatusAfter).FirstOrDefault().Name : string.Empty;

    //      auditItem.StatusChangeBy = item.StatusChangedBy != null ? userServiceClient.GetUserById((long)item.StatusChangedBy).UserName : string.Empty;
    //      auditItem.AsigneeChangedBy = item.AsigneeChangedBy != null ? userServiceClient.GetUserById((long)item.AsigneeChangedBy).UserName : string.Empty;

    //      auditItem.AsigneeChangedById = item.AsigneeChangedBy;
    //      auditItem.TaskId = item.TaskId;

    //      auditList.Add(auditItem);
    //    }
    //  }

    //  return auditList;
    //}

    private List<DashboardTaskViewModel> MapTasksToDashboard(SearchTasksReturn ret)
    {
      List<DashboardTaskViewModel> tasksDashboard = new List<DashboardTaskViewModel>();
      var statuses = ViewData[StatusPreparer.ViewDataKey] as List<StatusEntity>;

      foreach (var task in ret.Tasks)
      {
        tasksDashboard.Add(new DashboardTaskViewModel()
        {
          Id = task.Id,
          Title = task.Title,
          Start = task.DateCreated,
          End = task.DueDate,
          Status = statuses.Where(x => x.Id == task.Status).First().Name,
          Color = "#337ab7"

        });
      }

      return tasksDashboard;
    }

    private List<TaskAuditViewModel> MapTasksAudit(List<TaskAudit> tasksAudit)
    {
      throw new NotImplementedException();
    }

    private DashboardTaskViewModel MapTaskToDashboard(TaskEntityExtendedReturn ret)
    {
      DashboardTaskViewModel task = new DashboardTaskViewModel();
      task.Id = ret.Id;
      task.Title = ret.Title;
      task.Status = ret.Status;
      task.Start = ret.DateCreated;
      task.End = ret.DueDate;
      task.Asignee = ret.Asignee;

      return task;
    }
    private long GetUserId()
    {
      string userName = System.Web.HttpContext.Current.User.Identity.Name;
      var user = userServiceClient.GetUserByUsername(userName);


      if (user != null)
      {
        return user.Id;
      }
      else
      {
        return 0;
      }
    }

    #endregion
  }
}
