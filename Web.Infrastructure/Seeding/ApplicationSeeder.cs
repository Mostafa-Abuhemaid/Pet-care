using Microsoft.EntityFrameworkCore;
using Web.Infrastructure.Persistence.Data;
using Web.Infrastructure.Seeding;

namespace Web.Application.Helpers
{
    public class ApplicationSeeder
    {
        public ApplicationSeeder(DataSeeder dataSeeder,AppDbContext context )
        {
            _dataSeeder = dataSeeder;
            _context = context;
        }
        private readonly DataSeeder _dataSeeder;
        private readonly AppDbContext _context;


        public async Task SeedAllAsync()
        {
            // لو فيه كاتيجوريز أو منتجات خلاص
            if (_context.categories.Any() || _context.Products.Any())
                return;

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


            foreach (var file in files)
                await _dataSeeder.SeedProductsFromJsonAsync(file);
        }
    }

}
