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
  public class TaskController : ApiController
  {
    internal static readonly ILog _log = log4net.LogManager.GetLogger(typeof
    (TaskController));
    private readonly ITaskService _taskService;

    public TaskController()
    {
      _taskService = new TaskService();
    }

    [HttpGet]
    [ActionName("GetAllTasks")]
    public HttpResponseMessage GetAllTasks()
    {
      _log.DebugFormat("GetAllTasks invoked...");

      try
      {
        var getTasks = _taskService.GetAllTasks();
        if (getTasks != null && getTasks.Tasks.Count > 0)
        {
          _log.DebugFormat("GetAllTasks finished with : {0}", getTasks.Tasks.ToString());
          return Request.CreateResponse(HttpStatusCode.OK, getTasks);
        }
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Tasks not found");
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching tasks... {0}", ex.Message);
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
      }
    }


    [HttpGet]
    [ActionName("GetAllTasksDetails")]
    public HttpResponseMessage GetAllTasksDetails()
    {
      _log.DebugFormat("GetAllTasksDetails invoked...");

      try
      {
        var taskskRet = _taskService.GetAllTasksDetails();
        if (taskskRet != null && taskskRet.RecordCount > 0)
        {
          _log.DebugFormat("GetAllTasksDetails finished with : {0}", taskskRet.Tasks.ToString());
          return Request.CreateResponse(HttpStatusCode.OK, taskskRet);
        }
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Tasks not found");
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching tasks... {0}", ex.Message);
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
      }
    }

    [HttpPost]
    [ActionName("CreateTask")]
    public HttpResponseMessage CreateTask(TaskEntity task)
    {
      _log.DebugFormat("CreateTask invoked with : {0} ...", task.ToString());

      try
      {
        var taskRet = _taskService.CreateTask(task);
        if (string.IsNullOrEmpty(taskRet.ErrorMessage) && taskRet.StatusCode == "OK")
        {
          _log.DebugFormat("CreateTask finished with : {0}", taskRet != null ? taskRet.StatusCode.ToString() : "OK");
          return Request.CreateResponse(HttpStatusCode.OK);

        }
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error creating task");
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during creating task... {0}", ex.Message);
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
      }
    }

    [HttpPost]
    [ActionName("UpdateTask")]
    public HttpResponseMessage UpdateTask(TaskEntity task)
    {
      _log.DebugFormat("UpdateTask invoked with : {0} ...", task.ToString());

      try
      {
        _taskService.UpdateTask(task);
        return Request.CreateResponse(HttpStatusCode.OK, task.Id);
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during updating task... {0}", ex.Message);
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
      }
    }

    [HttpGet]
    [ActionName("GetTaskById")]
    public HttpResponseMessage GetTaskById(long taskId)
    {
      _log.DebugFormat("GetTaskById invoked with : {0} ...", taskId.ToString());

      try
      {
        //long taskId = _taskService.CreateTask(task);
        var task = _taskService.GetTaskById(taskId);
        if (task != null)
        {
          _log.DebugFormat("GetTaskById finished with : {0}", task.ToString());
          return Request.CreateResponse(HttpStatusCode.OK, task);

        }
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error getting task");
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching task... {0}", ex.Message);
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
      }
    }

    [HttpGet]
    [ActionName("GetTasksForUser")]
    public HttpResponseMessage GetTasksForUser(long userId)
    {
      _log.DebugFormat("GetTasksForUser invoked with UserId: {0} ...", userId.ToString());

      try
      {
        //long taskId = _taskService.CreateTask(task);
        var task = _taskService.GetTasksForUser(userId);
        if (task != null)
        {
          _log.DebugFormat("GetTasksForUser finished with : {0}", task.ToString());
          return Request.CreateResponse(HttpStatusCode.OK, task);

        }
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error getting task");
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching task... {0}", ex.Message);
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
      }
    }

    [HttpGet]
    [ActionName("GetTaskByIdCustom")]
    public HttpResponseMessage GetTaskByIdCustom([FromUri]long taskId)
    {
      _log.DebugFormat("GetTaskByIdCustom invoked with : {0} ...", taskId.ToString());

      try
      {
        //long taskId = _taskService.CreateTask(task);
        var task = _taskService.GetTaskByIdCustom(taskId);
        if (task != null)
        {
          _log.DebugFormat("GetTaskByIdCustom finished with : {0}", task.ToString());
          return Request.CreateResponse(HttpStatusCode.OK, task);

        }
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error getting task");
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching task... {0}", ex.Message);
        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
      }
    }


  }
}
