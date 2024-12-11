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
        var zones = await _zoneRepository.GetZonesForUserId(userId);
        return _mapper.Map<IEnumerable<ZoneDTO>>(zones);
    }

    public async Task<ZoneDTO?> GetZoneForUserId(Guid zoneId, string userId)
    {
        var zone = await _zoneRepository.GetZoneForUserId(zoneId, userId);
        return _mapper.Map<ZoneDTO>(zone);
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

        // TODO: Implement matching zones and statuses from db
        return zones;
    }

    public async Task<ZoneDTO?> BootZone(NodeDTO node, ZoneDTO zone)
    {
        var bootedZone = await _chyveClient.BootZone(node, zone.Id.ToString());

        zone.Status = ZoneStatus.RUNNING;

        var updatedZone = await Update(new Guid(bootedZone.Name), zone);

        return updatedZone;
    }

    public async Task<ZoneDTO?> StopZone(NodeDTO node, ZoneDTO zone)
    {
        var stoppedZone = await _chyveClient.StopZone(node, zone.Id.ToString());

        zone.Status = ZoneStatus.STOPPED;

        var updatedZone = await Update(new Guid(stoppedZone.Name), zone);

        return updatedZone;
    }

    public async Task<ZoneDTO?> DeleteZone(NodeDTO node, ZoneDTO zone)
    {
        var deletedZone = await _chyveClient.DeleteZone(node, zone.Id.ToString());
        _ = await _chyveClient.DeleteVnic(node, deletedZone.Net.First().Physical);

        var updatedZone = await Delete(new Guid(deletedZone.Name));

        return updatedZone;
    }

    public async Task<IEnumerable<ZoneDTO>?> GetZonesForOrgId(string orgId)
    {
        var zones = await _zoneRepository.GetZonesForOrgId(orgId);
        return _mapper.Map<IEnumerable<ZoneDTO>>(zones);
    }

    public async Task<ZoneDTO?> GetZoneForOrgId(Guid zoneId, string orgId)
    {
        var zone = await _zoneRepository.GetZoneForOrgId(zoneId, orgId);
        return _mapper.Map<ZoneDTO>(zone);
    }

    public async Task<ZoneDTO?> CreateForOrgId(ZoneDTO zone, string orgId)
    {
        var zoneToAdd = _mapper.Map<Zone>(zone);
        var addedZone = await _zoneRepository.AddForOrgId(zoneToAdd, orgId);
        return _mapper.Map<ZoneDTO>(addedZone);
    }
}
