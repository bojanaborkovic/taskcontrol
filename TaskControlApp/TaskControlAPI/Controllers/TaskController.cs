using BusinessServices;
using BusinessServices.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskControlDTOs;

namespace TaskControlAPI.Controllers
{
	public class TaskController : ApiController
	{
		internal static readonly ILog _log = log4net.LogManager.GetLogger(typeof
		(TaskController));
		private readonly ITaskService _taskService;

		public TaskController()
		{
			_taskService = new TaskService();
		}

		[HttpGet]
		[ActionName("GetAllTasks")]
		public HttpResponseMessage GetAllTasks()
		{
			_log.DebugFormat("GetAllTasks invoked...");

			try
			{
				var tasks = _taskService.GetAllTasks();
				if (tasks != null)
				{
					var taskEntities = tasks as List<TaskEntity> ?? tasks.ToList();
					if (taskEntities.Any())
					{
						_log.DebugFormat("GetAllTasks finished with : {0}", taskEntities.ToString());
						return Request.CreateResponse(HttpStatusCode.OK, taskEntities);
					}

				}
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Tasks not found");
			}
			catch (Exception ex)
			{
				_log.ErrorFormat("Error during fetching tasks... {0}", ex.Message);
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
			}
		}


		[HttpGet]
		[ActionName("GetAllTasksDetails")]
		public HttpResponseMessage GetAllTasksDetails()
		{
			_log.DebugFormat("GetAllTasksDetails invoked...");

			try
			{
				var tasks = _taskService.GetAllTasksDetails();
				if (tasks != null)
				{
					var taskEntities = tasks as List<TaskEntityExtended> ?? tasks.ToList();
					if (taskEntities.Any())
					{
						_log.DebugFormat("GetAllTasksDetails finished with : {0}", taskEntities.ToString());
						return Request.CreateResponse(HttpStatusCode.OK, taskEntities);
					}

				}
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Tasks not found");
			}
			catch (Exception ex)
			{
				_log.ErrorFormat("Error during fetching tasks... {0}", ex.Message);
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
			}
		}


	}
}
