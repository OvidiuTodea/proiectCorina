﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication3.Models;

namespace WebApplication3.Migrations
{
    [DbContext(typeof(MoviesDbContext))]
    [Migration("20190609070947_forCascadeDelete1")]
    partial class forCascadeDelete1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApplication3.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Important");

                    b.Property<int?>("MovieId");

                    b.Property<int?>("OwnerId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("WebApplication3.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<string>("Director");

                    b.Property<int>("Duration");

                    b.Property<int>("Genre");

                    b.Property<int?>("OwnerId");

                    b.Property<int>("Rating");

                    b.Property<int>("ReleaseYear");

                    b.Property<string>("Title");

                    b.Property<bool>("Watched");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("WebApplication3.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataRegistered");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<int>("UserRole");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApplication3.Models.Comment", b =>
                {
                    b.HasOne("WebApplication3.Models.Movie", "Movie")
                        .WithMany("Comments")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApplication3.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("WebApplication3.Models.Movie", b =>
                {
                    b.HasOne("WebApplication3.Models.User", "Owner")
                        .WithMany("Movies")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
