using System.Net;
using System.Text.Json.Serialization;
using Persistence.Entities;

namespace Persistence.DTOs;

public record class ZoneDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int CpuCount { get; set; }
    public int RamGB { get; set; }
    public int DiskGB { get; set; }
    public Guid? NodeId { get; set; }
    public Uri? ImageUri { get; set;}
    public required string Brand { get; set; }
    public Guid OrganizationId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ZoneStatus Status { get; set; }
    [JsonIgnore]
    public string? Path { get; set; }
    [JsonIgnore]
    public IPAddress? InternalIPAddress { get; set; }
    [JsonIgnore]
    public string? VNic { get; set; }
    [JsonIgnore]
    public string? IPType { get; set; }
    public required List<Service> ZoneServices { get; set;}
}
