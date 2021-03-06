﻿// <auto-generated />
using Jam_Inspired_Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Jam_Inspired_Persistence.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200304152918_SeedValues")]
    partial class SeedValues
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0");

            modelBuilder.Entity("Jam_Inspired_Domain.Value", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Values");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Kenneth"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Aaron"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Ashley"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
