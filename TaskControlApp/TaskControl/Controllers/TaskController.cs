﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TaskControl.Models;
using TaskControl.ServiceClients;
using TaskControl.ViewDataPreparers;
using TaskControlDTOs;
using System;
using Microsoft.AspNet.Identity;
using PagedList;
using Newtonsoft.Json.Serialization;
using BussinesService.Interfaces.Responses.Task;
using BussinesService.Interfaces.Responses.Project;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Text;
using System.Globalization;
using System.Threading;

namespace TaskControl.Controllers
{
  [Authorize]
  public class TaskController : Controller
  {
    private TaskServiceClient taskServiceClient = new TaskServiceClient("tasks") { DoSerialize = true };
    private UserServiceClient userServiceClient = new UserServiceClient("users") { DoSerialize = true };
    private ProjectServiceClient projectServiceClient = new ProjectServiceClient();

    public TaskController()
    {
      Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
    }

    // GET: Tasks
    //public ActionResult Index()
    //{
    //  var ret = taskServiceClient.GetAllTasksDetails();
    //  List<TaskSearchViewModel> viewModel = new List<TaskSearchViewModel>();
    //  if (ret != null && ret.RecordCount > 0)
    //  {
    //    viewModel = MapToViewModel(ret.Tasks);
    //  }

    //  return View("Index", viewModel.ToPagedList(1, 10));
    //}

