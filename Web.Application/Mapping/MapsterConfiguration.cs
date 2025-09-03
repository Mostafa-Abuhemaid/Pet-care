using Mapster;
using PetCare.Api.Entities;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.DTOs.ProductDTO;
using Web.Domain.Entites;

namespace Web.Application.Mapping
{
    public static class MapsterConfiguration
    {
        public static void RegisterMappings(TypeAdapterConfig config, string baseUrl)
        {
            config.NewConfig<Pet, PetResponse>()
                .Map(dest => dest.PhotoUrl,
                    src => $"{baseUrl}/Pet/{src.PhotoUrl}")
                .Map(dis => dis.breedingRequestStatus, src => src.breedingRequestStatus)
                .Map(dis => dis.height, src => src.height)
                .TwoWays();
            //.Map(dest => dest.Age,
            //    src => DateTime.Today.Year - src.BirthDay.Year -
            //          (DateTime.Today.DayOfYear < src.BirthDay.DayOfYear ? 1 : 0));


            config.NewConfig<Product, ProductResponse>()
                 .Map(dis => dis.rate, src => src.rate)
                .TwoWays();


        }
    }
}
