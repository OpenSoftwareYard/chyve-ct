using System;
using System.Net.Http.Json;
using ChyveClient.Models;

namespace ChyveClient;

public partial class Client
{
    public static async Task<TaskHandle> CreateVnic(Uri baseUri, string accessToken, Vnic vnic)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = baseUri;

        var res = await httpClient.PostAsJsonAsync(
            $"/net/vnics?api_key={accessToken}",
            vnic
        );

        var createdVnicHandle = await res.Content.ReadFromJsonAsync<TaskHandle>() ?? throw new Exception($"Failed to create vnic {await res.Content.ReadAsStringAsync()}");
        return createdVnicHandle;
    }
}
