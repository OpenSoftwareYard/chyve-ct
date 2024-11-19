
using Microsoft.Extensions.DependencyInjection;
using Persistence.DTOs;
using Persistence.Entities;
using Services;

namespace Scheduler.EventHandlers;

public class PlaceZoneHandler(ILogger<PlaceZoneHandler> logger, IServiceScopeFactory serviceScopeFactory)
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly ILogger<PlaceZoneHandler> _logger = logger;
    public async Task PlaceUnscheduledZones()
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var nodeService = scope.ServiceProvider.GetRequiredService<INodeService>();
        var zoneService = scope.ServiceProvider.GetRequiredService<IZoneService>();

        _logger.LogInformation("Start placing unscheduled zones");

        var unscheduledZones = await GetPendingZones(zoneService);
        await PlaceZone(unscheduledZones.First(), nodeService, zoneService);

        _logger.LogInformation("Finished placing unscheduled zones");
    }

    private async Task PlaceZone(ZoneDTO zone, INodeService nodeService, IZoneService zoneService)
    {
        _logger.LogInformation("Handling scheduling event for zone {zoneId}", zone.Id);

        zone.Status = ZoneStatus.SCHEDULING;
        await zoneService.Update(zone.Id, zone);

        try
        {
            var allocationResponse = await nodeService.AllocateZoneToOptimalNode(zone);
            _logger.LogInformation("Selected node {nodeId} for zone {zoneId}", allocationResponse.Node.Id, zone.Id);

            zone.NodeId = allocationResponse.Node.Id;
            zone.Status = ZoneStatus.SCHEDULED;
            zone.Path = allocationResponse.Path;
            zone.InternalIPAddress = allocationResponse.IpAddress;
            zone.VNic = allocationResponse.VnicName;
            zone.IPType = allocationResponse.IPType;
            zone.Brand = allocationResponse.Brand;

            await zoneService.Update(zone.Id, zone);
        }
        catch (Exception e)
        {
            _logger.LogCritical("Failed to allocate zone to node {ex}", e);
            zone.Status = ZoneStatus.UNSCHEDULED;

            await zoneService.Update(zone.Id, zone);

            throw;
        }
    }

    private async Task<IEnumerable<ZoneDTO>> GetPendingZones(IZoneService zoneService)
    {
        _logger.LogInformation("Finding unscheduled zones");

        var unscheduledZones = await zoneService.GetUnscheduledZones();
        return unscheduledZones;
    }
}