    [HttpGet]
    public ActionResult Index(string searchString, string sortOption, int page = 1)
    {
      int pageSize = 10;
      //var usersRet = serviceClient.SearchUsers();
      var tasksRet = taskServiceClient.GetAllTasksDetails(GetUserId());
      ViewBag.CurrentFilter = searchString;
      page = page > 0 ? page : 1;
      pageSize = pageSize > 0 ? pageSize : 10;

      ViewBag.AsigneeSortParam = sortOption == "asignee" ? "asignee_desc" : "asignee";
      ViewBag.DueDateSortParam = sortOption == "duedate" ? "duedate_desc" : "duedate";
      ViewBag.StatusSortParam = sortOption == "status" ? "status_desc" : "status";
      ViewBag.IdSortParam = sortOption == "Id" ? "Id_desc" : "Id";

      ViewBag.CurrentSort = sortOption;

      List<TaskEntityExtended> sortedTasks = new List<TaskEntityExtended>();

      if (tasksRet != null && tasksRet.RecordCount > 0)
      {
        if (!string.IsNullOrEmpty(searchString))
        {
          tasksRet.Tasks = tasksRet.Tasks.Where(x => x.Title.ToLower().Contains(searchString) || x.Description.ToLower().Contains(searchString)).ToList();
        }
      }

      long userId = GetUserId();

      var projectsForOwner = projectServiceClient.GetProjectsByOwner(userId);


      if (projectsForOwner != null)
      {
        ViewBag.OwnersProject = MapOwnersProjects(projectsForOwner);
      }


      if (tasksRet != null && tasksRet.RecordCount > 0)
      {
        switch (sortOption)
        {
          case "asignee_desc":
            sortedTasks = tasksRet.Tasks.OrderByDescending(x => x.Asignee).ToList();
            break;
          case "asignee":
            sortedTasks = tasksRet.Tasks.OrderBy(x => x.Asignee).ToList();
            break;
          case "duedate_desc":
            sortedTasks = tasksRet.Tasks.OrderByDescending(x => x.DueDate).ToList();
            break;
          case "duedate":
            sortedTasks = tasksRet.Tasks.OrderBy(x => x.DueDate).ToList();
            break;
          case "status_desc":
            List<TaskEntityExtended> list = tasksRet.Tasks.ToList();
            var result = tasksRet.Tasks.GroupBy(u => u.Status).Select(grp => new { Status = grp.Key, list = grp.ToList() }).ToList();
            var groupedList = result.SelectMany(x => x.list).ToList();
            sortedTasks = groupedList;
            break;
          case "Id":
            sortedTasks = tasksRet.Tasks.OrderBy(x => x.Id).ToList();
            break;
          default:
            sortedTasks = tasksRet.Tasks.OrderByDescending(x => x.DateCreated).ToList();
            break;

        }
      }

      var mappedTasks = MapToViewModel(sortedTasks);

      if (Session["TaskList"] != null)
      {
        Session["TaskList"] = null;
        Session["TaskList"] = mappedTasks;
      }
      else
      {
        Session["TaskList"] = mappedTasks;
      }


      //return View("Index", mappedTasks.ToPagedList(pageNumber, pageSize));
      return Request.IsAjaxRequest()
               ? (ActionResult)PartialView("TaskList", mappedTasks.ToPagedList(page, pageSize))
               : View(mappedTasks.ToPagedList(page, pageSize));
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

    [IssueTypePreparer, StatusPreparer, PriorityPreparer]
    public ActionResult Create()
    {
      PrepareViewModel();
      return View("New");
    }

    [HttpPost]
    [IssueTypePreparer, StatusPreparer, PriorityPreparer]
    public ActionResult Create(TaskViewModel model)
    {
      if (ModelState.IsValid)
      {
        string user = System.Web.HttpContext.Current.User.Identity.Name;
        var userRet = userServiceClient.GetUserByUsername(user);
        model.CreatedBy = userRet != null ? userRet.Id : 0;
        TaskEntity taskMapped = MapToEntity(model);

        var ret = taskServiceClient.CreateTask(taskMapped);

        if (ret != null && !string.IsNullOrEmpty(ret.ErrorMessage))
        {
          PrepareViewModel();
          return View("New", model);
        }
        else
        {
          return RedirectToAction("Index");
        }
      }
      else
      {
        PrepareViewModel();
        return View("New", model);
      }

    }

    [HttpGet]
    [IssueTypePreparer, StatusPreparer, PriorityPreparer]
    public ActionResult Edit(long taskId)
    {
      var retTask = taskServiceClient.GetTaskByIdCustom(taskId);

      var projects = projectServiceClient.GetAllProjects(GetUserId());
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
      TaskViewModel model = new TaskViewModel();

      TaskCommentsReturn comments = taskServiceClient.GetTaskComments(taskId);

      if (retTask != null)
      {
        model = MapToViewModel(retTask);
      }

      if (comments != null)
      {
        MapCommentsToViewModel(model, comments);
      }

      var statuses = ViewData[StatusPreparer.ViewDataKey] as List<StatusEntity>;
      ViewBag.Statuses = JsonConvert.SerializeObject(statuses);
      return View("Preview", model);

    }

    private void MapCommentsToViewModel(TaskViewModel model, TaskCommentsReturn comments)
    {
      model.TaskComments = new List<Models.Comment>();

      foreach (var comment in comments.TaskComments)
      {
        model.TaskComments.Add(new Models.Comment()
        {
          AuthorId = comment.AuthorId,
          AuthorName = comment.AuthorName,
          DateCreated = comment.DateCreated,
          Content = comment.Content
        });
      }
    }

    [HttpPost]
    [IssueTypePreparer, StatusPreparer, PriorityPreparer]
    public ActionResult Edit(TaskViewModel model)
    {
      var projects = projectServiceClient.GetAllProjects(GetUserId());
      var users = userServiceClient.GetAllUsers();

      string userName = System.Web.HttpContext.Current.User.Identity.Name;
      var user = userServiceClient.GetUserByUsername(userName);
      model.CreatedBy = user != null ? user.Id : 0;
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

    [HttpGet]
    public ActionResult GetActivityOnTask(string taskId)
    {
      long taskID = long.Parse(taskId);
      var ret = taskServiceClient.GetTaskHistory(taskID);
      var serializedRecords = JsonConvert.SerializeObject(ret.TasksAudit, new JsonSerializerSettings
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

      List<TaskSearchViewModel> mappedTasks = new List<TaskSearchViewModel>();

      if (ret != null && ret.RecordCount > 0)
      {
        mappedTasks = MapTasksToViewModel(ret.Tasks);
      }
      Session["TaskList"] = mappedTasks;

      return PartialView("TaskList", mappedTasks.ToPagedList(1, 10));
    }

    public ActionResult GenerateExcel()
    {

      // Step 1 - get the data from database
      var taskListToExport = (List<TaskSearchViewModel>)Session["TaskList"];
      var data = taskListToExport;

      // instantiate the GridView control from System.Web.UI.WebControls namespace
      // set the data source
      GridView gridview = new GridView();
      gridview.DataSource = data;
      gridview.DataBind();

      // Clear all the content from the current response
      Response.ClearContent();
      Response.Buffer = true;
      // set the header
      Response.AddHeader("content-disposition", "attachment;filename = TaskList.xls");

      Response.ContentType = "application/ms-excel";
      Response.Charset = "";
      // create HtmlTextWriter object with StringWriter
      using (StringWriter sw = new StringWriter())
      {
        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
        {
          // render the GridView to the HtmlTextWriter
          gridview.RenderControl(htw);
          // Output the GridView content saved into StringWriter
          Response.Output.Write(sw.ToString());
          Response.Flush();
          Response.End();
        }
      }
      return View();
    }

    [HttpGet]
    public ActionResult AddNewComment()
    {
      Models.Comment taskComment = new Models.Comment();
      string userName = System.Web.HttpContext.Current.User.Identity.Name;
      var user = userServiceClient.GetUserByUsername(userName);
      if (user != null)
      {
        taskComment.AuthorId = user.Id;
        taskComment.AuthorName = user.UserName;
      }
      return PartialView("AddNewNote", taskComment);
    }

    private List<TaskSearchViewModel> MapTasksToViewModel(List<TaskEntity> tasks)
    {
      List<TaskSearchViewModel> viewmodel = new List<TaskSearchViewModel>();
      foreach (var item in tasks)
      {
        viewmodel.Add(new TaskSearchViewModel()
        {
          Asignee = item.AsigneeUsername,
          Description = item.Description,
          Status = item.StatusName,
          Project = item.ProjectId.ToString(),
          Title = item.Title,
          IssueType = item.IssueTypeName,
          DateCreated = item.DateCreated,
          DueDate = item.DueDate,
          Reporter = item.ReporterName,
          Priority = item.PriorityName,
          TaskId = item.Id
        });
      }
      return viewmodel;
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
      var statuses = ViewData[StatusPreparer.ViewDataKey] as List<StatusEntity>;
      var issueTypes = ViewData[IssueTypePreparer.ViewDataKey] as List<IssueTypeEntity>;
      var priorities = ViewData[PriorityPreparer.ViewDataKey] as List<PriorityEntity>;

      TaskViewModel taskModel = new TaskViewModel();
      taskModel.Asignee = taskEntity.Asignee;
      taskModel.DateCreated = DateTime.UtcNow;
      taskModel.Description = taskEntity.Description;
      taskModel.DueDate = taskEntity.DueDate;
      taskModel.Reporter = taskEntity.Reporter;
      taskModel.Project = taskEntity.ProjectId;
      taskModel.ProjectName = taskEntity.Project;
      taskModel.Title = taskEntity.Title;
      taskModel.PriorityName = priorities.Where(x => x.Id == taskEntity.PriorityId).First().Name;
      taskModel.IssueTypeName = issueTypes.Where(x => x.Id == taskEntity.IssueTypeId).First().Name;
      taskModel.Status = (int)taskEntity.StatusId;
      taskModel.StatusName = statuses.Where(x => x.Id == taskEntity.StatusId).First().Name;
      taskModel.Id = taskEntity.Id;


      return taskModel;
    }

    private void PrepareViewModel()
    {
      var usernames = userServiceClient.GetAllUsers();
      if (usernames != null && usernames.RecordCount > 0)
      {
        var userNamesList = usernames.Users.Select(x => x.UserName).ToList();
        ViewBag.Usernames = JsonConvert.SerializeObject(userNamesList);
      }
      var projects = projectServiceClient.GetAllProjects(GetUserId());

      if (projects != null && projects.RecordCount > 0)
      {
        ViewBag.ProjectNames = JsonConvert.SerializeObject(projects.Projects.Select(x => x.Name));
      }
    }

    private TaskEntity MapToEntity(TaskViewModel model)
    {
      var asigneeRet = userServiceClient.GetUserByUsername(model.Asignee);
      //var asignee = JsonConvert.DeserializeObject<UserEntity>(asigneeRet);

      var reporterRet = userServiceClient.GetUserByUsername(model.Reporter);
      //var reporter = JsonConvert.DeserializeObject<UserEntity>(reporterRet);

      var project = projectServiceClient.GetProjectByName(model.ProjectName);

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
      task.ProjectId = project.Id;
      task.Title = model.Title;
      task.Id = model.Id;
      return task;
    }

    private ProjectFilterViewModel MapOwnersProjects(GetProjectReturn projectsForOwner)
    {
      ProjectFilterViewModel model = new ProjectFilterViewModel();
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

      model.OwnersProject = projectsOnDashboard;

      return model;
    }
    #endregion
  }
}