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
    public class UtilityController : ApiController
    {
				internal static readonly ILog _log = log4net.LogManager.GetLogger(typeof
        (UtilityController));
        private readonly IUtilityService _utilityService;

				public UtilityController()
				{
					_utilityService = new UtilityService();
				}

				[HttpGet]
				[ActionName("GetIssueTypes")]
				public HttpResponseMessage GetIssueTypes()
				{
					_log.DebugFormat("GetIssueTypes invoked...");
					var issueTypes = _utilityService.GetIssueTypes();

					if (issueTypes != null)
					{
						var issueTypesEntities = issueTypes as List<IssueTypeEntity> ?? issueTypes.ToList();
						if (issueTypesEntities.Any())
						{
							_log.DebugFormat("GetIssueTypes finished with : {0}", issueTypesEntities.ToString());
							return Request.CreateResponse(HttpStatusCode.OK, issueTypesEntities);
						}

					}
					return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Issue types not found");
				}

				[HttpGet]
				[ActionName("GetAllStatuses")]
				public HttpResponseMessage GetAllStatuses()
				{
					_log.DebugFormat("GetAllStatuses invoked...");
					var statuses = _utilityService.GetAllStatuses();

					if (statuses != null)
					{
						var statusEntities = statuses as List<StatusEntity> ?? statuses.ToList();
						if (statusEntities.Any())
						{
							_log.DebugFormat("GetAllStatuses finished with : {0}", statusEntities.ToString());
							return Request.CreateResponse(HttpStatusCode.OK, statusEntities);
						}

					}
					return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Statuses not found");
				}

				[HttpGet]
				[ActionName("GetPriorities")]
				public HttpResponseMessage GetPriorities()
				{
					_log.DebugFormat("GetPriorities invoked...");
					var priorities = _utilityService.GetPriorities();

					if (priorities != null)
					{
						var priorEnts = priorities as List<PriorityEntity> ?? priorities.ToList();
						if (priorEnts.Any())
						{
							_log.DebugFormat("GetPriorities finished with : {0}", priorEnts.ToString());
							return Request.CreateResponse(HttpStatusCode.OK, priorEnts);
						}

					}
					return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Priorities not found");
				}
        
    }
}
