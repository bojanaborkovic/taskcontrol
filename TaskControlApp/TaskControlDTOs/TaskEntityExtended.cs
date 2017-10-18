using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaskControlDTOs
{
  [DataContract]
  public class TaskEntityExtended
  {
    [DataMember]
    public long Id { get; set; }
    [DataMember]
    public string Title { get; set; }
    [DataMember]
    public string IssueType { get; set; }
    [DataMember]
    public int IssueTypeId { get; set; }
    [DataMember]
    public string Asignee { get; set; }
    [DataMember]
    public long AsigneeId { get; set; }
    [DataMember]
    public string Reporter { get; set; }
    [DataMember]
    public long ReporterId { get; set; }
    [DataMember]
    public string Status { get; set; }
    [DataMember]
    public long StatusId { get; set; }
    [DataMember]
    public string Description { get; set; }
    [DataMember]
    public string Project { get; set; }
    [DataMember]
    public long ProjectId { get; set; }

    [DataMember]
    public string DueDateString
    {
      get { return this.DueDate.ToString("yyyy-MM-dd HH:mm:ss"); }
      set { this.DueDate = DateTime.Parse(value); }
    }
    public DateTime DueDate { get; set; }

    [DataMember]
    public string DateCreatedString
    {
      get { return this.DateCreated.ToString("yyyy-MM-dd HH:mm:ss"); }
      set { this.DateCreated = DateTime.Parse(value); }
    }
    public DateTime DateCreated { get; set; }

    [DataMember]
    public string Priority { get; set; }
    [DataMember]
    public int PriorityId { get; set; }
  }

  [DataContract]
  public class TaskAudit
  {
    [DataMember]
    public long TaskId { get; set; }

    [DataMember]
    public string TaskTitle { get; set; }

    [DataMember]
    public TaskChangeType ChangeType { get; set; }

    [DataMember]
    public string ChangedByUsername { get; set; }

    [DataMember]
    public long? ChangedById { get; set; }

    [DataMember]
    public string ChangedFrom { get; set; }

    [DataMember]
    public long? ChangedFromId { get; set; }

    [DataMember]
    public string ChangedTo { get; set; }

    [DataMember]
    public long? ChangedToId { get; set; }

    public DateTime ChangedOn { get; set; }

    [DataMember]
    public string ChangedOnString
    {
      get { return this.ChangedOn.ToString("yyyy-MM-dd HH:mm:ss"); }
      set { this.ChangedOn = DateTime.Parse(value); }
    }


  }

  
  [DataContract]
  public enum TaskChangeType
  {
    [EnumMember]
    StatusChange = 0,
    [EnumMember]
    AsigneeChange = 1
  }
}
