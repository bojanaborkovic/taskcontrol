using BusinessServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TaskControlDTOs;
using BussinesService.Interfaces.Responses.Task;
using BusinessServices.Interfaces.Responses;
using System.Configuration;

namespace TaskControl.ServiceClients
{
  public class TaskServiceClient : BaseRestClient, ITaskService
  {
    public TaskServiceClient(string repoName)
    {
      endpoint = string.Empty;
      BaseUri = new Uri(string.Format("{0}{1}", ConfigurationManager.AppSettings["TaskControlApiURL"], repoName));
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public BaseTaskReturn GetTaskById(long TaskId)
    {
      return Get<BaseTaskReturn>(new Uri(string.Format("{0}/{1}", BaseUri.ToString(), "getbyid")));
    }

    public TaskEntityExtendedReturn GetTaskByIdCustom(long TaskId)
    {
      return Get<TaskEntityExtendedReturn>(new Uri(string.Format("{0}/{1}?taskId={2}", BaseUri.ToString(), "getbyidcustom", TaskId)));
    }

    public SearchTasksReturn GetAllTasks()
    {
      return Get<SearchTasksReturn>(new Uri(string.Format("{0}/{1}", BaseUri.ToString(), "all")));
    }

    public BasicReturn CreateTask(TaskEntity task)
    {
      return ExecutePost<BasicReturn>(string.Format("{0}/{1}", "tasks", "create"), task);
    }

    public TasksDetailsReturn GetAllTasksDetails()
    {
      return Get<TasksDetailsReturn>(new Uri(string.Format("{0}/{1}", BaseUri.ToString(), "details")));
    }

    public BasicReturn UpdateTask(TaskEntity task)
    {
      return ExecutePost<BasicReturn>(string.Format("{0}/{1}", "tasks", "update"), task);
    }

  
  }
}
