using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace DataModel.UnitOfWork
{
  public class UnitOfWork : IDisposable
  {

    #region private members
    private TaskControlEntities context = new TaskControlEntities();
    private GenericRepository<AspNetUser> _userRepository;
    private GenericRepository<Task> _taskRepostiory;
    private GenericRepository<Project> _projectRepository;
    private GenericRepository<AspNetRole> _roleRepository;
    private GenericRepository<AspNetUserRole> _usersInRoleRepository;
    private GenericRepository<IssueType> _issueTypeRepository;
    private GenericRepository<Status> _statusRepository;
    private GenericRepository<Priority> _priorityRepository;
    internal static readonly ILog _log = LogManager.GetLogger(typeof(UnitOfWork));
    private bool disposed = false;
    #endregion

    #region repositories
    public GenericRepository<AspNetUser> UserRepository
    {
      get
      {
        if (this._userRepository == null)
        {
          this._userRepository = new GenericRepository<AspNetUser>(context);
        }
        return _userRepository;
      }
    }

    public GenericRepository<Task> TaskRepository
    {
      get
      {
        if (this._taskRepostiory == null)
        {
          this._taskRepostiory = new GenericRepository<Task>(context);
        }
        return _taskRepostiory;
      }
    }

    public GenericRepository<Project> ProjectRepository
    {
      get
      {
        if (this._projectRepository == null)
        {
          this._projectRepository = new GenericRepository<Project>(context);
        }
        return _projectRepository;
      }
    }

    public GenericRepository<AspNetRole> RoleRepository
    {
      get
      {
        if (this._roleRepository == null)
        {
          this._roleRepository = new GenericRepository<AspNetRole>(context);
        }
        return _roleRepository;
      }
    }

    public GenericRepository<AspNetUserRole> UserInRoleRepository
    {
      get
      {
        if (this._usersInRoleRepository == null)
        {
          this._usersInRoleRepository = new GenericRepository<AspNetUserRole>(context);
        }
        return _usersInRoleRepository;
      }
    }

    public GenericRepository<IssueType> IssueTypeRepositorsy
    {
      get
      {
        if (this._issueTypeRepository == null)
        {
          this._issueTypeRepository = new GenericRepository<IssueType>(context);
        }
        return _issueTypeRepository;
      }
    }

    public GenericRepository<Status> StatusRepository
    {
      get
      {
        if (this._statusRepository == null)
        {
          this._statusRepository = new GenericRepository<Status>(context);
        }
        return _statusRepository;
      }
    }

    public GenericRepository<Priority> PriorityRepository
    {
      get
      {
        if (this._priorityRepository == null)
        {
          this._priorityRepository = new GenericRepository<Priority>(context);
        }
        return _priorityRepository;
      }
    }
    #endregion

    #region public member methods

    public void Save()
    {
      try
      {
        context.SaveChanges();
      }
      catch (DbEntityValidationException dbE)
      {
        _log.ErrorFormat("Error ocurred during saving changes in the database: {0} ", dbE.Message);
        var outputLines = new List<string>();
        foreach (var eve in dbE.EntityValidationErrors)
        {
          outputLines.Add(string.Format(
              "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
              eve.Entry.Entity.GetType().Name, eve.Entry.State));
          foreach (var ve in eve.ValidationErrors)
          {
            outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
          }

          _log.ErrorFormat("Errors: {0}", outputLines);
        }
      }
    }
    #endregion


    #region stored procedures

    public List<SearchUsersResult> SearchUsers()
    {
      using (var context = new TaskControlEntities())
      {
        var users = context.SearchUsers();

        return users.ToList();
      }
    }

    public List<ProjectResult> GetProjectsWithOwner()
    {
      using (var context = new TaskControlEntities())
      {
        var projects = context.GetAllProjectsWithOwner();

        return projects.ToList();
      }
    }


    public List<TaskDetailsResult> GetAllTasksDetails()
    {
      using (var context = new TaskControlEntities())
      {
        var tasks = context.GetAllTasksDetails();

        return tasks.ToList();
      }
    }

    public List<GetTaskResult> GetTaskById(long taskId)
    {
      using (var context = new TaskControlEntities())
      {
        var task = context.GetTaskById(taskId).ToList();

        return task;
      }
    }

    public List<GetProjectStatistics_Result> GetProjectStatistics(long projectId)
    {
      using (var context = new TaskControlEntities())
      {
        var task = context.GetProjectStatistics(projectId).ToList();

        return task;
      }
    }

    public List<GetTasksAssigneStatusHistory_Result> GetTasksAudit()
    {
      using (var context = new TaskControlEntities())
      {
        var tasks = context.GetTasksAssigneStatusHistory().ToList();

        return tasks;
      }
    }

    #endregion

    #region IDisposable implementation

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          _log.DebugFormat("UnitOfWork is being disposed");
          context.Dispose();
        }
      }
      this.disposed = true;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this); // instruction to garbage collector 
    }

    #endregion
  }
}
