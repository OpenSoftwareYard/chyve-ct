using System.Net.Http.Json;
using ChyveClient.Models;
using Persistence.DTOs;

namespace ChyveClient;

public partial class Client
{
    public static async Task<IEnumerable<ZoneDTO>?> GetZones(Uri baseUri, string accessToken)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = baseUri;

        var zones = await httpClient.GetFromJsonAsync<IEnumerable<Zone>>(
            $"/zones?api_key={accessToken}"
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

    public static async Task<TaskHandle> CreateZone(Uri baseUri, string accessToken, Zone zone)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = baseUri;

        var res = await httpClient.PostAsJsonAsync(
            $"/zones?api_key={accessToken}",
            zone
        );

        var taskHandle = await res.Content.ReadFromJsonAsync<TaskHandle>() ?? throw new Exception($"Failed to create zone {await res.Content.ReadAsStringAsync()}");

        return taskHandle;
    }
}
