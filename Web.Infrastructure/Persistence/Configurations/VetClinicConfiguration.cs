using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Api.Entities;
using Web.Infrastructure.Persistence.Configurations;

namespace Web.Infrastructure.Persistence.Configurations
{
    public class VetClinicConfiguration : BaseConfiguration<VetClinic>
    {
        public override void Configure(EntityTypeBuilder<VetClinic> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Phone)
                .HasMaxLength(20);

            builder.Property(x => x.PricePerNight)
                .HasColumnType("decimal(18,2)");

            builder.HasMany(x => x.Reviews)
                .WithOne(r => r.VetClinic)
                .HasForeignKey(r => r.VetClinicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.vetSchedules)
                .WithOne(s => s.vetClinic)
                .HasForeignKey(s => s.VetClinicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.appointments)
                .WithOne(a => a.VetClinic)
                .HasForeignKey(a => a.VetClinicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.PricePerNight);
        }
    }
}