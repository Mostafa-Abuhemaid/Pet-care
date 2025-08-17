using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Application.DTOs.DataSeederDTO;
using Web.Domain.Entites;
using Web.Infrastructure.Persistence.Data;
using Web.Infrastructure.Seeding.Helpers;

namespace Web.Infrastructure.Seeding
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedProductsFromJsonAsync(string filePath, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var json = await File.ReadAllTextAsync(filePath, cancellationToken);
            var productsDto = JsonSerializer.Deserialize<List<ProductJsonDto>>(json);

            if (productsDto == null || productsDto.Count == 0)
                return;

            foreach (var dto in productsDto)
            {
                // لو الكاتيجوري مش موجودة، ضيفها
                var category = await _context.categories
                    .FirstOrDefaultAsync(c => c.Name == dto.category, cancellationToken);

                if (category == null)
                {
                    category = new Category
                    {
                        Name = dto.category,
                        Description = $"{dto.category} category",
                        ImageUrl = string.Empty
                    };
                    _context.categories.Add(category);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                // لو المنتج موجود بنفس الاسم تجاهله
                var exists = await _context.Products
                    .AnyAsync(p => p.Name == dto.name, cancellationToken);

                if (exists) continue;

                var product = new Product
                {
                    Name = dto.name,
                    Description = dto.description,
                    ImageUrl = dto.image_url,
                    Price = PriceHelper.ParsePrice(dto.price),
                    StockQuantity = 100, // ممكن تخليها default
                    CategoryId = category.Id

                };

                _context.Products.Add(product);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
