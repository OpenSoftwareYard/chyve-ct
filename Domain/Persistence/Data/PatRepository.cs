using System;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Data;

public class PatRepository(ChyveContext chyveContext) : GenericRepository<PersonalAccessToken>(chyveContext), IPatRepository
{
    private readonly ChyveContext _chyveContext = chyveContext;

    public async Task<PersonalAccessToken?> GetByTokenHash(string tokenHash)
    {
        return await _chyveContext.PersonalAccessTokens.FirstOrDefaultAsync(t => t.TokenHash == tokenHash);
    }

    public async Task<IEnumerable<PersonalAccessToken>> GetByOrganizationId(Guid organizationId)
    {
        return await _chyveContext.PersonalAccessTokens.Where(t => t.OrganizationId == organizationId).ToListAsync();
    }
}
