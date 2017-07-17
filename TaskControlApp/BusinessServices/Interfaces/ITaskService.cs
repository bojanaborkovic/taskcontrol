using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;

namespace BusinessServices.Interfaces
{
	public interface ITaskService
	{
		TaskEntity GetTaskById(long TaskId);
		List<TaskEntity> GetAllTasks();
		long CreateTask(TaskEntity task);
		List<TaskEntityExtended> GetAllTasksDetails();
	}
}
