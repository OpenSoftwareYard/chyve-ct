using AutoMapper;
using Persistence.Data;
using Persistence.DTOs;
using Persistence.Entities;
using Security;

namespace Services;

public class PatService(IMapper mapper, IPatRepository patRepository, ITokenGenerator tokenGenerator) : IPatService
{
    private readonly IMapper _mapper = mapper;
    private readonly IPatRepository _patRepository = patRepository;
    private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
    public async Task<PatDetailsDTO> CreateToken(Guid organizationId, string name, List<PatScopeDTO> scopes)
    {
        var (token, hash) = _tokenGenerator.GenerateToken();

        var pat = new PersonalAccessToken()
        {
            OrganizationId = organizationId,
            Name = name,
            Scopes = _mapper.Map<List<PersonalAccessTokenScope>>(scopes),
            TokenHash = hash,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
        };

        var addedPat = await _patRepository.Add(pat);

        return new PatDetailsDTO
        {
            Id = addedPat.Id,
            OrganizationId = addedPat.OrganizationId,
            Scopes = _mapper.Map<List<PatScopeDTO>>(addedPat.Scopes),
            ExpiresAt = addedPat.ExpiresAt,
            CreatedAt = addedPat.CreatedAt,
            Token = token,
        };
    }

    private async Task<PersonalAccessToken?> GetValidPatFromToken(string token)
    {
        if (string.IsNullOrEmpty(token) || !token.StartsWith(TokenGenerator.TOKEN_PREFIX))
        {
            return null;
        }

        var tokenWithoutPrefix = token[TokenGenerator.TOKEN_PREFIX.Length..];
        var hash = _tokenGenerator.HashToken(tokenWithoutPrefix);

        var pat = await _patRepository.GetByTokenHash(hash);

        return pat?.ExpiresAt > DateTime.UtcNow ? pat : null;
    }

    public async Task<PatDetailsDTO> GetPatDetailsByToken(string token)
    {
        var pat = await GetValidPatFromToken(token);
        return _mapper.Map<PatDetailsDTO>(pat);
    }

    public async Task<bool> ValidateToken(string token)
    {
        var pat = await GetValidPatFromToken(token);
        return pat != null;
    }

    public async Task DeleteToken(Guid tokenId)
    {
        await _patRepository.Delete(tokenId);
    }
}
