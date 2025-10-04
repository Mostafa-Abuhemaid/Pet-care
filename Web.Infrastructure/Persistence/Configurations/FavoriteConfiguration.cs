using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Domain.Entites;

namespace Web.Infrastructure.Persistence.Configurations
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {


    builder.HasKey(f => new { f.UserId, f.ProductId, f.PetId, f.VetClinicId });

         
            builder.HasOne(f => f.User)
                .WithMany() 
                .HasForeignKey(f => f.UserId);

            builder.HasOne(f => f.Pet)
                .WithMany() 
                .HasForeignKey(f => f.PetId);


            builder.HasOne(f => f.VetClinic)
                .WithMany()
                .HasForeignKey(f => f.VetClinicId);
        }
    }

}