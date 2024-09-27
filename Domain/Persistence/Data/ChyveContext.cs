using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System;

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
  }
}
