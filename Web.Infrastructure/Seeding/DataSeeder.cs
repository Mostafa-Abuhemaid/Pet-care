using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Web.Application.DTOs.DataSeederDTO;
using Web.Domain.Entites;
using Web.Infrastructure.Persistence.Data;
using Web.Infrastructure.Seeding.Helpers;

namespace Web.Infrastructure.Seeding
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;
        private readonly Random _random;

        public DataSeeder(AppDbContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task SeedProductsFromJsonAsync(string filePath, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(filePath))
                return;

            var json = await File.ReadAllTextAsync(filePath, cancellationToken);
            var productsDto = JsonSerializer.Deserialize<List<ProductJsonDto>>(json);

            if (productsDto == null || productsDto.Count == 0)
                return;

            // ✅ اعمل query واحد بس للمنتجات الموجودة
            var existingProductNames = await _context.Products
                .Select(p => p.Name)
                .ToListAsync(cancellationToken);

            foreach (var dto in productsDto)
            {
                // ✅ تحقق من القائمة الموجودة (مش query جديد)
                if (existingProductNames.Contains(dto.name))
                    continue;

                // ابحث عن الكاتيجوري أو ضيفها
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

                var product = new Product
                {
                    Name = dto.name,
                    Description = dto.description,
                    ImageUrl = dto.image_url,
                    Price = PriceHelper.ParsePrice(dto.price),
                    StockQuantity = 100,
                    CategoryId = category.Id,
                    // ✅ إضافة Rate و Size
                    rate = GetRandomRate(),
                    Size = GetSizeByCategory(dto.category)
                };

                _context.Products.Add(product);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        // ✅ دالة لإنتاج Rate عشوائي
        private double GetRandomRate()
        {
            var rates = new double[] {3.3, 2.0, 2.5, 2.9, 3.0, 3.5, 3.8, 4.0, 4.5, 5.0 };
            return rates[_random.Next(rates.Length)];
        }

        // ✅ دالة لإنتاج Size حسب الكاتيجوري
        private string GetSizeByCategory(string categoryName)
        {
            var isEven = _random.Next(2) == 0; // عشوائي true/false

            return categoryName.ToLower() switch
            {
                var cat when cat.Contains("collar") || cat.Contains("harness") || cat.Contains("leash")
                    => isEven ? "38cm" : "50cm",

                var cat when cat.Contains("dry") && cat.Contains("dog")
                    => isEven ? "1.5kg" : "3kg",

                var cat when cat.Contains("dry") && cat.Contains("cat")
                    => isEven ? "400g" : "2kg",

                var cat when cat.Contains("toy") && cat.Contains("cat")
                    => isEven ? "1 piece" : "2 pieces",

                var cat when cat.Contains("wet") && cat.Contains("cat")
                    => isEven ? "200g" : "415g",

                var cat when cat.Contains("pharmacy") || cat.Contains("medicine")
                    => isEven ? "120ml" : "1 Tablet",

                // الكاتيجوريز الأخرى
                _ => isEven ? "1 unit" : "2 units"
            };
        }
    }
}