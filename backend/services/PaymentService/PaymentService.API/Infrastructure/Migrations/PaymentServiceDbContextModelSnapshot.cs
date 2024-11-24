﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PaymentService.API.Infrastructure;

#nullable disable

namespace PaymentService.API.Infrastructure.Migrations
{
    [DbContext(typeof(PaymentServiceDbContext))]
    partial class PaymentServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("PaymentService.API.Models.Payment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CouponCode")
                        .HasColumnType("longtext");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("ListPrice")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("PaymentDueDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PaypalOrderId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("PaypalOrderId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.HasIndex("Status", "PaymentDueDate");

                    b.ToTable("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
