namespace Persistence.Entities
{
    public class Organization : BaseEntity
    {
        public required string Name { get; set; }

        public required List<string> UserIds { get; set; }
        public required List<Zone> Zones { get; set; }
        public required List<PersonalAccessToken> PersonalAccessTokens { get; set; }
    }
}
