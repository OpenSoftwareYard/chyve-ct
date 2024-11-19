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
    public Guid OrganizationId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ZoneStatus Status { get; set; }
}
