using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskControl.ServiceClients;
using TaskControlDTOs;

namespace TaskControl.ViewDataPreparers
{
	public class PriorityPreparer : ActionFilterAttribute, IDataPreparer<PriorityEntity>
	{
		private UtilityServiceClient utilityServiceClient = new UtilityServiceClient("priority");
		//public IssueTypePreparer()
		//{
		//	AddDefaultItem = false;
		//}
		public IList<PriorityEntity> GetDataItems()
		{
			var ret = utilityServiceClient.GetPriorities();
			List<PriorityEntity> statuses = JsonConvert.DeserializeObject<List<PriorityEntity>>(ret);
			return statuses;
		}

		//public bool AddDefaultItem { get; set; }

		public static string ViewDataKey { get { return "Priority"; } }

		public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
			var dataItems = GetDataItems();

			filterContext.Controller.ViewData[ViewDataKey] = dataItems;
		}


		
	}
}