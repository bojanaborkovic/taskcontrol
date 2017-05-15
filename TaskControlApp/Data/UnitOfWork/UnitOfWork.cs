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
		private TaskControlEntities _context = new TaskControlEntities();
		private GenericRepository<AspNetUser> _userRepository;
		private GenericRepository<Task> _taskRepostiory;
		private GenericRepository<Project> _projectRepository;
		internal static readonly ILog _log = LogManager.GetLogger(typeof(UnitOfWork));
		private bool disposed = false;
		#endregion

		public UnitOfWork()
		{
			//_context = new TaskControlEntities();
		}


		public GenericRepository<AspNetUser> UserRepository
		{
			get
			{
				if(this._userRepository == null)
				{
					this._userRepository = new GenericRepository<AspNetUser>(_context);
				}
				return _userRepository;
			}
		}

		public GenericRepository<Task> TaskRepository
		{
			get
			{
				if(this._taskRepostiory == null)
				{
					this._taskRepostiory = new GenericRepository<Task>(_context);
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
					this._projectRepository = new GenericRepository<Project>(_context);
				}
				return _projectRepository;
			}
		}


		#region public member methods

		public void Save()
		{
			try
			{
				_context.SaveChanges();
			}
			catch(DbEntityValidationException dbE)
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

		#region IDisposable implementation

		protected virtual void Dispose(bool disposing)
		{
			if(!this.disposed)
			{
				if(disposing)
				{
					_log.DebugFormat("UnitOfWork is being disposed");
					_context.Dispose();
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
