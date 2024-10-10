using System;
using Persistence.Entities;

namespace Persistence.DTOs;

public record class ZoneDTO
{
    public Guid ZoneId { get; set; }
    public required string Name { get; set; }
    public int CpuCount { get; set; }
    public int RamGB { get; set; }
    public int DiskGB { get; set; }
    public int NodeId { get; set; }
}
