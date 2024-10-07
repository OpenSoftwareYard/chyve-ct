using System;
using System.Net;

namespace Persistence.Entities;

public class Node : BaseEntity
{
    public Guid NodeId { get; set; }
    public required Uri Uri { get; set; }
    public required string AccessToken { get; set; }
    public required string ExternalNetworkDevice { get; set; }
    public required IPAddress DefRouter { get; set; }
    public IPNetwork PrivateZoneNetwork { get; set; }

    public required List<Zone> Zones { get; set; }
}
