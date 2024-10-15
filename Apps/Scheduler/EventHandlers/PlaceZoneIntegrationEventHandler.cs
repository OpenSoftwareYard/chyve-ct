using System;
using EventBus;
using Events;
using Persistence.DTOs;
using Services;

namespace Scheduler.EventHandlers;

public class PlaceZoneIntegrationEventHandler(ILogger<PlaceZoneIntegrationEventHandler> logger, INodeService nodeService) : IIntegrationEventHandler<PlaceZoneIntegrationEvent>
{
    private readonly ILogger<PlaceZoneIntegrationEventHandler> _logger = logger;
    private readonly INodeService _nodeService = nodeService;

    public async Task Handle(PlaceZoneIntegrationEvent @event)
    {
        _logger.LogInformation("Handling scheduling event for zone {zoneId}", @event.Zone.ZoneId);
        var nodes = await _nodeService.GetAll();
        if (nodes == null)
        {
            _logger.LogWarning("No nodes configured to schedule zones");
            return;
        }
        var selectedNode = nodes
            .Where(node => FilterNode(node, @event.Zone))
            .Select(node => new ScoredNode(node, ScoreNode(node, @event.Zone)))
            .OrderByDescending(sn => sn.Score)
            .Select(sn => sn.Node)
            .FirstOrDefault();

        if (selectedNode == null)
        {
            _logger.LogCritical("Failed to schedule zone {zoneId}", @event.Zone.ZoneId);
            return;
        }

        _logger.LogInformation("Selected node {nodeId} for zone {zoneId}", selectedNode.NodeId, @event.Zone.ZoneId);

        await Task.CompletedTask;
    }

    private static bool FilterNode(NodeDTO node, ZoneDTO zone)
    {
        return node.TotalCpu - node.UsedCpu >= zone.CpuCount &&
            node.TotalRamGB - node.UsedRamGB >= zone.RamGB &&
            node.TotalDiskGB - node.UsedDiskGB >= zone.DiskGB;
    }

    private static double ScoreNode(NodeDTO node, ZoneDTO zone)
    {
        return (node.TotalCpu - node.UsedCpu) / (double)zone.CpuCount +
                (node.TotalRamGB - node.UsedRamGB) / (double)zone.RamGB +
                (node.TotalDiskGB - node.UsedDiskGB) / (double)zone.DiskGB;
    }

    private static NodeDTO SelectNode(IEnumerable<ScoredNode> scoredNodes)
    {
        return scoredNodes.First().Node;
    }

    private record ScoredNode(NodeDTO Node, double Score)
    {
        public readonly NodeDTO Node = Node;
        public readonly double Score = Score;
    }
}
