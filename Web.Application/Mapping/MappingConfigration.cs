

using Mapster;
using PetCare.Api.Entities;
using Web.Application.DTOs.PetProfileDTO;

namespace Web.Application.Mapping
{
    public class MappingConfigration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Pet, PetResponse>()
                 .Map(dest => dest.Age, src =>
        DateTime.Today.Year - src.BirthDay.Year - (DateTime.Today.DayOfYear < src.BirthDay.DayOfYear ? 1 : 0));


        }
    }
}
