using System;
using EventBus;
using Persistence.DTOs;

namespace Events;

public record class PlaceZoneIntegrationEvent(ZoneDTO Zone) : IntegrationEvent
{
    public ZoneDTO Zone { get; } = Zone;
}
