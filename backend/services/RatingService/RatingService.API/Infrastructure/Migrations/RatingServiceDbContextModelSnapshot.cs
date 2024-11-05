﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RatingService.API.Infrastructure;

#nullable disable

namespace RatingService.API.Infrastructure.Migrations
{
    [DbContext(typeof(RatingServiceDbContext))]
    partial class RatingServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("RatingService.API.Models.Course", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("AvgRating")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("InstructorId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RatingCount")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("RatingService.API.Models.Enrollment", b =>
                {
                    b.Property<string>("CourseId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("CourseId", "UserId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("RatingService.API.Models.Rating", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CourseId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Feedback")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Responded")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("RespondedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Response")
                        .HasColumnType("longtext");

                    b.Property<int>("Star")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("CourseId", "UserId")
                        .IsUnique();

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("RatingService.API.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ThumbnailUrl")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RatingService.API.Models.Rating", b =>
                {
                    b.HasOne("RatingService.API.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RatingService.API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
