using System;
using EventBus;
using Events;

namespace Scheduler.EventHandlers;

public class PlaceZoneIntegrationEventHandler : IIntegrationEventHandler<PlaceZoneIntegrationEvent>
{
    public Task Handle(PlaceZoneIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}
