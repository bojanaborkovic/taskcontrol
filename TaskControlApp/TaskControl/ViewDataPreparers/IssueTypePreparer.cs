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
  public class IssueTypePreparer : ActionFilterAttribute, IDataPreparer<IssueTypeEntity>
  {
    private UtilityServiceClient utilityServiceClient = new UtilityServiceClient("issueTypes");
    //public IssueTypePreparer()
    //{
    //	AddDefaultItem = false;
    //}
    public IList<IssueTypeEntity> GetDataItems()
    {
      var ret = utilityServiceClient.GetAllIssueTypes();
      List<IssueTypeEntity> issueTypes = JsonConvert.DeserializeObject<List<IssueTypeEntity>>(ret);
      return issueTypes;
    }

    //public bool AddDefaultItem { get; set; }

    public static string ViewDataKey { get { return "IssueTypes"; } }

    public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
    {
      base.OnActionExecuting(filterContext);
      var dataItems = GetDataItems();

      filterContext.Controller.ViewData[ViewDataKey] = dataItems;
    }



  }
}