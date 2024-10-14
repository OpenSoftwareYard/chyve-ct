using System.Text.Json.Serialization;

namespace EventBus;

public record class IntegrationEvent
{
    [JsonInclude]
    public Guid Id { get; private init; }
    [JsonInclude]
    public DateTime CreationDate { get; private init; }

    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    [JsonConstructor]
    public IntegrationEvent(Guid id, DateTime creationDate)
    {
        Id = id;
        CreationDate = creationDate;
    }
}
