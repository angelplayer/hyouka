﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using hyouka_api.Infrastructure;

namespace hyoukaapi.Migrations
{
  [DbContext(typeof(HyoukaContext))]
  [Migration("20180809152202_create-table")]
  partial class createtable
  {
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
      modelBuilder
          .HasAnnotation("ProductVersion", "2.1.1-rtm-30846");

      modelBuilder.Entity("hyouka_api.Domain.Episode", b =>
          {
            b.Property<int>("EpisodeId")
                      .ValueGeneratedOnAdd();

            b.Property<string>("File");

            b.Property<int?>("MovieId");

            b.Property<string>("Name");

            b.Property<string>("Number");

            b.HasKey("EpisodeId");

            b.HasIndex("MovieId");

            b.ToTable("Episodes");
          });

      modelBuilder.Entity("hyouka_api.Domain.Genre", b =>
          {
            b.Property<int>("GenreId")
                      .ValueGeneratedOnAdd();

            b.Property<string>("Name");

            b.HasKey("GenreId");

            b.ToTable("Genres");
          });

      modelBuilder.Entity("hyouka_api.Domain.Movie", b =>
          {
            b.Property<int>("MovieId")
                      .ValueGeneratedOnAdd();

            b.Property<DateTime>("CreatedAt");

            b.Property<string>("Description");

            b.Property<string>("Image");

            b.Property<DateTime>("ReleaseDate");

            b.Property<string>("Title");

            b.Property<DateTime>("UpdatedAt");

            b.HasKey("MovieId");

            b.ToTable("Movies");
          });

      modelBuilder.Entity("hyouka_api.Domain.MovieGenre", b =>
          {
            b.Property<int>("GenreId");

            b.Property<int>("MovieId");

            b.HasKey("GenreId", "MovieId");

            b.HasIndex("MovieId");

            b.ToTable("MovieGenre");
          });

      modelBuilder.Entity("hyouka_api.Domain.Episode", b =>
          {
            b.HasOne("hyouka_api.Domain.Movie")
                      .WithMany("EpisodeList")
                      .HasForeignKey("MovieId");
          });

      modelBuilder.Entity("hyouka_api.Domain.MovieGenre", b =>
          {
            b.HasOne("hyouka_api.Domain.Genre", "Genre")
                      .WithMany()
                      .HasForeignKey("GenreId")
                      .OnDelete(DeleteBehavior.Cascade);

            b.HasOne("hyouka_api.Domain.Movie", "Movie")
                      .WithMany("MovieGenre")
                      .HasForeignKey("MovieId")
                      .OnDelete(DeleteBehavior.Cascade);
          });
#pragma warning restore 612, 618
    }
  }
}
