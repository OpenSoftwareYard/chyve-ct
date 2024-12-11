using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using Services;

namespace ApiGateway.Authentication;

public class PatAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IPatService patService
    ) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private readonly IPatService _patService = patService;

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var rawAuthHeader))
        {
            return AuthenticateResult.NoResult();
        }

        var authHeader = rawAuthHeader.ToString();

        if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.NoResult();
        }

        var token = authHeader["Bearer ".Length..].Trim();

        try
        {
            var isValid = await _patService.ValidateToken(token);

            if (!isValid)
            {
                return AuthenticateResult.Fail("Invalid or expired token");
            }

            // Get PAT details and create claims
            var pat = await _patService.GetPatDetailsByToken(token);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, pat.OrganizationId.ToString()),
                new("pat_id", pat.Id.ToString()),
                new("type", "pat"),
                new("sub", $"org|{pat.OrganizationId}")
            };

            claims.AddRange(pat.Scopes.Select(s => new Claim("scope", s.Name)));

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail(ex);
        }
    }
}
