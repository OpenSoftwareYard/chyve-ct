using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System;
using System.Net;

namespace Persistence.Data;

public class ChyveContext : DbContext
{
    public virtual DbSet<Organization> Organizations { get; set; }
    public virtual DbSet<Node> Nodes { get; set; }
    public virtual DbSet<Zone> Zones { get; set; }

    public ChyveContext(DbContextOptions<ChyveContext> options) : base(options) { }

    public ChyveContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseHiLo();

        modelBuilder.HasPostgresEnum<ZoneStatus>();

        modelBuilder.Entity<Node>()
                .Property(e => e.PrivateZoneNetwork)
                .HasConversion(
                    v => v.ToString(),
                    v => IPNetwork.Parse(v)
            );

        modelBuilder.Entity<Node>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        modelBuilder.Entity<Zone>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");

        modelBuilder.Entity<Organization>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");
    }
}
