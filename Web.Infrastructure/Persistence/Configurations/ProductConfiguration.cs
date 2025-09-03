using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;

namespace Web.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : BaseConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Price)
                .IsRequired();

            // الحل: تحديد اسم الـ navigation property بشكل صحيح
            builder.HasOne(x => x.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(x => x.CategoryId)
                .HasConstraintName("FK_Product_Category"); // اختياري

            builder.HasOne(p => p.ProductStats)
                .WithOne(s => s.Product)
                .HasForeignKey<ProductStats>(s => s.ProductId);

            builder.HasIndex(x => x.Price);
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.Deleted);
            builder.HasIndex(x => x.StockQuantity);
        
    }
    }

}