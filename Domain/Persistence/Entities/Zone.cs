using System;
using System.Net;

namespace Persistence.Entities;

public class Zone : BaseEntity
{
    public required string Name { get; set; }
    public string? Path { get; set; }
    public required string Brand { get; set; }
    public string? IPType { get; set; }
    public string? VNic { get; set; }
    public IPAddress? InternalIPAddress { get; set; }

    public int CpuCount { get; set; }
    public int RamGB { get; set; }
    public int DiskGB { get; set; }

    public required Guid OrganizationId { get; set; }
    public required Organization Organization { get; set; }

    public Guid? NodeId { get; set; }
    public Node? Node { get; set; }
}
