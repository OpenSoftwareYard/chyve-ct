using Microsoft.AspNetCore.Mvc;
using Persistence.DTOs;
using Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonesController(IZoneService zoneService) : ControllerBase
    {
        private readonly IZoneService _zoneService = zoneService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZoneDTO>>> GetZones()
        {
            if (!Request.Headers.TryGetValue("UserId", out var userId))
            {
                return Unauthorized();
            }

            var zones = await _zoneService.GetZonesForUserId(userId!);

            if (zones == null)
            {
                return NotFound();
            }

            return Ok(zones);
        }
    }
}
