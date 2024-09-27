﻿// <auto-generated />
using System;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence.Data;

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

            NpgsqlModelBuilderExtensions.UseHiLo(modelBuilder, "EntityFrameworkHiLoSequence");

            modelBuilder.HasSequence("EntityFrameworkHiLoSequence")
                .IncrementsBy(10);

            modelBuilder.Entity("Persistence.Entities.Node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"));

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<IPAddress>("DefRouter")
                        .IsRequired()
                        .HasColumnType("inet");

                    b.Property<string>("ExternalNetworkDevice")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("NodeId")
                        .HasColumnType("uuid");

                    b.Property<string>("PrivateZoneNetwork")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Nodes");
                });

            modelBuilder.Entity("Persistence.Entities.Zone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<int>("Id"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IPType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<IPAddress>("InternalIPAddress")
                        .IsRequired()
                        .HasColumnType("inet");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NodeId")
                        .HasColumnType("integer");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("VNic")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ZoneId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("NodeId");

                    b.ToTable("Zones");
                });

            modelBuilder.Entity("Persistence.Entities.Zone", b =>
                {
                    b.HasOne("Persistence.Entities.Node", "Node")
                        .WithMany("Zones")
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Node");
                });

            modelBuilder.Entity("Persistence.Entities.Node", b =>
                {
                    b.Navigation("Zones");
                });
#pragma warning restore 612, 618
        }
    }
}
