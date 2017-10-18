using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TaskControlDTOs;

namespace TaskControl.Models
{
  public class ProjectViewModel
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public long OwnerId { get; set; }
    public string Owner { get; set; }

    [DataType(DataType.MultilineText)]
    public string Description { get; set; }

    public int InProgressCount { get; set; }

    public int CompletedCount { get; set; }

    public int ToDoCount { get; set; }

    public decimal TotalProgress { get; set; }

    public List<ProjectNoteViewModel> Notes { get; set; }

    public string CurrentComment { get; set; }
  }

  public enum Status
  {
    ToDo = 1,
    InProgress = 2,
    Done = 3
  }

  public class ProjectNoteViewModel
  {
   
    public string Note { get; set; }

    public long AuthorId { get; set; }

    public string AuthorName { get; set; }

    public DateTime CommentDate { get; set; }

    public string ProjectId { get; set; }
  }

  public class AddNewNote
  {

    public string Note { get; set; }

    public string ProjectId { get; set; }
  }
}