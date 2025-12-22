using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;

namespace Web.Infrastructure.Persistence.Configurations
{
    public class BoardingConfiguration : BaseConfiguration<Boarding>
    {
        public override void Configure(EntityTypeBuilder<Boarding> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Phone)
                .HasMaxLength(20);

            builder.Property(x => x.PricePerNight)
                .HasColumnType("decimal(18,2)");


            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.PricePerNight);



        }
    }
}
