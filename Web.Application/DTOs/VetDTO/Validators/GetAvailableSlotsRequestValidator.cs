using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.DTOs.VetDTO.Validators
{
    public class GetAvailableSlotsRequestValidator : AbstractValidator<GetAvailableSlotsRequest>
    {
        public GetAvailableSlotsRequestValidator()
        { 
            RuleFor(x => x.Date)
                .Must(d => d >= DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Date cannot be in the past");
        }
    }

}
