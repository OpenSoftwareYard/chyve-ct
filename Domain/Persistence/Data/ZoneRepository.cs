using System;
using Microsoft.EntityFrameworkCore;
using Persistence.DTOs;

namespace Persistence.Data;

public class ZoneRepository(ChyveContext context)
{
    private readonly ChyveContext _context = context;

    public async Task<IReadOnlyCollection<ZoneDTO>?> GetZones(string userId)
    {
        var organizations = await _context.Organizations.Where(o => o.UserIds.Contains(userId)).ToListAsync();

        if (organizations.Count == 0)
        {
            return null;
        }

        return await _context.Zones
            .Where(z => organizations.Contains(z.Organization))
            .Select(z => ZoneDTO.FromZone(z))
            .ToListAsync();
    }
}
