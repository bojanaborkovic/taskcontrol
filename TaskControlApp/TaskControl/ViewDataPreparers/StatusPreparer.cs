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
  public class StatusPreparer : ActionFilterAttribute, IDataPreparer<StatusEntity>
  {
    private UtilityServiceClient utilityServiceClient = new UtilityServiceClient("status");
    //public IssueTypePreparer()
    //{
    //	AddDefaultItem = false;
    //}
    public IList<StatusEntity> GetDataItems()
    {
      var ret = utilityServiceClient.GetAllStatuses();
      List<StatusEntity> statuses = JsonConvert.DeserializeObject<List<StatusEntity>>(ret);
      return statuses;
    }

    //public bool AddDefaultItem { get; set; }

    public static string ViewDataKey { get { return "Statuses"; } }

    public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
    {
      base.OnActionExecuting(filterContext);
      var dataItems = GetDataItems();

      filterContext.Controller.ViewData[ViewDataKey] = dataItems;
    }



  }
}