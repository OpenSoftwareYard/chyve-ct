using System;
using Persistence.DTOs;

namespace Services;

public interface IPatService
{
    public Task<PatDetailsDTO> CreateToken(Guid organizationId, string name, List<PatScopeDTO> scopes);
    public Task<bool> ValidateToken(string token);
    public Task<PatDetailsDTO> GetPatDetailsByToken(string token);
    public Task DeleteToken(Guid tokenId);
}
