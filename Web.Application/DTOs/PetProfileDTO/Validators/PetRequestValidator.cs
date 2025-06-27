using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.PetProfileDTO.Validators
{
    public class PetRequestValidator:AbstractValidator<PetRequest>  
    {
        public PetRequestValidator()
        {
            RuleFor(x => x.Name)
             .NotEmpty()
             .WithMessage("Pet name is required.")
             .Length(1,50)
             .WithMessage("Pet name must not be null or exceed 50 characters.");

            RuleFor(x => x.Breed)
                .NotEmpty()
                .WithMessage("Breed is required.");

            RuleFor(x => x.Age)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Age cannot be negative or Zero.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => g == "Male" || g == "Female")
                .WithMessage("Gender must be either 'Male' or 'Female'.");

                RuleFor(x => x.Photo)
                    .NotNull()
                    .WithMessage("Photo is required.");


            RuleFor(x => x.Photo)
                .Must(file =>
                {
                    if (file == null)
                        return true;
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    return allowedExtensions.Contains(extension);
                })
                .WithMessage("Only .jpg, .jpeg, and .png file types are allowed.");

            RuleFor(x => x.Photo)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Photo size must not exceed 5MB.");





        }

    }
}
