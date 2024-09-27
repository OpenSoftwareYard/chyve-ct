using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System;
using System.Net;

namespace Persistence.Data;

public class ChyveContext : DbContext
{
    public virtual DbSet<Node> Nodes { get; set; }
    public virtual DbSet<Zone> Zones { get; set; }

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
    }
}
