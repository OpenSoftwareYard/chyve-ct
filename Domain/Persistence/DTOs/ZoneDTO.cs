using System;
using Persistence.Entities;

namespace Persistence.DTOs;

public record ZoneDTO
{
    public Guid Id { get; set; }

    public static ZoneDTO FromZone(Zone zone)
    {
        return new ZoneDTO
        {
            Id = zone.ZoneId
        };
    }
}
