namespace Persistence.DTOs;

public record class PatDetailsDTO
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public required List<PatScopeDTO> Scopes { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string Token { get; set; }
}
