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
  public class TaskAuditReturn : BasicReturn
  {

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

  [DataContract]
  public class TaskCommentsReturn : BasicReturn
  {

    [DataMember]
    public List<Comment> TaskComments { get; set; }

    [DataMember]
    public int RecordCount { get; set; }
  }


  [DataContract]
  public class Comment
  {
    [DataMember]
    public long AuthorId { get; set; }

    [DataMember]
    public string AuthorName { get; set; }

    public DateTime DateCreated { get; set; }

    [DataMember]
    public string DateCreatedString
    {
      get { return this.DateCreated.ToString("yyyy-MM-dd HH:mm:ss"); }
      set { this.DateCreated = DateTime.Parse(value); }
    }

    [DataMember]
    public string Content { get; set; }

    [DataMember]
    public long TaskId { get; set; }

    public override string ToString()
    {
      return String.Format("AuthorId={0}, DateCreated={1}, Content={2}, TaskId={3]", AuthorId, DateCreated, Content, TaskId);
    }
  }

}
