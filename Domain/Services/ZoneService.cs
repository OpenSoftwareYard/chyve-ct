using System;
using System.ComponentModel;
using AutoMapper;
using Persistence.Data;
using Persistence.DTOs;
using Persistence.Entities;

namespace Services;

public class ZoneService(IZoneRepository repository, IMapper mapper, ChyveClient.Client chyveClient) : GenericService<Zone, ZoneDTO>(repository, mapper), IZoneService
{
    private readonly IZoneRepository _zoneRepository = repository;
    private readonly ChyveClient.Client _chyveClient = chyveClient;

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

    public async Task<IEnumerable<ZoneDTO>> PopulateZonesForNode(NodeDTO node)
    {
        var zones = await _chyveClient.GetZones(node);

        return zones.Select(zone => new ZoneDTO()
        {
            Id = new Guid(),
            Name = zone.Name,
            CpuCount = (int)(zone.CappedCpu?.Ncpus ?? 0),
            RamGB = (int)(zone.CappedMemory?.Physical ?? 0),
            DiskGB = 4,
            OrganizationId = Guid.Empty,
            Status = ZoneStatus.SCHEDULED,
        })
            .ToList();
    }
}
