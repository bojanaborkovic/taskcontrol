using AutoMapper;
using BusinessServices.Interfaces;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;


namespace BusinessServices
{
	public class TaskService : ITaskService
	{
		private readonly UnitOfWork _unitOfWork;

		public TaskService()
		{
			_unitOfWork = new UnitOfWork();
		}

		long ITaskService.CreateTask(TaskEntity task)
		{
			throw new NotImplementedException();
		}

		IEnumerable<TaskEntity> ITaskService.GetAllTasks()
		{
			throw new NotImplementedException();
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
	}
}
