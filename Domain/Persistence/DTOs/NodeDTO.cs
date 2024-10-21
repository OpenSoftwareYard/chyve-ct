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
}
