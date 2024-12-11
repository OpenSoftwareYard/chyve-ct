using System;
using Persistence.Entities;

namespace Persistence.Data;

public interface IZoneRepository : IGenericRepository<Zone>
{
    Task<IEnumerable<Zone>?> GetZonesForUserId(string userId);
    Task<IEnumerable<Zone>?> GetZonesForOrgId(string orgId);
    Task<Zone?> GetZoneForUserId(Guid zoneId, string userId);
    Task<Zone?> GetZoneForOrgId(Guid zoneId, string orgId);
    Task<Zone?> AddForUserId(Zone zone, string id);
    Task<Zone?> AddForOrgId(Zone zone, string orgId);
    Task<IEnumerable<Zone>> GetUnscheduledZones();
}
