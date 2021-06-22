﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;
using Core.Entities.Concrete;


namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public class EGonulluContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(@"Server=85.96.191.240;Database=ugur_db; User Id=ugur;Password=Qazwsx112_; Integrated Security=False;");
            //optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=EGonulluDb; Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Volunteer>()
                .HasKey(x => x.VolunteerId);

            modelBuilder.Entity<Advertisement>()
                .HasKey(x => x.AdvertisementId);

            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<OperationClaim>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<UserOperationClaim>()
                .HasKey(x => new { x.UserId, x.OperationClaimId });

            modelBuilder.Entity<Category>()
                .HasKey(x => x.CategoryId);

            modelBuilder.Entity<GlobalPurpose>()
                .HasKey(x => x.PurposeId);

            modelBuilder.Entity<Volunteer>()
                .HasOne(x => x.Company)
                .WithMany(x => x.Volunteers)
                .HasForeignKey(x=>x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AdvertisementCategory>()
            .HasKey(x => new { x.CategoryId, x.AdvertisementId });
            modelBuilder.Entity<AdvertisementCategory>()
                .HasOne(x => x.Advertisement)
                .WithMany(m => m.AdvertisementCategorys)
                .HasForeignKey(x => x.AdvertisementId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<AdvertisementCategory>()
                .HasOne(x => x.Category)
                .WithMany(m => m.AdvertisementCategorys)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<AdvertisementPurpose>()
                .HasOne(x => x.Advertisement)
                .WithMany(m => m.AdvertisementPurposes)
                .HasForeignKey(x => x.AdvertisementId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<AdvertisementPurpose>()
            .HasKey(x => new { x.PurposeId, x.AdvertisementId });
            modelBuilder.Entity<AdvertisementPurpose>()
                .HasOne(x => x.Purpose)
                .WithMany(m => m.AdvertisementPurposes)
                .HasForeignKey(x => x.PurposeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserOperationClaim>()
                .HasOne(x => x.OperationClaim)
                .WithMany(m => m.UserOperationClaims)
                .HasForeignKey(x => x.OperationClaimId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<UserOperationClaim>()
             .HasOne(x => x.User)
             .WithMany(m => m.UserOperationClaims)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserOperationClaim>()
                .HasOne(x => x.OperationClaim)
                .WithMany(m => m.UserOperationClaims)
                .HasForeignKey(x => x.OperationClaimId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AdvertisementVolunteer>()
                .HasKey(x => new { x.VolunteerId, x.AdvertisementId });

            modelBuilder.Entity<AdvertisementVolunteer>()
                .HasOne(x => x.Advertisement)
                .WithMany(m => m.AdvertisementVolunteers)
                .HasForeignKey(x => x.AdvertisementId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AdvertisementVolunteer>()
                .HasOne(x => x.Volunteer)
                .WithMany(m => m.AdvertisementVolunteers)
                .HasForeignKey(x => x.VolunteerId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<OrganisationVolunteer>()
                .HasKey(x => new { x.VolunteerId, x.OrganisationId });

            modelBuilder.Entity<OrganisationVolunteer>()
                .HasOne(x => x.Organisation)
                .WithMany(m => m.OrganisationVolunteers)
                .HasForeignKey(x => x.OrganisationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrganisationVolunteer>()
                .HasOne(x => x.Volunteer)
                .WithMany(m => m.OrganisationVolunteer)
                .HasForeignKey(x => x.VolunteerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VolunteerAdvertisementComplated>()
                .HasOne(x => x.AdvertisementVolunteer)
                .WithMany()
                .HasPrincipalKey(x => x.Id);
        }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AdvertisementVolunteer> AdvertisementVolunteers { get; set; }
        public DbSet<City> Citys { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<CompanyDepartment> CompanyDepartments { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<OrganisationVolunteer> OrganisationVolunteers { get; set; }
        public DbSet<University> Universitys { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<VolunteerAdvertisementComplated> VolunteerAdvertisementComplateds { get; set; }

    }
}
