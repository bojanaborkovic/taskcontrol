using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BussinesService.Interfaces.Responses.Task;
using TaskControl.Models;
using TaskControl.ServiceClients;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TaskControl.Controllers
{
  [Authorize(Roles = "Admin, IT Admin, User")]
  public class DashboardController : Controller
  {
    private TaskServiceClient taskServiceClient = new TaskServiceClient("tasks") { DoSerialize = true };

    // GET: Dashboard
    public ActionResult Index()
    {

      var ret = taskServiceClient.GetAllTasksDetails();
      DashboardViewModel model = new DashboardViewModel();
      model.TaskViewModel = new List<DashboardTaskViewModel>();
      model.TaskViewModel = MapTasksToDashboard(ret);

      ViewBag.TaskList = JsonConvert.SerializeObject(model.TaskViewModel, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      });
      return View("Index", model);
    }

    private List<DashboardTaskViewModel> MapTasksToDashboard(TasksDetailsReturn ret)
    {
      List<DashboardTaskViewModel> tasksDashboard = new List<DashboardTaskViewModel>();

      foreach(var task in ret.Tasks)
      {
        tasksDashboard.Add(new DashboardTaskViewModel()
        {
          Id = task.Id,
          Title = task.Title,
          Start = task.DateCreated,
          End = task.DueDate

        });
      }

      return tasksDashboard;
    }



    // GET: Dashboard/Details/5
    public ActionResult Details(int id)
    {
      return View();
    }

    // GET: Dashboard/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: Dashboard/Create
    [HttpPost]
    public ActionResult Create(FormCollection collection)
    {
      try
      {
        // TODO: Add insert logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    // GET: Dashboard/Edit/5
    public ActionResult Edit(int id)
    {
      return View();
    }

    // POST: Dashboard/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add update logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    // GET: Dashboard/Delete/5
    public ActionResult Delete(int id)
    {
      return View();
    }

    // POST: Dashboard/Delete/5
    [HttpPost]
    public ActionResult Delete(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add delete logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }
  }
}
