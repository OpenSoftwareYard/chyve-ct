using System.Net.Http.Json;
using ChyveClient.Models;
using Persistence.DTOs;

namespace ChyveClient;

public partial class Client
{
    public async Task<IEnumerable<ZoneDTO>?> GetZones()
    {
        var zones = await _httpClient.GetFromJsonAsync<IEnumerable<Zone>>(
            $"/zones?api_key={_accessToken}"
        );

        return zones?.Select(z => new ZoneDTO
        {
            Id = new Guid(z.Id),
            Name = z.Name,
            CpuCount = z.CpuCount,
            RamGB = Zone.ParsePhysicalSizeString(z.PhysicalMemory),
            DiskGB = Zone.ParsePhysicalSizeString(z.PhysicalDisk),
        });
    }

    public async Task<ZoneDTO> CreateZone(Zone zone)
    {
        var res = await _httpClient.PutAsJsonAsync(
            "/zones",
            zone
        );

        var returnedZone = await res.Content.ReadFromJsonAsync<Zone>() ?? throw new Exception($"Failed to create zone {await res.Content.ReadAsStringAsync()}");

        return new ZoneDTO
        {
            Id = new Guid(returnedZone.Id),
            Name = returnedZone.Name,
            CpuCount = returnedZone.CpuCount,
            RamGB = Zone.ParsePhysicalSizeString(zone.PhysicalMemory),
            DiskGB = Zone.ParsePhysicalSizeString(zone.PhysicalDisk),
        };
    }
}
