using System;
using System.Text.Json.Serialization;
using AutoMapper.Internal.Mappers;

namespace ChyveClient.Models;

public record Net
{
    public required string Physical { get; set; }

    [JsonPropertyName("allowed_address")]
    public required string AllowedAddress { get; set; }

    [JsonPropertyName("defrouter")]
    public required string RouterAddress { get; set; }
}
public record Zone
{
    public required string Id { get; set; }

    [JsonPropertyName("zonename")]
    public required string Name { get; set; }

    [JsonPropertyName("zonepath")]
    public required string Path { get; set; }

    public required string Brand { get; set; }

    [JsonPropertyName("ip_type")]
    public required string IPType { get; set; }

    [JsonPropertyName("ncpus")]
    public required int CpuCount { get; set; }

    [JsonPropertyName("physical_mem")]
    public required string PhysicalMemory { get; set; }

    public required string PhysicalDisk { get; set; } = "4G";

    public required Net Net { get; set; }

    public static int ParsePhysicalMemoryString(string physicalMemory)
    {
        var parsed_mem_amount = int.Parse(physicalMemory.SkipLast(1).ToString()!);
        if (physicalMemory.ToLower().EndsWith('g'))
        {
            return parsed_mem_amount;
        }
        if (physicalMemory.ToLower().EndsWith('m'))
        {
            return parsed_mem_amount % 1000 + 1;
        }

        // Should not happen
        throw new Exception("Unknown memory unit");
    }
}
