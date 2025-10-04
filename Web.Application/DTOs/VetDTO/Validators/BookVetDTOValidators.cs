using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.VetDTO.Validators
{
    public class BookVetDTOValidator : AbstractValidator<BookVetDTO>
    { 
        public BookVetDTOValidator()
        {
            RuleFor(x => x.PetId)
                .GreaterThan(0)
                .WithMessage("PetId must be greater than 0.");


            RuleFor(x => x.Date)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Date must be today or in the future.");
            
            RuleFor(x => x.Time)
                .NotEmpty()
                .WithMessage("Time is required.");
            
            RuleFor(x => x.ServiceIds)
                .NotEmpty()
                .WithMessage("At least one service must be selected."); } }
}
