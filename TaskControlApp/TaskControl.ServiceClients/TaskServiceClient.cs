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
      return Get<BaseTaskReturn>(new Uri(string.Format("{0}/{1}?taskId={2}", BaseUri.ToString(), "getbyid", TaskId)));
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

    public SearchTasksReturn GetTasksForUser(long userId, long? projectId)
    {
      string url = string.Format("{0}/{1}?userId={2}&projectId={3}", BaseUri.ToString(), "gettasksforuser", userId, projectId);
      return Get<SearchTasksReturn>(new Uri(url));
    }

    public SearchTasksReturn GetTasksOnProject(long projectId)
    {
      string url = string.Format("{0}/{1}?projectId={2}", BaseUri.ToString(), "gettasksonproject", projectId);
      return Get<SearchTasksReturn>(new Uri(url));
    }

    public BasicReturn UpdateTaskStatus(UpdateTaskStatus task)
    { 
      return ExecutePost<BasicReturn>(string.Format("{0}/{1}", "tasks", "updatetaskstatus"), task);
    }

    public TaskAuditReturn GetTaskHistory(long? taskId)
    {
      string url = string.Format("{0}/{1}?taskId={2}", BaseUri.ToString(), "gettaskhistory", taskId);
      return Get<TaskAuditReturn>(new Uri(url));
    }
  }
}
