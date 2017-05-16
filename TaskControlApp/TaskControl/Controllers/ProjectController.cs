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
using TaskControlDTOs;

namespace TaskControl.Controllers
{
    public class ProjectController : Controller
    {
				private System.Net.Http.HttpClient client;
				string url = "http://localhost:64291/projects/all";
				private UnitOfWork unitOfWork = new UnitOfWork();

		public ProjectController()
				{
					client = new System.Net.Http.HttpClient();
					client.BaseAddress = new Uri(url);
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				}


				// GET: Project
				[HttpGet]
				public async Task<ActionResult> Index()
				{
					HttpResponseMessage responseMessage = await client.GetAsync(url);
					if (responseMessage.IsSuccessStatusCode)
					{
						var responseData = responseMessage.Content.ReadAsStringAsync().Result;

						var projects = JsonConvert.DeserializeObject<List<ProjectEntity>>(responseData);
						
						return View(MapToProjectsViewModel(projects));
					}
					return View("Error");
					//var projects = unitOfWork.ProjectRepository.Get(orderBy: q => q.OrderBy(d => d.Name));
					//return View(projects.ToList());
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