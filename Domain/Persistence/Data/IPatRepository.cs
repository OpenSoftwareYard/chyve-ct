using System;
using Persistence.Entities;

namespace Persistence.Data;

public interface IPatRepository : IGenericRepository<PersonalAccessToken>
{
    Task<PersonalAccessToken?> GetByTokenHash(string tokenHash);
    Task<IEnumerable<PersonalAccessToken>> GetByOrganizationId(Guid organizationId);
}
