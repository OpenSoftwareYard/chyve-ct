
using Persistence.DTOs;
using Services;

namespace Scheduler.EventHandlers;

public class PlaceZoneHandler(ILogger<PlaceZoneHandler> logger, INodeService nodeService, IZoneService zoneService)
{
    private readonly ILogger<PlaceZoneHandler> _logger = logger;
    private readonly INodeService _nodeService = nodeService;
    private readonly IZoneService _zoneService = zoneService;

    public async Task PlaceUnscheduledZones()
    {
        _logger.LogInformation("Start placing unscheduled zones");

        var unscheduledZones = await GetPendingZones();
        foreach (var zone in unscheduledZones)
        {
            await PlaceZone(zone);
        }

        _logger.LogInformation("Finished placing unscheduled zones");
    }

    private async Task PlaceZone(ZoneDTO zone)
    {
        _logger.LogInformation("Handling scheduling event for zone {zoneId}", zone.Id);

        try
        {
            var selectedNode = await _nodeService.AllocateZoneToOptimalNode(zone);
            _logger.LogInformation("Selected node {nodeId} for zone {zoneId}", selectedNode.Id, zone.Id);

            zone.NodeId = selectedNode.Id;
            await _zoneService.Update(zone.Id, zone);
        }
        catch (Exception e)
        {
            _logger.LogCritical("Failed to allocate zone to node {ex}", e);
            throw;
        }
    }

    private async Task<IEnumerable<ZoneDTO>> GetPendingZones()
    {
        _logger.LogInformation("Finding unscheduled zones");

        var unscheduledZones = await _zoneService.GetUnscheduledZones();
        return unscheduledZones;
    }
}
