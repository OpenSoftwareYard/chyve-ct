using System;
using System.Net.Http.Json;
using ChyveClient.Models;

namespace ChyveClient;

public partial class Client
{
    public async Task<Vnic> CreateVnic(Vnic vnic)
    {
        var res = await _httpClient.PostAsJsonAsync(
            "/net/vnics",
            vnic
        );

        var createdVnic = await res.Content.ReadFromJsonAsync<Vnic>() ?? throw new Exception($"Failed to create vnic {await res.Content.ReadAsStringAsync()}");
        return createdVnic;
    }
}
