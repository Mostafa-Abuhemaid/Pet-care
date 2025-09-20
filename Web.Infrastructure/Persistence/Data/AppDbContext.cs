using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;

namespace Web.Infrastructure.Persistence.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}
        public DbSet<Pet_Cat> Pet_Cats { get; set; }
        public DbSet<Pet_Dog> Pet_Dogs { get; set; }

        public DbSet<Cat_Data> Cat_Data { get; set; }
        //public DbSet<PetOwner> PetOwners { get; set; }
        public DbSet<BreedingRequest> BreedingRequests { get; set; }
        public DbSet<VetClinic> VetClinics { get; set; }
        //public DbSet<EmergencyClinic> EmergencyClinics { get; set; }
        public DbSet<VetReview> VetReviews { get; set; }

        public DbSet<Category> categories { get; set; }
        public DbSet<Product> Products { get; set; }
      
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<PromoCode> promoCodes { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<Address> addresses { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Special_Offers> Special_Offers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure TPC inheritance
            modelBuilder.Entity<Pet>().UseTpcMappingStrategy();
            modelBuilder.Entity<Pet_Cat>().ToTable("Pet_Cats");
            modelBuilder.Entity<Pet_Dog>().ToTable("Pet_Dogs");

            base.OnModelCreating(modelBuilder); // Important for Identity

           

            modelBuilder.Entity<CartItem>().HasOne(i => i.Cart)
                 .WithMany(c => c.Items)
                 .HasForeignKey(i => i.CartId);

            modelBuilder.Entity<CartItem>().HasKey(ci => new { ci.CartId, ci.ProductId });

            modelBuilder.Entity<Favorite>().HasKey(ci => new { ci.ProductId, ci.UserId });

            #region explanation
            //   انا هنا فصلت كل الكونفيجريشن بتاع كل مودل ف كلاس لوحده للتنظيم وعملت هنا كول لكل الكونفيجريشن
            // IEntityTypeConfiguration دي عن طريق انه هيطبق كل الكلاسسز اللي بتورث من  
            #endregion
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            #region explanation2
            // Restrict بص ي هندسه (مصطفي و محمد )انا ضيفت الكود دا هنا علشان اغير سلوك المسح  كله يكون  
            //  علشان مدخلش ف مشكله زي اني امسح كاتيجوري مثلا تمسح كل المنتجات اللي تحتها  
            // Cascade على أكتر من مستوى ====>(CategoryId → Product → OrderItems)، وده يمسح آلاف الصفوف من غير ما تحس.
            //soft delete بيحظرك ويجبرك انك تمسح العلاقات اللي بين الجداول دي وبعض او انك تستخدم   Restrict لكن بقا سلوك 
            #endregion
            var cascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

      
        }
    }
}
