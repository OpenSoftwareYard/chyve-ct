using System;
using System.Net;

namespace Persistence.Entities;

public class Node : BaseEntity
{
    public required Uri WebApiUri { get; set; }
    public required string AccessToken { get; set; }
    public required string ExternalNetworkDevice { get; set; }
    public required string InternalStubDevice { get; set; }
    public required IPAddress DefRouter { get; set; }
    public IPNetwork PrivateZoneNetwork { get; set; }
    public required string ZoneBasePath { get; set; }

    public int TotalCpu { get; set; }
    public int UsedCpu { get; set; }

    public int TotalRamGB { get; set; }
    public int UsedRamGB { get; set; }

    public int TotalDiskGB { get; set; }
    public int UsedDiskGB { get; set; }

    public required List<Zone> Zones { get; set; }
}
