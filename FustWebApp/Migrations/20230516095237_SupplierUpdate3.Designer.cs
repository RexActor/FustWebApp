﻿// <auto-generated />
using System;
using FustWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FustWebApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230516095237_SupplierUpdate3")]
    partial class SupplierUpdate3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.15");

            modelBuilder.Entity("FustWebApp.Models.Domain.Audit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AffectedColumns")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("NewValues")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OldValues")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PrimaryKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("FustWebApp.Models.Domain.Currency", b =>
                {
                    b.Property<int>("currencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("currencyAbrevation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("currencyExchangeRate")
                        .HasColumnType("REAL");

                    b.Property<string>("currencyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("currencyTargetName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("currencyTargetNameAbrevation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("currencyId");

                    b.ToTable("Currency");
                });

            modelBuilder.Entity("FustWebApp.Models.Domain.Fust", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExpectedQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FustAmountPerPallet")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FustName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FustTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("baseValue")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("FustTypeId");

                    b.ToTable("Fusts");
                });

            modelBuilder.Entity("FustWebApp.Models.Domain.FustType", b =>
                {
                    b.Property<int>("FustTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<string>("FustTypeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("FustTypeId");

                    b.ToTable("FustTypes");
                });

            modelBuilder.Entity("FustWebApp.Models.Domain.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("GroupComment")
                        .HasColumnType("TEXT");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("FustWebApp.Models.Domain.LoadFusts", b =>
                {
                    b.Property<int>("LoadFustId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ExpectedQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FustName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FustTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LoadsLoadId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReceivedQty")
                        .HasColumnType("INTEGER");

                    b.Property<int>("currencyId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("unitValue")
                        .HasColumnType("REAL");

                    b.HasKey("LoadFustId");

                    b.HasIndex("FustTypeId");

                    b.HasIndex("LoadsLoadId");

                    b.HasIndex("currencyId");

                    b.ToTable("LoadFusts");
                });

            modelBuilder.Entity("FustWebApp.Models.Domain.Loads", b =>
                {
                    b.Property<int>("LoadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoadComment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LoadDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoadGroup")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LoadOrigin")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LoadSupplier")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LoadTrailerNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LoadType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LoadUpdatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("PONumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReceivedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ReceivedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoadId");

                    b.ToTable("Loads");
                });

            modelBuilder.Entity("FustWebApp.Models.Domain.Origin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Origins");
                });

            modelBuilder.Entity("FustWebApp.Models.Domain.StockHolding", b =>
                {
                    b.Property<int>("StockHoldingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("StockHoldingFustItemsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StockHoldingQty")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("StockHoldingSupplierId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StockholdingDate")
                        .HasColumnType("TEXT");

                    b.HasKey("StockHoldingId");

                    b.HasIndex("StockHoldingFustItemsId");

                    b.HasIndex("StockHoldingSupplierId");

                    b.ToTable("StockHolding");
                });

            modelBuilder.Entity("FustWebApp.Models.Domain.Supplier", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FustTypeList")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SupplierAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SupplierGroup")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SupplierOrigin")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("FustWebApp.Models.Domain.Fust", b =>
                {
                    b.HasOne("FustWebApp.Models.Domain.FustType", "FustType")
                        .WithMany()
                        .HasForeignKey("FustTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FustType");
                });

            modelBuilder.Entity("FustWebApp.Models.Domain.LoadFusts", b =>
                {
                    b.HasOne("FustWebApp.Models.Domain.FustType", "FustType")
                        .WithMany()
                        .HasForeignKey("FustTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FustWebApp.Models.Domain.Loads", "Loads")
                        .WithMany("LoadFustItems")
                        .HasForeignKey("LoadsLoadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FustWebApp.Models.Domain.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("currencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("FustType");

                    b.Navigation("Loads");
                });

            modelBuilder.Entity("FustWebApp.Models.Domain.StockHolding", b =>
                {
                    b.HasOne("FustWebApp.Models.Domain.Fust", "StockHoldingFustItems")
                        .WithMany()
                        .HasForeignKey("StockHoldingFustItemsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FustWebApp.Models.Domain.Supplier", "StockHoldingSupplier")
                        .WithMany()
                        .HasForeignKey("StockHoldingSupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StockHoldingFustItems");

                    b.Navigation("StockHoldingSupplier");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FustWebApp.Models.Domain.Loads", b =>
                {
                    b.Navigation("LoadFustItems");
                });
#pragma warning restore 612, 618
        }
    }
}
