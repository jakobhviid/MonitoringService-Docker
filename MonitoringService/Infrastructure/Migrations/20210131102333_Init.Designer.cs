﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MonitoringService.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MonitoringService.Infrastructure.Migrations
{
    [DbContext(typeof(DockerHostContext))]
    [Migration("20210131102333_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("MonitoringService.Domain.DockerContainer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ContainerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("DockerHostId")
                        .HasColumnType("uuid");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DockerHostId");

                    b.HasIndex("ContainerId", "DockerHostId")
                        .IsUnique();

                    b.ToTable("DockerContainers");
                });

            modelBuilder.Entity("MonitoringService.Domain.DockerHost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CommandRequestTopic")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CommandResponseTopic")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ServerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ServerName")
                        .IsUnique();

                    b.ToTable("DockerHosts");
                });

            modelBuilder.Entity("MonitoringService.Domain.StatsRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("CpuPercentage")
                        .HasColumnType("double precision");

                    b.Property<decimal>("CpuUsage")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("DiskInputBytes")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("DiskOutputBytes")
                        .HasColumnType("numeric(20,0)");

                    b.Property<Guid>("DockerContainerId")
                        .HasColumnType("uuid");

                    b.Property<double>("MemoryPercentage")
                        .HasColumnType("double precision");

                    b.Property<decimal>("NetInputBytes")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("NetOutputBytes")
                        .HasColumnType("numeric(20,0)");

                    b.Property<int>("NumOfCpu")
                        .HasColumnType("integer");

                    b.Property<decimal>("SystemCpuUsage")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("DockerContainerId", "UpdateTime")
                        .IsUnique();

                    b.ToTable("StatsRecords");
                });

            modelBuilder.Entity("MonitoringService.Domain.StatusRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DockerContainerId")
                        .HasColumnType("uuid");

                    b.Property<string>("Health")
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("DockerContainerId", "UpdateTime")
                        .IsUnique();

                    b.ToTable("StatusRecords");
                });

            modelBuilder.Entity("MonitoringService.Domain.DockerContainer", b =>
                {
                    b.HasOne("MonitoringService.Domain.DockerHost", "DockerHost")
                        .WithMany("DockerContainers")
                        .HasForeignKey("DockerHostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DockerHost");
                });

            modelBuilder.Entity("MonitoringService.Domain.StatsRecord", b =>
                {
                    b.HasOne("MonitoringService.Domain.DockerContainer", "DockerContainer")
                        .WithMany("StatsRecords")
                        .HasForeignKey("DockerContainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DockerContainer");
                });

            modelBuilder.Entity("MonitoringService.Domain.StatusRecord", b =>
                {
                    b.HasOne("MonitoringService.Domain.DockerContainer", "DockerContainer")
                        .WithMany("StatusRecords")
                        .HasForeignKey("DockerContainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DockerContainer");
                });

            modelBuilder.Entity("MonitoringService.Domain.DockerContainer", b =>
                {
                    b.Navigation("StatsRecords");

                    b.Navigation("StatusRecords");
                });

            modelBuilder.Entity("MonitoringService.Domain.DockerHost", b =>
                {
                    b.Navigation("DockerContainers");
                });
#pragma warning restore 612, 618
        }
    }
}
