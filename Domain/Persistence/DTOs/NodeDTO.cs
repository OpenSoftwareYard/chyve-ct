namespace Persistence.DTOs;

public record class NodeDTO
{
    public Guid NodeId { get; set; }
    public int TotalCpu { get; set; }
    public int UsedCpu { get; set; }
    public int TotalRamGB { get; set; }
    public int UsedRamGB { get; set; }
    public int TotalDiskGB { get; set; }
    public int UsedDiskGB { get; set; }
    public required List<ZoneDTO> Zones { get; set; }
}
