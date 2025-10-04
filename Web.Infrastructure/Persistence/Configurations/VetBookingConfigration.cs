using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Infrastructure.Persistence.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Web.Domain.Entites;


    public class VetBookingServiceConfiguration : IEntityTypeConfiguration<VetBookingService>
    {
        public void Configure(EntityTypeBuilder<VetBookingService> builder)
        {
            builder.HasKey(vbs => new { vbs.VetBookingId, vbs.VetClinicServiceId });

            builder.HasOne(vbs => vbs.VetBooking)
                .WithMany(vb => vb.VetBookingServices)
                .HasForeignKey(vbs => vbs.VetBookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(vbs => vbs.VetClinicService)
                .WithMany(vcs => vcs.VetBookingServices)
                .HasForeignKey(vbs => vbs.VetClinicServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

public class VetClinicServiceConfiguration : BaseConfiguration<VetClinicService>
    {
        public override void Configure(EntityTypeBuilder<VetClinicService> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(x => x.VetClinic)
                .WithMany(v => v.Services)
                .HasForeignKey(x => x.VetClinicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Name);
        }
    }


}
