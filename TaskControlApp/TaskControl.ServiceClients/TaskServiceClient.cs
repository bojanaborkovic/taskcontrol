using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TaskControlDTOs;

namespace TaskControl.ServiceClients
{
  public class TaskServiceClient : BaseRestClient
  {
    public TaskServiceClient(string repoName)
    {
      endpoint = string.Empty;
      endpoint = string.Format("{0}{1}", System.Configuration.ConfigurationManager.AppSettings["TaskControlApiURL"], repoName);
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public string GetAllTasks()
    {
      string action = "/all";
      address = action;
      Method = HttpVerb.GET;
      var json = MakeRequest();
      return json;
    }

    public string GetAllTasksDetails()
    {
      string action = "/details";
      address = action;
      Method = HttpVerb.GET;
      var json = MakeRequest();
      return json;
    }

    public string CreateTask(TaskEntity task)
    {
      string action = "/create";
      address = action;
      Method = HttpVerb.POST;
      PostData = new JavaScriptSerializer().Serialize(task);
      var json = MakeRequest();
      return json;

    }

    public string GetTaskById(long taskId)
    {
      string action = "/getbyid";
      address = action;
      Method = HttpVerb.GET;
      var json = MakeRequest();
      return json;
    }

    public string GetTaskByIdCustom(long taskId)
    {
      string action = "/getbyidcustom";
      address = action;
      string paramts = string.Format("?taskId={0}", taskId);
      Method = HttpVerb.GET;
      var json = MakeRequest(paramts);
      return json;
    }

    public object UpdateTask(TaskEntity task)
    {
      string action = "/update";
      address = action;
      Method = HttpVerb.POST;
      PostData = new JavaScriptSerializer().Serialize(task);
      var json = MakeRequest();
      return json;
    }
  }
}
