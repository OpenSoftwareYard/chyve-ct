using Microsoft.AspNetCore.Mvc;
using Persistence.DTOs;
using Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonesController(IZoneService zoneService, INodeService nodeService) : ControllerBase
    {
        private readonly IZoneService _zoneService = zoneService;
        private readonly INodeService _nodeService = nodeService;

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

        [HttpPost]
        public async Task<ActionResult<ZoneDTO>> CreateZone([FromBody] ZoneDTO zone)
        {
            if (!Request.Headers.TryGetValue("UserId", out var userId))
            {
                return Unauthorized();
            }

            var createdZone = await _zoneService.CreateForUserId(zone, userId!);

            if (createdZone == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(createdZone);
        }

        [HttpPost("{zoneId}/start")]
        public async Task<ActionResult<ZoneDTO>> StartZone(Guid zoneId)
        {
            if (!Request.Headers.TryGetValue("UserId", out var userId))
            {
                return Unauthorized();
            }

            var zone = await _zoneService.GetZoneForUserId(zoneId, userId!);

            if (zone == null || zone.NodeId == null)
            {
                return NotFound();
            }

            var node = await _nodeService.GetById(zone.NodeId.GetValueOrDefault());

            if (node == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            var bootedZone = await _zoneService.BootZone(node, zone);

            return Ok(bootedZone);
        }

        [HttpPost("{zoneId}/stop")]
        public async Task<ActionResult<ZoneDTO>> StopZone(Guid zoneId)
        {
            if (!Request.Headers.TryGetValue("UserId", out var userId))
            {
                return Unauthorized();
            }

            var zone = await _zoneService.GetZoneForUserId(zoneId, userId!);

            if (zone == null || zone.NodeId == null)
            {
                return NotFound();
            }

            var node = await _nodeService.GetById(zone.NodeId.GetValueOrDefault());

            if (node == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            var stoppedZone = await _zoneService.StopZone(node, zone);

            return Ok(stoppedZone);
        }

        [HttpDelete("{zoneId}")]
        public async Task<ActionResult<ZoneDTO>> DeleteZone(Guid zoneId)
        {
            if (!Request.Headers.TryGetValue("UserId", out var userId))
            {
                return Unauthorized();
            }

            var zone = await _zoneService.GetZoneForUserId(zoneId, userId!);

            if (zone == null || zone.NodeId == null)
            {
                return NotFound();
            }

            var node = await _nodeService.GetById(zone.NodeId.GetValueOrDefault());

            if (node == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            var deletedZone = await _zoneService.DeleteZone(node, zone);

            return Ok(deletedZone);
        }
    }
}
