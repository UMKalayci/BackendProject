﻿// <auto-generated />
using System;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(EGonulluContext))]
    [Migration("20210617080519_RoleIdentity")]
    partial class RoleIdentity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Entities.Concrete.OperationClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("OperationClaims");
                });

            modelBuilder.Entity("Core.Entities.Concrete.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<bool>("Status");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Core.Entities.Concrete.UserOperationClaim", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("OperationClaimId");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("UserId", "OperationClaimId");

                    b.HasIndex("OperationClaimId");

                    b.ToTable("UserOperationClaims");
                });

            modelBuilder.Entity("Entities.Concrete.Advertisement", b =>
                {
                    b.Property<int>("AdvertisementId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdvertisementDesc");

                    b.Property<string>("AdvertisementTitle");

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("InsertDate");

                    b.Property<bool>("IsOnline");

                    b.Property<int>("OrganisationId");

                    b.Property<DateTime>("StartDate");

                    b.Property<bool>("Status");

                    b.Property<DateTime>("UpdateDate");

                    b.HasKey("AdvertisementId");

                    b.HasIndex("OrganisationId");

                    b.ToTable("Advertisements");
                });

            modelBuilder.Entity("Entities.Concrete.AdvertisementCategory", b =>
                {
                    b.Property<int>("CategoryId");

                    b.Property<int>("AdvertisementId");

                    b.Property<DateTime>("InsertDate");

                    b.Property<bool>("Status");

                    b.Property<DateTime>("UpdateDate");

                    b.HasKey("CategoryId", "AdvertisementId");

                    b.HasIndex("AdvertisementId");

                    b.ToTable("AdvertisementCategory");
                });

            modelBuilder.Entity("Entities.Concrete.AdvertisementPurpose", b =>
                {
                    b.Property<int>("PurposeId");

                    b.Property<int>("AdvertisementId");

                    b.Property<DateTime>("InsertDate");

                    b.Property<bool>("Status");

                    b.Property<DateTime>("UpdateDate");

                    b.HasKey("PurposeId", "AdvertisementId");

                    b.HasIndex("AdvertisementId");

                    b.ToTable("AdvertisementPurpose");
                });

            modelBuilder.Entity("Entities.Concrete.AdvertisementVolunteer", b =>
                {
                    b.Property<int>("VolunteerId");

                    b.Property<int>("AdvertisementId");

                    b.Property<DateTime>("InsertDate");

                    b.Property<bool>("Status");

                    b.Property<DateTime>("UpdateDate");

                    b.HasKey("VolunteerId", "AdvertisementId");

                    b.HasIndex("AdvertisementId");

                    b.ToTable("AdvertisementVolunteers");
                });

            modelBuilder.Entity("Entities.Concrete.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName");

                    b.Property<DateTime>("InsertDate");

                    b.Property<bool>("Status");

                    b.Property<DateTime>("UpdateDate");

                    b.HasKey("CategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Entities.Concrete.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CityName");

                    b.Property<DateTime>("InsertDate");

                    b.Property<bool>("Status");

                    b.Property<DateTime>("UpdateDate");

                    b.HasKey("CityId");

                    b.ToTable("Citys");
                });

            modelBuilder.Entity("Entities.Concrete.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityId");

                    b.Property<string>("CompanyName");

                    b.Property<string>("Desc");

                    b.Property<DateTime>("FoundationOfYear");

                    b.Property<byte[]>("Image");

                    b.Property<DateTime>("InsertDate");

                    b.Property<string>("Phone");

                    b.Property<bool>("Status");

                    b.Property<DateTime>("UpdateDate");

                    b.Property<int>("UserId");

                    b.HasKey("CompanyId");

                    b.HasIndex("UserId");

                    b.ToTable("Companys");
                });

            modelBuilder.Entity("Entities.Concrete.CompanyDepartment", b =>
                {
                    b.Property<int>("CompanyDepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CompanyDepartmentName");

                    b.Property<int>("CompanyId");

                    b.Property<DateTime>("InsertDate");

                    b.Property<bool>("Status");

                    b.Property<DateTime>("UpdateDate");

                    b.HasKey("CompanyDepartmentId");

                    b.HasIndex("CompanyId");

                    b.ToTable("CompanyDepartments");
                });

            modelBuilder.Entity("Entities.Concrete.GlobalPurpose", b =>
                {
                    b.Property<int>("PurposeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("InsertDate");

                    b.Property<string>("PurposeName");

                    b.Property<bool>("Status");

                    b.Property<DateTime>("UpdateDate");

                    b.HasKey("PurposeId");

                    b.ToTable("GlobalPurpose");
                });

            modelBuilder.Entity("Entities.Concrete.Organisation", b =>
                {
                    b.Property<int>("OrganisationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityId");

                    b.Property<string>("Desc");

                    b.Property<byte[]>("FinanceDocument");

                    b.Property<DateTime>("FoundationOfYear");

                    b.Property<byte[]>("Image");

                    b.Property<DateTime>("InsertDate");

                    b.Property<bool>("IsMemberAcikAcik");

                    b.Property<string>("OrganisationName");

                    b.Property<string>("Phone");

                    b.Property<bool>("Status");

                    b.Property<DateTime>("UpdateDate");

                    b.Property<int>("UserId");

                    b.HasKey("OrganisationId");

                    b.HasIndex("CityId");

                    b.HasIndex("UserId");

                    b.ToTable("Organisations");
                });

            modelBuilder.Entity("Entities.Concrete.OrganisationVolunteer", b =>
                {
                    b.Property<int>("VolunteerId");

                    b.Property<int>("OrganisationId");

                    b.Property<DateTime>("InsertDate");

                    b.Property<bool>("Status");

                    b.Property<DateTime>("UpdateDate");

                    b.HasKey("VolunteerId", "OrganisationId");

                    b.HasIndex("OrganisationId");

                    b.ToTable("OrganisationVolunteers");
                });

            modelBuilder.Entity("Entities.Concrete.University", b =>
                {
                    b.Property<int>("UniversityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityId");

                    b.Property<string>("Desc");

                    b.Property<DateTime>("FoundationOfYear");

                    b.Property<byte[]>("Image");

                    b.Property<DateTime>("InsertDate");

                    b.Property<string>("Phone");

                    b.Property<bool>("Status");

                    b.Property<string>("UniversityName");

                    b.Property<DateTime>("UpdateDate");

                    b.Property<int?>("UserId");

                    b.HasKey("UniversityId");

                    b.HasIndex("UserId");

                    b.ToTable("Universitys");
                });

            modelBuilder.Entity("Entities.Concrete.Volunteer", b =>
                {
                    b.Property<int>("VolunteerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BirthDate");

                    b.Property<int>("CityId");

                    b.Property<int?>("CompanyDepartmentId");

                    b.Property<int?>("CompanyId");

                    b.Property<int>("Gender");

                    b.Property<string>("HighSchool");

                    b.Property<DateTime>("InsertDate");

                    b.Property<string>("Phone");

                    b.Property<bool>("Status");

                    b.Property<int?>("UniversityId");

                    b.Property<DateTime>("UpdateDate");

                    b.Property<int>("UserId");

                    b.HasKey("VolunteerId");

                    b.HasIndex("CityId");

                    b.HasIndex("CompanyDepartmentId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("UniversityId");

                    b.HasIndex("UserId");

                    b.ToTable("Volunteers");
                });

            modelBuilder.Entity("Core.Entities.Concrete.UserOperationClaim", b =>
                {
                    b.HasOne("Core.Entities.Concrete.OperationClaim", "OperationClaim")
                        .WithMany("UserOperationClaims")
                        .HasForeignKey("OperationClaimId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Core.Entities.Concrete.User", "User")
                        .WithMany("UserOperationClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Entities.Concrete.Advertisement", b =>
                {
                    b.HasOne("Entities.Concrete.Organisation", "Organisation")
                        .WithMany("Advertisements")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Entities.Concrete.AdvertisementCategory", b =>
                {
                    b.HasOne("Entities.Concrete.Advertisement", "Advertisement")
                        .WithMany("AdvertisementCategorys")
                        .HasForeignKey("AdvertisementId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Entities.Concrete.Category", "Category")
                        .WithMany("AdvertisementCategorys")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Entities.Concrete.AdvertisementPurpose", b =>
                {
                    b.HasOne("Entities.Concrete.Advertisement", "Advertisement")
                        .WithMany("AdvertisementPurposes")
                        .HasForeignKey("AdvertisementId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Entities.Concrete.GlobalPurpose", "Purpose")
                        .WithMany("AdvertisementPurposes")
                        .HasForeignKey("PurposeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Entities.Concrete.AdvertisementVolunteer", b =>
                {
                    b.HasOne("Entities.Concrete.Advertisement", "Advertisement")
                        .WithMany("AdvertisementVolunteers")
                        .HasForeignKey("AdvertisementId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Entities.Concrete.Volunteer", "Volunteer")
                        .WithMany("AdvertisementVolunteers")
                        .HasForeignKey("VolunteerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Entities.Concrete.Company", b =>
                {
                    b.HasOne("Core.Entities.Concrete.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Entities.Concrete.CompanyDepartment", b =>
                {
                    b.HasOne("Entities.Concrete.Company", "Company")
                        .WithMany("CompanyDepartments")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Entities.Concrete.Organisation", b =>
                {
                    b.HasOne("Entities.Concrete.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Core.Entities.Concrete.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Entities.Concrete.OrganisationVolunteer", b =>
                {
                    b.HasOne("Entities.Concrete.Organisation", "Organisation")
                        .WithMany("OrganisationVolunteers")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Entities.Concrete.Volunteer", "Volunteer")
                        .WithMany("OrganisationVolunteer")
                        .HasForeignKey("VolunteerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Entities.Concrete.University", b =>
                {
                    b.HasOne("Core.Entities.Concrete.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Entities.Concrete.Volunteer", b =>
                {
                    b.HasOne("Entities.Concrete.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Entities.Concrete.CompanyDepartment", "CompanyDepartment")
                        .WithMany()
                        .HasForeignKey("CompanyDepartmentId");

                    b.HasOne("Entities.Concrete.Company", "Company")
                        .WithMany("Volunteers")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Entities.Concrete.University", "University")
                        .WithMany()
                        .HasForeignKey("UniversityId");

                    b.HasOne("Core.Entities.Concrete.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
