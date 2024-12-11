using Microsoft.AspNetCore.Mvc;
using Persistence.DTOs;
using Persistence.Entities;
using Services;

namespace WebAPI.Controllers
{
    [Route("api/tokens")]
    [ApiController]
    public class PersonalAccessTokenController(IPatService patService, IGenericService<Organization, OrganizationDTO> organizationService) : ControllerBase
    {
        private readonly IPatService _patService = patService;
        private readonly IGenericService<Organization, OrganizationDTO> _organizationService = organizationService; // TODO: Create a proper service for this

        [HttpPost]
        public async Task<ActionResult<PatDetailsDTO>> CreateToken(
            [FromBody] CreatePatRequestDTO request)
        {
            if (!Request.Headers.TryGetValue("UserId", out var userId) || !Request.Headers.TryGetValue("UserType", out var userType))
            {
                return Unauthorized();
            }

            List<OrganizationDTO> orgs = [];

            if (userType == "org")
            {
                orgs = [await _organizationService.GetById(Guid.Parse(userId!))];
            }
            else
            {
                orgs = (await _organizationService.GetAll()).Where(o => o.UserIds.Contains(userId!)).ToList();
            }

            if (orgs.Count == 0)
            {
                return Unauthorized();
            }

            var pat = await _patService.CreateToken(request.OrganizationId, request.Name, request.Scopes);
            return Ok(pat);
        }

        [HttpDelete("{patId}")]
        public async Task<IActionResult> RevokeToken(Guid patId)
        {
            if (!Request.Headers.TryGetValue("UserId", out var userId) || !Request.Headers.TryGetValue("UserType", out var userType))
            {
                return Unauthorized();
            }

            List<OrganizationDTO> orgs = [];

            if (userType == "org")
            {
                orgs = [await _organizationService.GetById(Guid.Parse(userId!))];
            }
            else
            {
                orgs = (await _organizationService.GetAll()).Where(o => o.UserIds.Contains(userId!) && o.PersonalAccessTokens.Select(o => o.Id).Contains(patId)).ToList();
            }

            if (orgs.Count == 0)
            {
                return Unauthorized();
            }

            await _patService.DeleteToken(patId);

            return NoContent();
        }
    }
}
