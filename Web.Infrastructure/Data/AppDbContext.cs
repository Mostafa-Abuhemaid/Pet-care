using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;

namespace Web.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }
        public DbSet<Pet_Cat> Pet_Cats { get; set; }
        public DbSet<Cat_Data> Cat_Data { get; set; }
        //public DbSet<PetOwner> PetOwners { get; set; }
        public DbSet<BreedingRequest> BreedingRequests { get; set; }
        public DbSet<VetClinic> VetClinics { get; set; }
        //public DbSet<EmergencyClinic> EmergencyClinics { get; set; }
        public DbSet<VetReview> VetReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure TPC inheritance
            modelBuilder.Entity<Pet>().UseTpcMappingStrategy();
            modelBuilder.Entity<Pet_Cat>().ToTable("Pet_Cats");
            base.OnModelCreating(modelBuilder); // Important for Identity
            modelBuilder.Entity<BreedingRequest>()
                .HasOne(br => br.RequesterPet)
                .WithMany()
                .HasForeignKey(br => br.RequesterPetId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<BreedingRequest>()
                .HasOne(br => br.TargetPet)
                .WithMany()
                .HasForeignKey(br => br.TargetPetId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
        }
    }
