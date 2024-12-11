namespace Persistence.Entities;

public class PersonalAccessToken : BaseEntity
{
    public required string Name { get; set; }
    public required string TokenHash { get; set; }
    public required List<PersonalAccessTokenScope> Scopes { get; set; }
    public required DateTime ExpiresAt { get; set; }

    public Organization? Organization { get; set; }
    public required Guid OrganizationId { get; set; }
}
