using Mapster;
using PetCare.Api.Entities;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.DTOs.ProductDTO;
using Web.Application.DTOs.VetDTO;
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

            config.NewConfig<VetClinic, VetDetailsDto>()
                .Map(dest => dest.logoUrl,
                    src => $"{baseUrl}/Vet/{src.logoUrl}")
                 .Map(dest => dest.Location,
                      src => src.Address == null ? "N/A" : $"{src.Address.Country}/{src.Address.City}/{src.Address.Street}")
                 .Map(dis => dis.Schedules, src => src.vetSchedules)
                 .Map(dest => dest.AverageRating,
                      src => src.Reviews != null && src.Reviews.Any()
                    ? src.Reviews.Average(r => r.Rating)
                    : 0.0)
                 .Map(dest => dest.ReviewsCount,
                 src => src.Reviews != null ? src.Reviews.Count : 0);







        }
    }
}
