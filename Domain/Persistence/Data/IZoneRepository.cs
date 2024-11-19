using System;
using Persistence.Entities;

namespace Persistence.Data;

public interface IZoneRepository : IGenericRepository<Zone>
{
    Task<IEnumerable<Zone>?> GetZonesForUserId(string userId);
    Task<Zone?> GetZoneForUserId(Guid zoneId, string userId);
    Task<Zone?> AddForUserId(Zone zone, string id);
    Task<IEnumerable<Zone>> GetUnscheduledZones();
}
