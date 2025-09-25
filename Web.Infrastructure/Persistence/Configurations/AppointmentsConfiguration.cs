using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Domain.Entites;
using Web.Infrastructure.Persistence.Configurations;

namespace Web.Infrastructure.Persistence.Configurations
{
    public class AppointmentsConfiguration : BaseConfiguration<Appointments>
    {
        public override void Configure(EntityTypeBuilder<Appointments> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Date)
                .IsRequired();

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.EndTime)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(a => a.VetClinic)
                .WithMany(c => c.appointments)
                .HasForeignKey(a => a.VetClinicId);

            builder.HasOne(a => a.AppUser)
                .WithMany()
                .HasForeignKey(a => a.AppUserId);
        }
    }
}
