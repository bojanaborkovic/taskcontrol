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
using PagedList;

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
      var ret = taskServiceClient.GetAllTasksDetails();
      List<TaskSearchViewModel> viewModel = new List<TaskSearchViewModel>();
      if (ret != null && ret.RecordCount > 0)
      {
        viewModel = MapToViewModel(ret.Tasks);
      }

      return View("Index", viewModel.ToPagedList(1, 10));
    }

    [HttpGet]
    public ActionResult Search(string sortOrder, string currentFilter, string searchString, int pageNumber = 1, int pageSize = 10)
    {
      //var usersRet = serviceClient.SearchUsers();
      var tasksRet = taskServiceClient.GetAllTasksDetails();
      ViewBag.CurrentFilter = searchString;
      pageNumber = pageNumber > 0 ? pageNumber : 1;
      pageSize = pageSize > 0 ? pageSize : 10;

      ViewBag.AsigneeSortParam = sortOrder == "asignee" ? "asignee_desc" : "asignee";
      ViewBag.DueDateSortParam = sortOrder == "duedate" ? "duedate_desc" : "duedate";
      ViewBag.StatusSortParam = sortOrder == "status" ? "status_desc" : "status";
      ViewBag.IdSortParam = sortOrder == "Id" ? "Id_desc" : "Id";

      ViewBag.CurrentSort = sortOrder;

      List<TaskEntityExtended> sortedTasks = new List<TaskEntityExtended>();

      if (!string.IsNullOrEmpty(searchString))
      {
        tasksRet.Tasks = tasksRet.Tasks.Where(x => x.Title.Contains(searchString) || x.Description.Contains(searchString)).ToList();
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
          sortedTasks = tasksRet.Tasks.OrderBy(x => x.DateCreated).ToList();
          break;

      }

      var mappedTasks = MapToViewModel(sortedTasks);

      return View("Index", mappedTasks.ToPagedList(pageNumber, pageSize));
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

    private void PrepareViewModel()
    {
      var usernames = userServiceClient.GetAllUsers();
      if (usernames != null && usernames.RecordCount > 0)
      {
        var userNamesList = usernames.Users.Select(x => x.UserName).ToList();
        ViewBag.Usernames = JsonConvert.SerializeObject(userNamesList);
      }
      var projects = projectServiceClient.GetAllProjects();

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
    #endregion
  }
}