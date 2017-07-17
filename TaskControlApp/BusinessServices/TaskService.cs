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

namespace BusinessServices
{
	public class TaskService : ITaskService
	{
		private readonly UnitOfWork _unitOfWork;
		internal static readonly ILog _log = log4net.LogManager.GetLogger(typeof(TaskService));

		public TaskService()
		{
			_unitOfWork = new UnitOfWork();
		}

		long ITaskService.CreateTask(TaskEntity task)
		{
			throw new NotImplementedException();
		}

		public List<TaskEntity> GetAllTasks()
		{
			_log.DebugFormat("GetAllTasks invoked");
			try
			{
				var tasks = _unitOfWork.TaskRepository.Get(orderBy: q => q.OrderBy(d => d.DateCreated));
				if (tasks.Any())
				{
					var config = new MapperConfiguration(cfg => {
						cfg.CreateMap<Task, TaskEntity>();
					});

					IMapper mapper = config.CreateMapper();
					var usersMapped = mapper.Map<List<Task>, List<TaskEntity>>(tasks.ToList());
					_log.DebugFormat("GetAllTasks finished with : {0}", tasks.ToString());
					return usersMapped;
				}
			}
			catch (Exception ex)
			{
				_log.ErrorFormat("Error during fetching users... {0}", ex.Message);
			}


			return null;
		}

		TaskEntity ITaskService.GetTaskById(long TaskId)
		{
			var task = _unitOfWork.TaskRepository.GetByID(TaskId);
			if (task != null)
			{
				var taskModel = Mapper.Map<DataModel.Task, TaskEntity>(task);
				return taskModel;
			}
			return null;
		}

		public List<TaskEntityExtended> GetAllTasksDetails()
		{
			_log.DebugFormat("GetAllTasksDetails invoked");
			try
			{
				var tasks = _unitOfWork.GetAllTasksDetails();
				List<TaskEntityExtended> tasksWithDetails = new List<TaskEntityExtended>();
				tasksWithDetails = MapTasks(tasks);
				return tasksWithDetails;

			}
			catch (Exception ex)
			{
				_log.ErrorFormat("Error fetching tasks... {0}", ex.Message);
			}
			return null;

		}

		private List<TaskEntityExtended> MapTasks(List<TaskDetailsResult> tasks)
		{
			List<TaskEntityExtended> tasksDetails = new List<TaskEntityExtended>();

			foreach(var task in tasks)
			{
				tasksDetails.Add(new TaskEntityExtended()
				{
					TaskId = task.TaskId,
					Title = task.Title,
					Asignee = task.Asignee,
					Status = task.TaskStatus,
					Description = task.Description,
					DateCreated = task.DateCreated,
					DueDate = task.DueDate,
					Reporter = task.Reporter,
					Project = task.Project,
					Priority = task.Priority,
					IssueType = task.IssueType

				});
			}

			return tasksDetails;
		}
	}
}
