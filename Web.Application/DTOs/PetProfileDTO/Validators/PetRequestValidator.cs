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

            RuleFor(x => x.Gender)
        .NotEmpty().WithMessage("Gender is required.")
        .Must(g => string.Equals(g, "Male", StringComparison.OrdinalIgnoreCase)
                || string.Equals(g, "Female", StringComparison.OrdinalIgnoreCase))
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

            RuleFor(x => x.Color)
    .NotEmpty().WithMessage("Color is required.")
    .MaximumLength(10).WithMessage("Color must not exceed 10 characters.");

            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0).WithMessage("Weight must be greater than or equal to 0.")
                .LessThanOrEqualTo(500).WithMessage("Weight must not exceed 500 kg.");

            RuleFor(x => x.BirthDay)
       .NotEmpty().WithMessage("BirthDay is required.")
       .Must(birthDay => birthDay != default(DateOnly)).WithMessage("BirthDay is required.")
       .Must(birthDay => birthDay <= DateOnly.FromDateTime(DateTime.Today))
       .WithMessage("BirthDay cannot be in the future.")
       .Must(birthDay => birthDay > DateOnly.FromDateTime(DateTime.Today.AddYears(-30)))
       .WithMessage("BirthDay is too old, please enter a valid age.");

            RuleFor(x => x.MedicalConditions)
                .MaximumLength(1000).WithMessage("Medical Conditions must not exceed 1000 characters.");

            RuleFor(x => x.breedingRequestStatus)
            .NotEmpty()
            .WithMessage("BreedingStatus is required please choise Intact or Neutered.")
        .Must(g => string.Equals(g, "Intact", StringComparison.OrdinalIgnoreCase)
                || string.Equals(g, "Neutered", StringComparison.OrdinalIgnoreCase))
        .WithMessage("BreedingStatus must be either 'Neutered' or 'Neutered'.");


        }

    }
}
