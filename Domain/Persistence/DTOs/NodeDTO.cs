using System.Net;
using System.Text.Json.Serialization;

namespace Persistence.DTOs;

public record class NodeDTO
{
    public Guid Id { get; set; }
    public required Uri WebApiUri { get; set; }
    public required string AccessToken { get; set; }
    public int TotalCpu { get; set; }
    public int UsedCpu { get; set; }
    public int TotalRamGB { get; set; }
    public int UsedRamGB { get; set; }
    public int TotalDiskGB { get; set; }
    public int UsedDiskGB { get; set; }
    public required List<ZoneDTO> Zones { get; set; }
    [JsonIgnore]
    public IPNetwork PrivateZoneNetwork { get; set; }
    [JsonIgnore]
    public IPAddress? DefRouter { get; set; }
    public required string ExternalNetworkDevice { get; set; }
    public required string InternalStubDevice { get; set; }
    public required string ZoneBasePath { get; set; }
}
