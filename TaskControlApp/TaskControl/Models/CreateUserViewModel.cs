using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TaskControl.Models.Validators;

namespace TaskControl.Models
{
  [Validator(typeof(CreateUserViewModelValidator))]
  public class CreateUserViewModel
  {

    public long UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; }
    public int? RoleId { get; set; }
  }
}