﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence.Data;
using Persistence.Entities;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(ChyveContext))]
    partial class ChyveContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "zone_status", new[] { "unscheduled", "running", "stopped", "scheduling", "scheduled" });
            NpgsqlModelBuilderExtensions.UseHiLo(modelBuilder, "EntityFrameworkHiLoSequence");

            modelBuilder.HasSequence("EntityFrameworkHiLoSequence")
                .IncrementsBy(10);

            modelBuilder.Entity("Persistence.Entities.Node", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("ConnectionKey")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("ConnectionUser")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<IPAddress>("DefRouter")
                        .IsRequired()
                        .HasColumnType("inet");

                    b.Property<string>("ExternalNetworkDevice")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("InternalStubDevice")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Port")
                        .HasColumnType("integer");

                    b.Property<string>("PrivateZoneNetwork")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TotalCpu")
                        .HasColumnType("integer");

                    b.Property<int>("TotalDiskGB")
                        .HasColumnType("integer");

                    b.Property<int>("TotalRamGB")
                        .HasColumnType("integer");

                    b.Property<string>("ZoneBasePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Nodes");
                });

            modelBuilder.Entity("Persistence.Entities.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<string>>("UserIds")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Persistence.Entities.Zone", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Brand")
                        .HasColumnType("text");

                    b.Property<int>("CpuCount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("DiskGB")
                        .HasColumnType("integer");

                    b.Property<string>("IPType")
                        .HasColumnType("text");

                    b.Property<IPAddress>("InternalIPAddress")
                        .HasColumnType("inet");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("NodeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Path")
                        .HasColumnType("text");

                    b.Property<int>("RamGB")
                        .HasColumnType("integer");

                    b.Property<ZoneStatus>("Status")
                        .HasColumnType("zone_status");

                    b.Property<string>("VNic")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("NodeId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Zones");
                });

            modelBuilder.Entity("Persistence.Entities.Zone", b =>
                {
                    b.HasOne("Persistence.Entities.Node", "Node")
                        .WithMany("Zones")
                        .HasForeignKey("NodeId");

                    b.HasOne("Persistence.Entities.Organization", "Organization")
                        .WithMany("Zones")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Node");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Persistence.Entities.Node", b =>
                {
                    b.Navigation("Zones");
                });

            modelBuilder.Entity("Persistence.Entities.Organization", b =>
                {
                    b.Navigation("Zones");
                });
#pragma warning restore 612, 618
        }
    }
}
