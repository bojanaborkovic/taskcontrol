using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TaskControlDTOs;

namespace TaskControl.Controllers
{
    public class ProjectController : Controller
    {
				private System.Net.Http.HttpClient client;
				string url = "http://localhost/TaskControlAPI/projects/all";

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

						var Projects = JsonConvert.DeserializeObject<List<ProjectEntity>>(responseData);

						return View(Projects);
					}
					return View("Error");
				}
    }
}