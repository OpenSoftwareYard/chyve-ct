using System.Net.Http.Json;
using Persistence.DTOs;
using Persistence.Entities;

namespace ChyveClient;

public partial class Client
{
    public async Task<IEnumerable<ZoneDTO>?> GetZones()
    {
        var zones = await _httpClient.GetFromJsonAsync<IEnumerable<ChyveClient.Models.Zone>>(
            "/zones"
        );

        return zones?.Select(z => new ZoneDTO
        {
            Id = new Guid(z.Id),
            Name = z.Name,
            CpuCount = z.CpuCount,
            RamGB = Models.Zone.ParsePhysicalMemoryString(z.PhysicalMemory),
            DiskGB = 4,
        });
    }
}
