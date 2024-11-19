using System;
using System.Text.Json.Serialization;
using AutoMapper.Internal.Mappers;

namespace ChyveClient.Models;

public record CappedCpu
{
    [JsonPropertyName("ncpus")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
    public float Ncpus { get; set; }
}

public record CappedMemory
{
    [JsonPropertyName("physical")]
    public required string Physical { get; set; }
}

public record Net
{
    [JsonPropertyName("allowed-address")]
    public required string AllowedAddress { get; set; }

    [JsonPropertyName("defrouter")]
    public required string DefRouter { get; set; }

    [JsonPropertyName("physical")]
    public required string Physical { get; set; }
}

public record Zone
{
    [JsonPropertyName("autoboot")]
    public required string Autoboot { get; set; }

    [JsonPropertyName("bootargs")]
    public required string BootArgs { get; set; }

    [JsonPropertyName("brand")]
    public required string Brand { get; set; }

    [JsonPropertyName("capped-cpu")]
    public CappedCpu? CappedCpu { get; set; }

    [JsonPropertyName("capped-memory")]
    public CappedMemory? CappedMemory { get; set; }

    [JsonPropertyName("fs-allowed")]
    public required string FsAllowed { get; set; }

    [JsonPropertyName("hostid")]
    public required string HostId { get; set; }

    [JsonPropertyName("ip-type")]
    public required string IPType { get; set; }

    [JsonPropertyName("limitpriv")]
    public required string LimitPriv { get; set; }

    [JsonPropertyName("net")]
    public required IEnumerable<Net> Net { get; set; }

    [JsonPropertyName("pool")]
    public required string Pool { get; set; }

    [JsonPropertyName("scheduling-class")]
    public required string SchedulingClass { get; set; }

    [JsonPropertyName("zonename")]
    public required string Name { get; set; }

    [JsonPropertyName("zonepath")]
    public required string Path { get; set; }

    [JsonPropertyName("resolvers")]
    public List<string>? Resolvers { get; set; }
}
