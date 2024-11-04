﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence.Data;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(ChyveContext))]
    [Migration("20241104165918_RenameNodeFields")]
    partial class RenameNodeFields
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseHiLo(modelBuilder, "EntityFrameworkHiLoSequence");

            modelBuilder.HasSequence("EntityFrameworkHiLoSequence")
                .IncrementsBy(10);

            modelBuilder.Entity("Persistence.Entities.Node", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AccessToken")
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

                    b.Property<string>("PrivateZoneNetwork")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TotalCpu")
                        .HasColumnType("integer");

                    b.Property<int>("TotalDiskGB")
                        .HasColumnType("integer");

                    b.Property<int>("TotalRamGB")
                        .HasColumnType("integer");

                    b.Property<int>("UsedCpu")
                        .HasColumnType("integer");

                    b.Property<int>("UsedDiskGB")
                        .HasColumnType("integer");

                    b.Property<int>("UsedRamGB")
                        .HasColumnType("integer");

                    b.Property<string>("WebApiUri")
                        .IsRequired()
                        .HasColumnType("text");

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
                        .IsRequired()
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
