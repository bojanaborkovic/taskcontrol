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

    public BasicReturn UpdateTaskStatus(UpdateTaskStatus update)
    {
      BasicReturn ret = new BasicReturn();

      try
      {
        var taskToUpdate = _unitOfWork.TaskRepository.Get(x => x.Id == update.TaskId).FirstOrDefault();
        taskToUpdate.Status = update.StatusId;

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

    public TasksDetailsReturn GetAllTasksDetails(long userId)
    {
      _log.DebugFormat("GetAllTasksDetails invoked");
      TasksDetailsReturn ret = new TasksDetailsReturn();
      try
      {
        List<long> projectAcess = CheckProjectAccessForUser(userId);

        var tasks = _unitOfWork.GetAllTasksDetails().Where(t => projectAcess.Contains(t.ProjectId));
        var taskSorted = tasks.OrderByDescending(x => x.DateCreated).ToList();
        List<TaskEntityExtended> tasksWithDetails = new List<TaskEntityExtended>();

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

    public SearchTasksReturn GetTasksForUser(long userId, long? projectId)
    {
      _log.DebugFormat("GetTasksForUser invoked");
      SearchTasksReturn ret = new SearchTasksReturn();
      try
      {
        var tasks = _unitOfWork.TaskRepository.GetAll().Where(x => x.Asignee == userId).ToList();

        if (projectId != null)
        {
          tasks = tasks.Where(x => x.ProjectId == projectId).ToList();
        }

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
          ret.Tasks = MapAdditionalFields(taskskMapped);
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

    private List<TaskEntity> MapAdditionalFields(List<TaskEntity> taskskMapped)
    {
      List<TaskEntity> tasks = new List<TaskEntity>();
      if (taskskMapped != null && taskskMapped.Count > 0)
      {
        foreach (var item in taskskMapped)
        {
          tasks.Add(new TaskEntity()
          {
            Asignee = item.Asignee,
            AsigneeUsername = GetUsernamebyId(item.Asignee),
            Description = item.Description,
            Status = item.Status,
            StatusName = GetStatusNameById(item.Status),
            ProjectId = item.ProjectId,
            Title = item.Title,
            IssueType = item.IssueType,
            IssueTypeName = GetIssueTypeNameById(item.IssueType),
            DateCreated = item.DateCreated,
            DueDate = item.DueDate,
            Reporter = item.Reporter,
            ReporterName = GetUsernamebyId(item.Reporter),
            Priority = item.Priority,
            PriorityName = GetPriorityNameById(item.Priority),
            Id = item.Id
          });
        }
      }

      return tasks;
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

    public TaskAuditReturn GetTaskHistory(long? taskId)
    {
      _log.DebugFormat("GetTaskHistory invoked");
      TaskAuditReturn ret = new TaskAuditReturn();
      try
      {
        if (taskId == null)
        {
          // get all changes on all tasks
          var statusHistory = _unitOfWork.TaskStatusHistoryRepository.GetAll();

          var asigneeHistory = _unitOfWork.TaskAsigneeHistoryRepository.GetAll();

          ret = MergeHistoryLists(asigneeHistory, statusHistory);
        }
        else // get changes for specific task (both status and asignee changes)
        {
          var statusHistory = _unitOfWork.TaskStatusHistoryRepository.Get(x => x.TaskId == taskId).ToList();

          var asigneeHistory = _unitOfWork.TaskAsigneeHistoryRepository.Get(x => x.TaskId == taskId).ToList();

          ret = MergeHistoryLists(asigneeHistory, statusHistory);
        }
        if (ret.TasksAudit.Any())
        {

          _log.DebugFormat("GetTaskHistory finished with : {0}", ret.TasksAudit.ToString());
          ret.RecordCount = ret.RecordCount;
          ret.StatusCode = "OK";
        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error during fetching tasks... {0}", ex.Message);
      }


      return ret;
    }

    public TaskCommentsReturn GetTaskComments(long taskId)
    {
      _log.DebugFormat("GetTaskComments invoked for taskId : {0}", taskId);
      TaskCommentsReturn ret = new TaskCommentsReturn();
      try
      {
        List<DataModel.Comment> comments = new List<DataModel.Comment>();
        comments = _unitOfWork.CommentRepository.GetAll().Where(x => x.TaskId == taskId).ToList();

        if (comments != null && comments.Any())
        {
          comments = comments.OrderByDescending(x => x.DateCreated).ToList();
          TaskCommentsReturn commentsMapped = MapComments(comments);
          _log.DebugFormat("GetTaskComments finished with : {0}", commentsMapped.ToString());
          ret.TaskComments = commentsMapped.TaskComments;
          ret.RecordCount = commentsMapped.RecordCount;
          ret.StatusCode = "OK";
        }
      }
      catch (Exception ex)
      {
        _log.ErrorFormat("Error fetching comments... {0}", ex.Message);
        ret.ErrorMessage = ex.Message;
      }
      return ret;
    }

    private TaskCommentsReturn MapComments(List<DataModel.Comment> comments)
    {
      TaskCommentsReturn ret = new TaskCommentsReturn();
      ret.TaskComments = new List<BussinesService.Interfaces.Responses.Task.Comment>();
      foreach (var comment in comments)
      {
        ret.TaskComments.Add(new BussinesService.Interfaces.Responses.Task.Comment()
        {
          AuthorId = comment.Author,
          AuthorName = GetUsernamebyId(comment.Author),
          Content = comment.Text,
          TaskId = comment.TaskId,
          DateCreated = (DateTime)comment.DateCreated
        });
      }

      ret.RecordCount = ret.TaskComments.Count;
      return ret;
    }

    private TaskAuditReturn MergeHistoryLists(IEnumerable<TaskAsigneeHistory> asigneeHistory, IEnumerable<TaskStatusHistory> statusHistory)
    {
      TaskAuditReturn ret = new TaskAuditReturn();
      ret.TasksAudit = new List<TaskAudit>();

      if (statusHistory != null && statusHistory.Count() > 0)
      {
        foreach (var historyItem in statusHistory)
        {
          if (historyItem.StatusAfter != historyItem.StatusBefore)
          {
            ret.TasksAudit.Add(new TaskAudit()
            {
              TaskId = historyItem.TaskId,
              TaskTitle = GetTaskName(historyItem.TaskId),
              ChangeType = TaskChangeType.StatusChange,
              ChangedById = historyItem.ChangeBy,
              ChangedByUsername = GetUsernamebyId(historyItem.ChangeBy),
              ChangedFromId = historyItem.StatusBefore,
              ChangedFrom = GetStatusNameById(historyItem.StatusBefore),
              ChangedOnString = historyItem.ChangeDate != null ? historyItem.ChangeDate.ToString() : string.Empty,
              ChangedToId = historyItem.StatusAfter,
              ChangedTo = GetStatusNameById(historyItem.StatusAfter),
              ProjectId = GetTaskProjectByTaskId(historyItem.TaskId),
              ProjectName = GetTaskNameByTaskId(historyItem.TaskId)
            });
          }
        }
      }

      if (asigneeHistory != null && asigneeHistory.Count() > 0)
      {
        foreach (var historyItem in asigneeHistory)
        {
          if (historyItem.AsigneeAfter != historyItem.AsigneeBefore)
          {
            ret.TasksAudit.Add(new TaskAudit()
            {

              TaskId = historyItem.TaskId,
              TaskTitle = GetTaskName(historyItem.TaskId),
              ChangeType = TaskChangeType.AsigneeChange,
              ChangedById = historyItem.ChangeBy,
              ChangedByUsername = GetUsernamebyId(historyItem.ChangeBy),
              ChangedFromId = historyItem.AsigneeBefore,
              ChangedFrom = GetUsernamebyId(historyItem.AsigneeBefore),
              ChangedOnString = historyItem.ChangeDate != null ? historyItem.ChangeDate.ToString() : string.Empty,
              ChangedToId = historyItem.AsigneeAfter,
              ChangedTo = GetUsernamebyId(historyItem.AsigneeAfter),
              ProjectId = GetTaskProjectByTaskId(historyItem.TaskId),
              ProjectName = GetTaskNameByTaskId(historyItem.TaskId)

            });
          }
        }
      }

      ret.RecordCount = ret.TasksAudit.Count;
      ret.TasksAudit.Shuffle();

      ret.TasksAudit = ret.TasksAudit.OrderByDescending(x => x.ChangedOnString).ToList();

      return ret;

    }

    private string GetTaskNameByTaskId(long taskId)
    {
      string projectName = string.Empty;

      var task = _unitOfWork.TaskRepository.Get(x => x.Id == taskId).FirstOrDefault();
      if (task != null)
      {
        var project = _unitOfWork.ProjectRepository.GetByID(task.ProjectId);
        if (project != null)
        {
          return project.Name;
        }
      }

      return projectName;
    }

    private long GetTaskProjectByTaskId(long taskId)
    {
      var task = _unitOfWork.TaskRepository.Get(x => x.Id == taskId).FirstOrDefault();
      if (task != null)
      {
        return task.ProjectId;
      }
      else
      {
        return 0;
      }
    }

    private string GetTaskName(long taskId)
    {
      string taskName = string.Empty;
      var task = _unitOfWork.TaskRepository.Get(x => x.Id == taskId).FirstOrDefault();
      if (task != null)
      {
        taskName = task.Title;
      }

      return taskName;
    }

    private string GetStatusNameById(int? statusBefore)
    {
      string statusName = "N/A";
      if (statusBefore != null)
      {
        var status = _unitOfWork.StatusRepository.Get(x => x.Id == statusBefore).First();
        if (status != null)
        {
          statusName = status.Name;
        }
      }

      return statusName;

    }


    private string GetIssueTypeNameById(int? issueTypeId)
    {
      string issueType = "N/A";
      if (issueType != null)
      {
        var issueTypeName = _unitOfWork.IssueTypeRepositorsy.Get(x => x.Id == issueTypeId).First();
        if (issueTypeName != null)
        {
          issueType = issueTypeName.Name;
        }
      }

      return issueType;
    }


    private string GetUsernamebyId(long? changeBy)
    {
      string username = "N/A";
      if (changeBy != null)
      {
        var user = _unitOfWork.UserRepository.Get(x => x.Id == changeBy).First();
        if (user != null)
        {
          username = user.UserName;
        }
      }

      return username;
    }

    private string GetPriorityNameById(int? priorityId)
    {
      string priority = "N/A";
      if (priorityId != null)
      {
        var priorityItem = _unitOfWork.PriorityRepository.Get(x => x.Id == priorityId).First();
        if (priorityItem != null)
        {
          priority = priorityItem.Name;
        }
      }

      return priority;
    }

    private List<long> CheckProjectAccessForUser(long userId)
    {
      List<long> projectIDs = new List<long>();
      var role = _unitOfWork.UserInRoleRepository.Get().Where(x => x.UserId == userId).SingleOrDefault();
      if (role != null)
      {
        long roleId = role.RoleId;
        var projectAccess = _unitOfWork.RoleClaimsRepository.GetAll().Where(x => x.RoleId == roleId).ToList();
        if (projectAccess != null && projectAccess.Count > 0)
        {
          foreach (var access in projectAccess)
          {
            projectIDs.Add(access.ProjectId);
          }
        }
      }

      return projectIDs;
    }


    #endregion

    #region mappers
    private List<TaskEntityExtended> MapTasks(List<GetAllTasksDetails_Result> tasks)
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
          IssueType = task.IssueType,
          ProjectId = task.ProjectId

        });
      }

      return tasksDetails;
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
