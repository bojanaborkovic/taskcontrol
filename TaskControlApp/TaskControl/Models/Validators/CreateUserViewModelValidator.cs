using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskControl.Models.Validators
{
  public class CreateUserViewModelValidator : AbstractValidator<CreateUserViewModel>
  {
    public CreateUserViewModelValidator()
    {
      RuleFor(m => m.UserName).NotEmpty().Length(0, 100);
      RuleFor(m => m.Email).NotEmpty().WithMessage("Email is required!").EmailAddress().WithMessage("Please provide a valid email address!");
      RuleFor(m => m.FirstName).NotEmpty().WithMessage("First Name is required!");
      RuleFor(m => m.LastName).NotEmpty().WithMessage("Last Name is required!");
      RuleFor(m => m.Password).NotEmpty().WithMessage("Password is required!");
    }
  }
}