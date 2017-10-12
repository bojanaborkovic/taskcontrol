using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace TaskControl.Models
{
  public class DashboardViewModel
  {
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    public List<ProjectViewModel> OwnersProjects { get; set; }

    public List<DashboardTaskViewModel> TaskViewModel { get; set; }

    public List<TaskAuditViewModel> TaskAuditViewModel { get; set; }

    public DashboardTaskViewModel TaskDetails { get; set; }
  }


  public class DashboardTaskViewModel
  {
    public long Id { get; set; }
    public string Title { get; set; }

    public string Status { get; set; }

    public string Asignee { get; set; }

    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    [ScriptIgnore]
    public string Color { get; set; }
  }

  public class TaskAuditViewModel
  {
    public long TaskId { get; set; }
    public string TaskTitle { get; set; }

    public long? AsigneeBefore { get; set; }

    public string AsigneeBeforeUsername { get; set; }

    public long? AsigneeAfter { get; set; }

    public string AsigneeAfterUsername { get; set; }

    public DateTime? AsigneeChangedOn { get; set; }

    public string AsigneeChangedBy { get; set; }

    public long? AsigneeChangedById { get; set; }

    public long? StatusChangedBy { get; set; }

    public long? StatusBefore { get; set; }

    public string StatusBeforeName { get; set; }

    public long? StatusAfter { get; set; }

    public string StatusAfterName { get; set; }

    public DateTime? StatusChangedOn { get; set; }

    public string StatusChangeBy { get; set; }


  }

  public class UpdateTaskStatusModel
  {
    public string TaskId { get; set; }

    public string StatusName { get; set; }
  }
}