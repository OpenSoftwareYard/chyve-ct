using System;
using System.Net;

namespace Persistence.Entities;

public class Node : BaseEntity
{
    public required string Address { get; set; }
    public required int Port { get; set; }
    public required byte[] ConnectionKey { get; set; }
    public required string ConnectionUser { get; set; }
    public required string ExternalNetworkDevice { get; set; }
    public required string InternalStubDevice { get; set; }
    public required IPAddress DefRouter { get; set; }
    public IPNetwork PrivateZoneNetwork { get; set; }
    public required string ZoneBasePath { get; set; }

    public int TotalCpu { get; set; }
    public int TotalRamGB { get; set; }
    public int TotalDiskGB { get; set; }
    public required List<Zone> Zones { get; set; }
}
