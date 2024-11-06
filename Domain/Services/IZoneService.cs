using System;
using Persistence.DTOs;
using Persistence.Entities;

namespace Services;

public interface IZoneService : IGenericService<Zone, ZoneDTO>
{
    Task<IEnumerable<ZoneDTO>?> GetZonesForUserId(string userId);
    Task<ZoneDTO?> CreateForUserId(ZoneDTO zone, string userId);
    Task<IEnumerable<ZoneDTO>> GetUnscheduledZones();
    Task<IEnumerable<ZoneDTO>> PopulateZonesForNode(NodeDTO node);
}
