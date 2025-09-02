using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.PetProfileDTO;

namespace Web.Application.DTOs.CategoryDTO
{
    public class CategoryRequestValidators : AbstractValidator<SendCategoryDTO>
    {
        public CategoryRequestValidators()
        {
            RuleFor(x => x.Name)
             .NotEmpty()
             .WithMessage("category name is required.")
             .Length(3, 50)
             .WithMessage("category name must not be null or exceed 50 characters.");

        }
    }

}