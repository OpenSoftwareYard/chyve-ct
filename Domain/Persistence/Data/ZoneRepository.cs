using System;
using Microsoft.EntityFrameworkCore;
using Persistence.DTOs;

namespace Persistence.Data;

public class ZoneRepository(ChyveContext context)
{
    private readonly ChyveContext _context = context;

    public async Task<IReadOnlyCollection<ZoneDTO>> GetZones(string userId)
    {
        return await _context.Zones.Where(z => z.UserId == userId).Select(z => ZoneDTO.FromZone(z)).ToListAsync();
    }
}
