using System;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Data;

public class ZoneRepository(ChyveContext context) : GenericRepository<Zone>(context), IZoneRepository
{
    public async Task<IEnumerable<Zone>?> GetForUserId(string userId)
    {
        var organizations = await _context.Organizations.Where(o => o.UserIds.Contains(userId)).ToListAsync();

        if (organizations.Count == 0)
        {
            return null;
        }

        return await _context.Zones
          .Where(z => organizations.Contains(z.Organization))
          .ToListAsync();
    }
}
