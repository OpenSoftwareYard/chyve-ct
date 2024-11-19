using System;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Data;

public class ZoneRepository(ChyveContext context) : GenericRepository<Zone>(context), IZoneRepository
{
    public async Task<IEnumerable<Zone>?> GetZonesForUserId(string userId)
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

    public async Task<Zone?> GetZoneForUserId(Guid zoneId, string userId)
    {
        var organizations = await _context.Organizations.Where(o => o.UserIds.Contains(userId)).ToListAsync();

        if (organizations.Count == 0)
        {
            return null;
        }

        var zone = await GetById(zoneId);

        if (zone == null || !organizations.Contains(zone.Organization))
        {
            return null;
        }

        return zone;
    }

    public async Task<Zone?> AddForUserId(Zone zone, string userId)
    {
        var organizations = await _context.Organizations.Where(o => o.UserIds.Contains(userId)).ToListAsync();

        Console.WriteLine("Using userId {0}", userId);
        Console.WriteLine("Got organizations {0}", organizations[0]);

        if (organizations.Count == 0)
        {
            return null;
        }

        zone.Organization = organizations.First();
        zone.OrganizationId = zone.Organization.Id;

        Console.WriteLine("Using final zone {0}", zone);

        return await Add(zone);
    }

    public async Task<IEnumerable<Zone>> GetUnscheduledZones()
    {
        var zones = await _context.Zones.Where(z => z.NodeId == null && z.Status == ZoneStatus.UNSCHEDULED).OrderBy(z => z.CreatedAt).ToListAsync();
        return zones;
    }
}
