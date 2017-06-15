using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskControl.Models
{
  public class UserInRoleViewModel
  {
   
    [Display(Name = "User Role")]
    public int SelectedRoleId { get; set; }
    public IEnumerable<SelectListItem> UserRoles { get; set; }

    [Display(Name = "User")]
    public int SelectedUserId { get; set; }
    public IEnumerable<SelectListItem> Users { get; set; }

  }
}