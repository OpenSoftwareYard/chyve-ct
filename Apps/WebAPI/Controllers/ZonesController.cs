using Microsoft.AspNetCore.Mvc;
using Persistence.Data;
using Persistence.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonesController(ZoneRepository zoneRepository) : ControllerBase
    {
        private readonly ZoneRepository _zoneRepository = zoneRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZoneDTO>>> GetZones()
        {
            if (!Request.Headers.TryGetValue("UserId", out var userId))
            {
                return Unauthorized();
            }

            var zones = await _zoneRepository.GetZones(userId!);

            if (zones == null)
            {
                return NotFound();
            }

            return Ok(zones);
        }
    }
}
