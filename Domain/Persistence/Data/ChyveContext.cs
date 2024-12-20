using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System;
using System.Net;

namespace Persistence.Data;

public class ChyveContext : DbContext
{
    public required virtual DbSet<Organization> Organizations { get; set; }
    public required virtual DbSet<Node> Nodes { get; set; }
    public required virtual DbSet<Zone> Zones { get; set; }
    public required virtual DbSet<PersonalAccessToken> PersonalAccessTokens { get; set; }
    public required virtual DbSet<PersonalAccessTokenScope> PersonalAccessTokenScopes { get; set; }

    public ChyveContext(DbContextOptions<ChyveContext> options) : base(options) { }

    public ChyveContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseHiLo();

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

        modelBuilder.Entity<Zone>()
            .OwnsMany(z => z.Services);

        modelBuilder.Entity<Organization>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()");
    }
}
