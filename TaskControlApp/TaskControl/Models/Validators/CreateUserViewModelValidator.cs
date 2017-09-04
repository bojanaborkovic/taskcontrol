using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
      RuleFor(m => m.Password).NotEmpty().WithMessage("Password is required!").Must(password => ValidatePassword(password)).WithMessage("Password must containt 8-15 characters, have both upper and lower case letters, and at least one digit!");
      RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match");
    }


    private bool ValidatePassword(string password)
    {
      const int MIN_LENGTH = 8;
      const int MAX_LENGTH = 15;

      if (password == null) throw new ArgumentNullException();

      bool meetsLengthRequirements = password.Length >= MIN_LENGTH && password.Length <= MAX_LENGTH;
      bool hasUpperCaseLetter = false;
      bool hasLowerCaseLetter = false;
      bool hasDecimalDigit = false;

      if (meetsLengthRequirements)
      {
        foreach (char c in password)
        {
          if (char.IsUpper(c)) hasUpperCaseLetter = true;
          else if (char.IsLower(c)) hasLowerCaseLetter = true;
          else if (char.IsDigit(c)) hasDecimalDigit = true;
        }
      }

      bool isValid = meetsLengthRequirements
                  && hasUpperCaseLetter
                  && hasLowerCaseLetter
                  && hasDecimalDigit
                  ;
      return isValid;
    }
  }
}