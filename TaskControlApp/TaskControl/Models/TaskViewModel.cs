using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskControl.Models
{
  public class TaskViewModel
  {
    public long TaskId { get; set; }

    public string Title { get; set; }

    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    public string Asignee { get; set; }

    public DateTime? DateCreated { get; set; }

    public int IssueType { get; set; }

    public int Status { get; set; }

    public string Reporter { get; set; }

    public int Priority { get; set; }

    public long Project { get; set; }

    public string ProjectName { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

  }
}