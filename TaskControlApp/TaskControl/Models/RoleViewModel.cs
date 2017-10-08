using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace TaskControl.Models
{
  public class RoleViewModel
  {
    public long? RoleId { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    public DateTime? DateCreated { get; set; }

    public string[] ProjectList { get; set; }

    [ScriptIgnore]
    public List<ProjectViewModel> ProjectsAccess { get; set; }
  }
}