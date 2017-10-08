using AutoMapper;
using BusinessServices.Interfaces;
using DataModel.UnitOfWork;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;
using Task = DataModel.Task;
using DataModel;
using BusinessServices.Interfaces.Responses;
using BussinesService.Interfaces.Responses.Task;

namespace BusinessServices
{
  public class TaskService : ITaskService
  {
    private readonly UnitOfWork _unitOfWork;
    internal static readonly ILog _log = log4net.LogManager.GetLogger(typeof(TaskService));

    #region constructors
    public TaskService()
    {
      _unitOfWork = new UnitOfWork();
    }
    #endregion

    #region ITaskService members

    public BasicReturn CreateTask(TaskEntity task)
    {
      BasicReturn ret = new BasicReturn();

      try
      {
        var config = new MapperConfiguration(cfg =>
        {
          cfg.CreateMap<TaskEntity, Task>();
        });
        IMapper mapper = config.CreateMapper();
        var taskToInsert = mapper.Map<Task>(task);

        _unitOfWork.TaskRepository.Insert(taskToInsert);
        _unitOfWork.Save();
        ret.StatusCode = "OK";
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error creating task : {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
        ret.StatusCode = "Error";
      }
      return ret;
    }

    public BasicReturn UpdateTask(TaskEntity task)
    {
      BasicReturn ret = new BasicReturn();

      try
      {
        var config = new MapperConfiguration(cfg =>
        {
          cfg.CreateMap<TaskEntity, Task>();
        });
        IMapper mapper = config.CreateMapper();
        var taskToUpdate = mapper.Map<Task>(task);

        _unitOfWork.TaskRepository.Update(taskToUpdate);
        _unitOfWork.Save();
        ret.StatusCode = "OK";
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error updating task : {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
      }

      return ret;

    }

    public SearchTasksReturn GetAllTasks()
    {
      _log.DebugFormat("GetAllTasks invoked");
      SearchTasksReturn ret = new SearchTasksReturn();
      try
      {
        var tasks = _unitOfWork.TaskRepository.Get(orderBy: q => q.OrderBy(d => d.DateCreated));
        if (tasks.Any())
        {
          var config = new MapperConfiguration(cfg =>
          {
            cfg.CreateMap<Task, TaskEntity>();
          });

          IMapper mapper = config.CreateMapper();
          var taskskMapped = mapper.Map<List<Task>, List<TaskEntity>>(tasks.ToList());
          _log.DebugFormat("GetAllTasks finished with : {0}", tasks.ToString());
          ret.Tasks = taskskMapped;
          ret.RecordCount = taskskMapped.Count;
          ret.StatusCode = "OK";
        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching users... {0}", ex.Message);
      }


      return ret;
    }

    public BaseTaskReturn GetTaskById(long TaskId)
    {
      BaseTaskReturn ret = new BaseTaskReturn();
      var task = _unitOfWork.TaskRepository.GetByID(TaskId);
      if (task != null)
      {
        var taskModel = Mapper.Map<DataModel.Task, TaskEntity>(task);
        ret.Task = taskModel;
        ret.StatusCode = "OK";
      }
      return ret;
    }

    public TasksDetailsReturn GetAllTasksDetails()
    {
      _log.DebugFormat("GetAllTasksDetails invoked");
      TasksDetailsReturn ret = new TasksDetailsReturn();
      try
      {
        var tasks = _unitOfWork.GetAllTasksDetails();
        var taskSorted = tasks.OrderByDescending(x => x.DateCreated).ToList();
        List<TaskEntityExtended> tasksWithDetails = new List<TaskEntityExtended>();

        var tasksAudit = _unitOfWork.GetTasksAudit();

        ret.TasksAudit = MapTasksAudit(tasksAudit);

        tasksWithDetails = MapTasks(taskSorted);
        ret.Tasks = tasksWithDetails;
        ret.RecordCount = tasksWithDetails.Count;
        ret.StatusCode = "OK";

      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error fetching tasks... {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
      }
      return ret;

    }

    public TaskEntityExtendedReturn GetTaskByIdCustom(long TaskId)
    {
      _log.DebugFormat("GetTaskByIdCustom invoked for task with Id : {0}", TaskId);
      TaskEntityExtendedReturn ret = new TaskEntityExtendedReturn();
      try
      {
        var task = _unitOfWork.GetTaskById(TaskId);
        TaskEntityExtendedReturn taskEntity = MapToTaskEntity(task.FirstOrDefault());
        ret = taskEntity;

      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error fetching task... {0}", ex.Message);
      }
      return ret;
    }

    public SearchTasksReturn GetTasksForUser(long userId)
    {
      _log.DebugFormat("GetTasksForUser invoked");
      SearchTasksReturn ret = new SearchTasksReturn();
      try
      {
        var tasks = _unitOfWork.TaskRepository.GetAll().Where(x => x.Asignee == userId).ToList();
        tasks = tasks.OrderByDescending(x => x.DateCreated).ToList();
        if (tasks.Any())
        {
          var config = new MapperConfiguration(cfg =>
          {
            cfg.CreateMap<Task, TaskEntity>();
          });

          IMapper mapper = config.CreateMapper();
          var taskskMapped = mapper.Map<List<Task>, List<TaskEntity>>(tasks.ToList());
          _log.DebugFormat("GetTasksForUser finished with : {0}", tasks.ToString());
          ret.Tasks = taskskMapped;
          ret.RecordCount = taskskMapped.Count;
          ret.StatusCode = "OK";
        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error fetching tasks... {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
      }
      return ret;
    }

    public SearchTasksReturn GetTasksOnProject(long projectId)
    {
      _log.DebugFormat("GetTasksOnProject invoked");
      SearchTasksReturn ret = new SearchTasksReturn();
      try
      {
        var tasks = _unitOfWork.TaskRepository.Get(x => x.ProjectId == projectId).ToList();
        if (tasks.Any())
        {
          var config = new MapperConfiguration(cfg =>
          {
            cfg.CreateMap<Task, TaskEntity>();
          });

          IMapper mapper = config.CreateMapper();
          var taskskMapped = mapper.Map<List<Task>, List<TaskEntity>>(tasks.ToList());
          _log.DebugFormat("GetTasksOnProject finished with : {0}", tasks.ToString());
          ret.Tasks = taskskMapped;
          ret.RecordCount = taskskMapped.Count;
          ret.StatusCode = "OK";
        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching tasks... {0}", ex.Message);
      }


      return ret;
    }

    #endregion

    #region mappers
    private List<TaskEntityExtended> MapTasks(List<TaskDetailsResult> tasks)
    {
      List<TaskEntityExtended> tasksDetails = new List<TaskEntityExtended>();

      foreach (var task in tasks)
      {
        tasksDetails.Add(new TaskEntityExtended()
        {
          Id = task.TaskId,
          Title = task.Title,
          Asignee = task.Asignee,
          Status = task.TaskStatus,
          Description = task.Description,
          DateCreated = (DateTime)task.DateCreated,
          DueDate = (DateTime)task.DueDate,
          Reporter = task.Reporter,
          Project = task.Project,
          Priority = task.Priority,
          IssueType = task.IssueType

        });
      }

      return tasksDetails;
    }
    private List<TaskAudit> MapTasksAudit(List<GetTasksAssigneStatusHistory_Result> tasksAudit)
    {
      List<TaskAudit> audit = new List<TaskAudit>();

      foreach (var item in tasksAudit)
      {
        audit.Add(new TaskAudit()
        {
          TaskId = item.TaskId,
          AsigneeBefore = item.AsigneeBefore,
          AsigneeAfter = item.AsigneeAfter,
          AsigneeChangedOnDate = item.AssigneChangedOn,
          AsigneeChangedBy = item.AssigneChangedBy,
          StatusBefore = item.StatusBefore,
          StatusAfter = item.StatusAfter,
          StatusChangedOnDate = (DateTime)item.StatusChangedOn,
          StatusChangedBy = item.StatusChangeBy
        });
      }

      return audit;
    }
    private TaskEntityExtendedReturn MapToTaskEntity(GetTaskResult getTaskResult)
    {
      TaskEntityExtendedReturn taskEntity = new TaskEntityExtendedReturn();
      taskEntity.Asignee = getTaskResult.Asignee;
      taskEntity.AsigneeId = getTaskResult.AsigneeId;
      taskEntity.Reporter = getTaskResult.Reporter;
      taskEntity.ReporterId = getTaskResult.ReporterId;
      taskEntity.DateCreated = (DateTime)getTaskResult.DateCreated;
      taskEntity.DueDate = (DateTime)getTaskResult.DueDate;
      taskEntity.Description = getTaskResult.Description;
      taskEntity.IssueType = _unitOfWork.IssueTypeRepositorsy.GetByID(getTaskResult.IssueType).Name;
      taskEntity.IssueTypeId = getTaskResult.IssueType;
      taskEntity.Status = _unitOfWork.StatusRepository.GetByID(getTaskResult.Status).Name;
      taskEntity.StatusId = getTaskResult.Status;
      taskEntity.Priority = _unitOfWork.PriorityRepository.GetByID((long)getTaskResult.Priority).Name;
      taskEntity.PriorityId = (int)getTaskResult.Priority;
      taskEntity.Id = getTaskResult.TaskId;
      taskEntity.Title = getTaskResult.Title;
      taskEntity.ProjectId = getTaskResult.ProjectId;
      taskEntity.Project = _unitOfWork.ProjectRepository.GetByID(getTaskResult.ProjectId).Name;

      return taskEntity;
    }
    #endregion

  }
}
