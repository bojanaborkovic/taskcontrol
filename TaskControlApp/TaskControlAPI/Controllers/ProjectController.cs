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
    public class ProjectController : ApiController
    {

				private readonly IProjectService _projectService;
				internal static readonly ILog _log = log4net.LogManager.GetLogger(typeof(ProjectController));
				public ProjectController()
				{
					_projectService = new ProjectService();
				}


				// GET: projects/all
				[HttpGet]
				[ActionName("GetAllProjects")]
				public HttpResponseMessage Get()
				{
					_log.DebugFormat("GetAllProjects invoked...");
					var projects = _projectService.GetAllProjects();
					if (projects != null)
					{
						var projectEntities = projects as List<ProjectEntity> ?? projects.ToList();
						if (projectEntities.Any())
						{
							_log.DebugFormat("GetAllProjects finished with : {0}", projectEntities.ToString());
							return Request.CreateResponse(HttpStatusCode.OK, projectEntities);
						}
					
					}
					return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Projects not found");
				}

        // GET: api/Project/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Project
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Project/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Project/5
        public void Delete(int id)
        {
        }
    }
}
