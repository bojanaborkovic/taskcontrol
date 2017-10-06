using BusinessServices.Interfaces.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TaskControlDTOs;

namespace BussinesService.Interfaces.Responses.Task
{
  [DataContract]
  public class BaseTaskReturn : BasicReturn
  {
    [DataMember]
    public TaskEntity Task { get; set; }
  }

  [DataContract]
  public class SearchTasksReturn : BasicReturn
  {
    [DataMember]
    public List<TaskEntity> Tasks { get; set; }

    [DataMember]
    public int RecordCount { get; set; }
  }


  [DataContract]
  public class TasksDetailsReturn : BasicReturn
  {
    [DataMember]
    public List<TaskEntityExtended> Tasks { get; set; }

    [DataMember]
    public List<TaskAudit> TasksAudit { get; set; }

    [DataMember]
    public int RecordCount { get; set; }
  }

  [DataContract]
  public class TaskEntityExtendedReturn : TaskEntityExtended
  {
    [DataMember]
    public string ErrorMessage { get; set; }

    [DataMember]
    public string StatusCode { get; set; }

  }

}
