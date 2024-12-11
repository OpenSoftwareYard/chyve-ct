using System;
using Persistence.DTOs;
using Persistence.Entities;

namespace Services;

public interface IZoneService : IGenericService<Zone, ZoneDTO>
{
    Task<IEnumerable<ZoneDTO>?> GetZonesForUserId(string userId);
    Task<IEnumerable<ZoneDTO>?> GetZonesForOrgId(string orgId);
    Task<ZoneDTO?> GetZoneForUserId(Guid zoneId, string userId);
    Task<ZoneDTO?> GetZoneForOrgId(Guid zoneId, string orgId);
    Task<ZoneDTO?> CreateForUserId(ZoneDTO zone, string userId);
    Task<ZoneDTO?> CreateForOrgId(ZoneDTO zone, string orgId);
    Task<IEnumerable<ZoneDTO>> GetUnscheduledZones();
    Task<IEnumerable<ZoneDTO>> PopulateZonesForNode(NodeDTO node);
    Task<ZoneDTO?> BootZone(NodeDTO node, ZoneDTO zone);
    Task<ZoneDTO?> StopZone(NodeDTO node, ZoneDTO zone);
    Task<ZoneDTO?> DeleteZone(NodeDTO node, ZoneDTO zone);
}
