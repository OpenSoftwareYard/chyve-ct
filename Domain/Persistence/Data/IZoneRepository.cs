using System;
using Persistence.Entities;

namespace Persistence.Data;

public interface IZoneRepository : IGenericRepository<Zone>
{
    Task<IEnumerable<Zone>?> GetForUserId(string id);
    Task<Zone?> AddForUserId(Zone zone, string id);
}
