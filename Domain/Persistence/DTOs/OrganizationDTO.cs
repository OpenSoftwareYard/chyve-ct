namespace Persistence.DTOs;

public record class OrganizationDTO
{
    public required string Name { get; set; }

    public required List<string> UserIds { get; set; }
    public required List<PatDetailsDTO> PersonalAccessTokens { get; set; }
}
