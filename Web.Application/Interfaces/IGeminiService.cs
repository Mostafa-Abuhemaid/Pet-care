using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Enums;

namespace Web.Application.Interfaces
{
    public interface IGeminiService
    {
        Task<string> GeneratePetAdvice(PetType petType, string breed, string petName);
    }
}
