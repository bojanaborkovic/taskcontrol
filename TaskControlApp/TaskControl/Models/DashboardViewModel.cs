using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskControl.Models
{
  public class DashboardViewModel
  {
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    public List<DashboardTaskViewModel> TaskViewModel { get; set; }
  }


  public class DashboardTaskViewModel
  {
    public long Id { get; set; }
    public string Title { get; set; }

    public string Status { get; set; }

    public string Asignee { get; set; }

    public DateTime Start { get; set; }
    public DateTime End { get; set; }
  }
}