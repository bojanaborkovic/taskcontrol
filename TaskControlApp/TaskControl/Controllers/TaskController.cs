using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using TaskControl.Models;
using TaskControl.ServiceClients;
using TaskControl.ViewDataPreparers;
using TaskControlDTOs;

namespace TaskControl.Controllers
{
	public class TaskController : Controller
    {
				private TaskServiceClient taskServiceClient = new TaskServiceClient("tasks");

				// GET: Tasks
				public ActionResult Index()
        {
					var ret = taskServiceClient.GetAllTasksDetails();
					var tasks = JsonConvert.DeserializeObject<List<TaskEntityExtended>>(ret);
					List<TaskViewModel> viewModel = MapToViewModel(tasks);
					
          return View(viewModel);  
        }

				[IssueTypePreparer, StatusPreparer, PriorityPreparer]
				public ActionResult Create()
				{
					return View("New");
				}

		#region mappers

		private List<TaskViewModel> MapToViewModel(List<TaskEntityExtended> tasks)
				{
					List<TaskViewModel> viewmodel = new List<TaskViewModel>();
					foreach(var item in tasks)
					{
						viewmodel.Add(new TaskViewModel() { 
							TaskId = item.TaskId,
							Asignee = item.Asignee,
							Description = item.Description,
							Status = item.Status,
							Project = item.Project,
							Title = item.Title,
							IssueType = item.IssueType,
							DateCreated = item.DateCreated,
							DueDate = item.DueDate,
							Reporter = item.Reporter,
							Priority = item.Priority
						});
					}
					return viewmodel;
				}
		#endregion
	}
}