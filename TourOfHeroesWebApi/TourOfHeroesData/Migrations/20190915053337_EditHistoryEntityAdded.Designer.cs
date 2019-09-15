﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TourOfHeroesData;

namespace TourOfHeroesData.Migrations
{
    [DbContext(typeof(TourOfHeroesDbContext))]
    [Migration("20190915053337_EditHistoryEntityAdded")]
    partial class EditHistoryEntityAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TourOfHeroesData.Models.EditHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EditedOn");

                    b.Property<int>("HeroId");

                    b.Property<string>("NewValue");

                    b.Property<string>("OldValue");

                    b.HasKey("Id");

                    b.HasIndex("HeroId");

                    b.ToTable("EditHistories");
                });

            modelBuilder.Entity("TourOfHeroesData.Models.Hero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CoverImage");

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Heroes");
                });

            modelBuilder.Entity("TourOfHeroesData.Models.EditHistory", b =>
                {
                    b.HasOne("TourOfHeroesData.Models.Hero", "Hero")
                        .WithMany("EditHistories")
                        .HasForeignKey("HeroId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
