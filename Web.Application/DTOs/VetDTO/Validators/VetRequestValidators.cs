using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.VetDTO.Validators
{
    public class VetRequestValidator : AbstractValidator<VetRequest>
    {
        public VetRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Vet name is required.")
                .Length(1, 100).WithMessage("Vet name must not be empty or exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .Matches(@"^\+?\d{7,15}$").WithMessage("Phone must be a valid number (7-15 digits).");

            RuleFor(x => x.Photo)
                .Must(file =>
                {
                    if (file == null) return true;
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    return allowedExtensions.Contains(extension);
                })
                .WithMessage("Only .jpg, .jpeg, and .png file types are allowed.");

            RuleFor(x => x.Photo)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Photo size must not exceed 5MB.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required.")
                .MaximumLength(50).WithMessage("Type must not exceed 50 characters.");

            RuleFor(x => x.Services)
                .NotNull().WithMessage("Services list is required.")
                .Must(s => s.Count > 0).WithMessage("At least one service must be provided.");

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address is required.");

            RuleFor(x => x.PricePerNight)
                .GreaterThan(0).WithMessage("Price per night must be greater than 0.");

            RuleFor(x => x.Experience)
                .GreaterThanOrEqualTo(0).WithMessage("Experience must be a positive number.")
                .LessThanOrEqualTo(80).WithMessage("Experience seems invalid.");

            RuleFor(x => x.CountOfPatients)
                .GreaterThanOrEqualTo(0).WithMessage("Count of patients must be positive.");

        }
    }
    }
