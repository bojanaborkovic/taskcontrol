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
				//private System.Net.Http.HttpClient client;
				//string url = "http://localhost/TaskControlAPI/projects/all";
				private UnitOfWork unitOfWork = new UnitOfWork();
				private ProjectServiceClient serviceClient = new ProjectServiceClient();

				public ProjectController()
				{

				}

				//public ProjectController(ProjectServiceClient serviceClient)
				//{
				//		this.serviceClient = serviceClient;
				//}


				// GET: Project
				[HttpGet]
				public ActionResult Index()
				{
					var responseData = serviceClient.GetAllProjects();

					var projects = JsonConvert.DeserializeObject<List<ProjectEntity>>(responseData);

					return View("Index", MapToProjectsViewModel(projects));
				}

		private List<ProjectViewModel> MapToProjectsViewModel(List<ProjectEntity> projects)
		{
			List<ProjectViewModel> viewMOdel = new List<ProjectViewModel>();
			foreach(var project in projects)
			{
				viewMOdel.Add(new ProjectViewModel() { Id = project.Id, Name = project.Name });
			}

			return viewMOdel;
		}
	}
}