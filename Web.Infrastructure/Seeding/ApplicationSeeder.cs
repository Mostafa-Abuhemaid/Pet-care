using Microsoft.EntityFrameworkCore;
using Web.Infrastructure.Persistence.Data;
using Web.Infrastructure.Seeding;

namespace Web.Application.Helpers
{
    public class ApplicationSeeder
    {
        private readonly DataSeeder _dataSeeder;
        private readonly AppDbContext _context;

        public ApplicationSeeder(DataSeeder dataSeeder, AppDbContext context)
        {
            _dataSeeder = dataSeeder;
            _context = context;
        }

        public async Task SeedAllAsync()
        {
            try
            {
                // ✅ فحص أكثر دقة - لو في منتجات عندها Rate و Size يبقى الـ seeding اتعمل
                var hasSeededData = await _context.Products
                    .Where(p => p.rate > 0 && !string.IsNullOrEmpty(p.Size))
                    .AnyAsync();

                if (hasSeededData)
                {
                    Console.WriteLine("✅ Data already seeded with Rate and Size. Skipping...");
                    return;
                }

                Console.WriteLine("🚀 Starting data seeding...");

                var files = new[]
                {
                    "D:\\courses\\back end.net\\API Pojectes\\wepScraping2\\pharmacy_products.json",
                    "D:\\courses\\back end.net\\API Pojectes\\wepScraping2\\individual_products_products.json",
                    "D:\\courses\\back end.net\\API Pojectes\\wepScraping2\\collars_leashes_harnesses_dogs_products.json",
                    "D:\\courses\\back end.net\\API Pojectes\\wepScraping2\\dry_food_dogs_products.json",
                    "D:\\courses\\back end.net\\API Pojectes\\wepScraping2\\dry_food_cats_products.json",
                    "D:\\courses\\back end.net\\API Pojectes\\wepScraping2\\toys_cats_products.json",
                    "D:\\courses\\back end.net\\API Pojectes\\wepScraping2\\wet_food_cats_products.json"
                };

                int processedFiles = 0;
                foreach (var file in files)
                {
                    if (File.Exists(file))
                    {
                        Console.WriteLine($"📂 Processing: {Path.GetFileName(file)}");
                        await _dataSeeder.SeedProductsFromJsonAsync(file);
                        processedFiles++;
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ File not found: {Path.GetFileName(file)}");
                    }
                }

                Console.WriteLine($"🎉 Seeding completed! Processed {processedFiles} files.");

                // ✅ إحصائيات نهائية
                var categoriesCount = await _context.categories.CountAsync();
                var productsCount = await _context.Products.CountAsync();
                Console.WriteLine($"📊 Total: {categoriesCount} categories, {productsCount} products");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Seeding failed: {ex.Message}");
                throw;
            }
        }
    }
}