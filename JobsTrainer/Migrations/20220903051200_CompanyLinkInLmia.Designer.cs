﻿// <auto-generated />
using System;
using JobsTrainer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JobsTrainer.Migrations
{
    [DbContext(typeof(TrainingContext))]
    [Migration("20220903051200_CompanyLinkInLmia")]
    partial class CompanyLinkInLmia
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("JobsTrainer.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsFriendly")
                        .HasColumnType("bit");

                    b.Property<string>("Link")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("JobsTrainer.Models.Keyword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Keywords");
                });

            modelBuilder.Entity("JobsTrainer.Models.LmiaInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Approved")
                        .HasColumnType("int");

                    b.Property<string>("CompanyLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Employer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Occupation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProgramStream")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProvinceTerritory")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LmiaInfos");
                });

            modelBuilder.Entity("JobsTrainer.Models.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("JobsTrainer.Models.TrainJob", b =>
                {
                    b.Property<long>("JobId")
                        .HasColumnType("bigint");

                    b.Property<string>("Company")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CompanyLink")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Country")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Exported")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEasy")
                        .HasColumnType("bit");

                    b.Property<string>("Sample")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Sentiment")
                        .HasColumnType("bit");

                    b.Property<string>("Skills")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("JobId");

                    b.HasIndex("Title", "CreatedAt");

                    b.ToTable("TrainJobs");
                });
#pragma warning restore 612, 618
        }
    }
}