using BusinessServices.Interfaces.Responses;
using BussinesService.Interfaces.Responses.Task;
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
    BaseTaskReturn GetTaskById(long TaskId);
    TaskEntityExtendedReturn GetTaskByIdCustom(long TaskId);
    SearchTasksReturn GetAllTasks();
    BasicReturn CreateTask(TaskEntity task);
    TasksDetailsReturn GetAllTasksDetails();
    BasicReturn UpdateTask(TaskEntity task);
    SearchTasksReturn GetTasksForUser(long userId, long? projectId);
    SearchTasksReturn GetTasksOnProject(long projectId);

    BasicReturn UpdateTaskStatus(UpdateTaskStatus update);
    TaskAuditReturn GetTaskHistory(long? taskId);

  }
}
