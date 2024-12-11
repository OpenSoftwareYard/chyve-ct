using System;

namespace Persistence.Entities;

public class PersonalAccessTokenScope : BaseEntity
{
    public required string Name { get; set; }

    public required List<PersonalAccessToken> PersonalAccessTokens { get; set; }
}
