using System;
using System.ComponentModel;
using AutoMapper;
using Persistence.Data;
using Persistence.DTOs;
using Persistence.Entities;

namespace Services;

public class ZoneService(IZoneRepository repository, IMapper mapper) : GenericService<Zone, ZoneDTO>(repository, mapper), IZoneService
{
    private readonly IZoneRepository _zoneRepository = repository;
    public async Task<IEnumerable<ZoneDTO>?> GetZonesForUserId(string userId)
    {
        var zones = await _zoneRepository.GetForUserId(userId);
        return _mapper.Map<IEnumerable<ZoneDTO>>(zones);
    }
}
