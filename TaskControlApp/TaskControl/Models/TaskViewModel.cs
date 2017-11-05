using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskControl.Models
{
  public class TaskViewModel
  {
    public long Id { get; set; }

    public string Title { get; set; }

    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    public string Asignee { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateCreated { get; set; }

    public int IssueType { get; set; }

    public string IssueTypeName { get; set; }

    public int Status { get; set; }

    public string StatusName { get; set; }

    public string Reporter { get; set; }

    public int Priority { get; set; }

    public string PriorityName { get; set; }

    public long Project { get; set; }

    public string ProjectName { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime DueDate { get; set; }

    public long? CreatedBy { get; set; }

    public List<Comment> TaskComments { get; set; }

    [DataType(DataType.MultilineText)]
    public string CurrentComment { get; set; }

  }

  public class Comment
  {
    public long AuthorId { get; set; }

    public string AuthorName { get; set; }

    public DateTime? DateCreated { get; set; }

    public string Content { get; set; }

    public long TaskId { get; set; }
  }
}