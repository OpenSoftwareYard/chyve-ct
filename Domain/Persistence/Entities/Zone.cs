using System;
using System.Net;

namespace Persistence.Entities;

public class Zone : BaseEntity
{
    public Guid ZoneId { get; set; }
    public required string Name { get; set; }
    public required string Path { get; set; }
    public required string Brand { get; set; }
    public required string IPType { get; set; }
    public required string VNic { get; set; }
    public required IPAddress InternalIPAddress { get; set; }

    public required int OrganizationId { get; set; }
    public required Organization Organization { get; set; }

    public required int NodeId { get; set; }
    public required Node Node { get; set; }
}
