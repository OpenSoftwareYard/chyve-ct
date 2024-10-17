using System;
using EventBus;
using Events;
using Persistence.DTOs;
using Services;

namespace Scheduler.EventHandlers;

public class PlaceZoneIntegrationEventHandler(ILogger<PlaceZoneIntegrationEventHandler> logger, INodeService nodeService, IZoneService zoneService) : IIntegrationEventHandler<PlaceZoneIntegrationEvent>
{
    private readonly ILogger<PlaceZoneIntegrationEventHandler> _logger = logger;
    private readonly INodeService _nodeService = nodeService;

    private readonly IZoneService _zoneService = zoneService;

    public async Task Handle(PlaceZoneIntegrationEvent @event)
    {
        _logger.LogInformation("Handling scheduling event for zone {zoneId}", @event.Zone.Id);

        try
        {
            var selectedNode = await _nodeService.AllocateZoneToOptimalNode(@event.Zone);
            _logger.LogInformation("Selected node {nodeId} for zone {zoneId}", selectedNode.Id, @event.Zone.Id);

            @event.Zone.NodeId = selectedNode.Id;
            await _zoneService.Update(@event.Zone.Id, @event.Zone);
        }
        catch (Exception e)
        {
            _logger.LogCritical("Failed to allocate zone to node {ex}", e);
            throw;
        }

        await Task.CompletedTask;
    }
}
