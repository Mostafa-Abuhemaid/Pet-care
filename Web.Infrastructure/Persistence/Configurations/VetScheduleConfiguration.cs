using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Domain.Entites;
using Web.Infrastructure.Persistence.Configurations;

namespace Web.Infrastructure.Persistence.Configurations
{
    public class VetScheduleConfiguration : BaseConfiguration<VetSchedule>
    {
        public override void Configure(EntityTypeBuilder<VetSchedule> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.DayOfWeek)
                .IsRequired();

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.EndTime)
                .IsRequired();

            builder.HasOne(s => s.vetClinic)
                .WithMany(c => c.vetSchedules)
                .HasForeignKey(s => s.VetClinicId);
        }
    }
}
