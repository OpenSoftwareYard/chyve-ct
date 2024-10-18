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

    public async Task<ZoneDTO?> CreateForUserId(ZoneDTO zone, string userId)
    {
        var zoneToAdd = _mapper.Map<Zone>(zone);
        var addedZone = await _zoneRepository.AddForUserId(zoneToAdd, userId);
        return _mapper.Map<ZoneDTO>(addedZone);
    }

    public async Task<IEnumerable<ZoneDTO>> GetUnscheduledZones()
    {
        var zones = await _zoneRepository.GetUnscheduledZones();
        return _mapper.Map<IEnumerable<ZoneDTO>>(zones);
    }
}
