using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Api.Entities;
using Web.Infrastructure.Persistence.Configurations;

namespace Web.Infrastructure.Persistence.Configurations
{
    public class VetReviewConfiguration : BaseConfiguration<VetReview>
    {
        public override void Configure(EntityTypeBuilder<VetReview> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Rating)
                .IsRequired();

            builder.Property(x => x.Comment)
                .HasMaxLength(1000);

            builder.HasOne(r => r.VetClinic)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.VetClinicId);

            builder.HasOne(r => r.AppUser)
                .WithMany()
                .HasForeignKey(r => r.AppUserId);
        }
    }
}
