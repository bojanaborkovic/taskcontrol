using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaskControlDTOs
{
  [DataContract]
  public class TaskEntity
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DataMember]
    public long Id { get; set; }
    [DataMember]
    public string Title { get; set; }
    [DataMember]
    public int IssueType { get; set; }
    [DataMember]
    public long Asignee { get; set; }
    [DataMember]
    public long Reporter { get; set; }
    [DataMember]
    public int Status { get; set; }
    [DataMember]
    public string Description { get; set; }
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
    public string AsigneeUsername { get; set; }

    [DataMember]
    public string StatusName { get; set; }

    [DataMember]
    public string IssueTypeName { get; set; }

    [DataMember]
    public string ReporterName { get; set; }

    [DataMember]
    public string PriorityName { get; set; }

    [DataMember]
    public string DateCreatedString
    {
      get { return this.DateCreated.ToString("yyyy-MM-dd HH:mm:ss"); }
      set { this.DateCreated = DateTime.Parse(value); }
    }
    public DateTime DateCreated { get; set; }

    [DataMember]
    public int Priority { get; set; }

    [DataMember]
    public long? CreatedBy { get; set; }
  }

  public class UpdateTaskStatus
  {
    [DataMember]
    public int StatusId { get; set; }
    
    [DataMember]
    public long TaskId { get; set; }
  }
}
