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

            modelBuilder.Entity("PaymentService.API.Models.BatchPayout", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ReferencePaypalPayoutBatchId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Date");

                    b.ToTable("BatchPayouts");
                });

            modelBuilder.Entity("PaymentService.API.Models.Course", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("PaymentService.API.Models.Payment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CouponCode")
                        .HasColumnType("longtext");

                    b.Property<string>("CourseCreatorId")
                        .IsRequired()
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

                    b.Property<DateTime>("RefundDueDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RefundReason")
                        .HasColumnType("longtext");

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

                    b.HasIndex("CreatedAt", "Status");

                    b.HasIndex("Status", "PaymentDueDate");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("PaymentService.API.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PaypalEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("PaypalPayerId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
