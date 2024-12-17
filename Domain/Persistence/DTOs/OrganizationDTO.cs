namespace Persistence.DTOs;

public record class OrganizationDTO
{
    public Guid Id { get; set;}
    public required string Name { get; set; }
    public required List<string> UserIds { get; set; }
    public required List<PatDetailsDTO> PersonalAccessTokens { get; set; }
}
