using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;

namespace Web.Infrastructure.Persistence.Configurations
{
    public class BreedingRequestConfiguration : BaseConfiguration<BreedingRequest>
    {
        public override void Configure(EntityTypeBuilder<BreedingRequest> builder)
        {
            base.Configure(builder);

            // RequesterPet FK
            builder.HasOne(br => br.RequesterPet)
                .WithMany()
                .HasForeignKey(br => br.RequesterPetId)
                .IsRequired();

            // TargetPet FK
            builder.HasOne(br => br.TargetPet)
                .WithMany()
                .HasForeignKey(br => br.TargetPetId)
                .IsRequired();

            // Status (Enum)
            builder.Property(br => br.Status)
                .IsRequired();

            // RequestDate, StartDate, EndDate
            builder.Property(br => br.RequestDate)
                .IsRequired();

            builder.Property(br => br.StartDate)
                .IsRequired();

            builder.Property(br => br.EndDate)
                .IsRequired();

            // Message
            builder.Property(br => br.Message)
                .HasMaxLength(500);
        }
    }


}
