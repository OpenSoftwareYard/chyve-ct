namespace Persistence.DTOs;

public record class CreatePatRequestDTO
{
    public required string Name { get; set; }
    public required List<PatScopeDTO> Scopes { get; set; }
    public required Guid OrganizationId { get; set; }
}
