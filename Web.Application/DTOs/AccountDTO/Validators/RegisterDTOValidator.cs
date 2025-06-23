using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Enums;

namespace Web.Application.DTOs.AccountDTO.Validators
{
    public class RegisterDTOValidator:AbstractValidator<RegisterDTO>  
    {
        public RegisterDTOValidator()
        {
            RuleFor(x => x.Email)
              .NotEmpty()
              .EmailAddress();



            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(PasswordRegexPatterns.Password);

            RuleFor(x => x.ConfirmPassword)
       .NotEmpty().WithMessage("Confirm Password is required.")
       .Equal(x => x.Password).WithMessage("Passwords do not match.");

            RuleFor(x => x.Name)
              .NotEmpty();

        }
    }
}
