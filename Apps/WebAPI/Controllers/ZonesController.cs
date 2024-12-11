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
            if (!Request.Headers.TryGetValue("UserId", out var userId) || !Request.Headers.TryGetValue("UserType", out var userType))
            {
                return Unauthorized();
            }

            IEnumerable<ZoneDTO>? zones;

            if (userType == "org")
            {
                zones = await _zoneService.GetZonesForOrgId(userId!);
            }
            else
            {
                zones = await _zoneService.GetZonesForUserId(userId!);
            }

            if (zones == null)
            {
                return NotFound();
            }

            return Ok(zones);
        }

        [HttpPost]
        public async Task<ActionResult<ZoneDTO>> CreateZone([FromBody] ZoneDTO zone)
        {
            if (!Request.Headers.TryGetValue("UserId", out var userId) || !Request.Headers.TryGetValue("UserType", out var userType))
            {
                return Unauthorized();
            }

            ZoneDTO? createdZone;

            if (userType == "org")
            {
                createdZone = await _zoneService.CreateForOrgId(zone, userId!);
            }
            else
            {
                createdZone = await _zoneService.CreateForUserId(zone, userId!);
            }

            if (createdZone == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(createdZone);
        }

        [HttpPost("{zoneId}/start")]
        public async Task<ActionResult<ZoneDTO>> StartZone(Guid zoneId)
        {
            if (!Request.Headers.TryGetValue("UserId", out var userId) || !Request.Headers.TryGetValue("UserType", out var userType))
            {
                return Unauthorized();
            }

            ZoneDTO? zone;

            if (userType == "org")
            {
                zone = await _zoneService.GetZoneForOrgId(zoneId, userId!);
            }
            else
            {
                zone = await _zoneService.GetZoneForUserId(zoneId, userId!);
            }

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
            if (!Request.Headers.TryGetValue("UserId", out var userId) || !Request.Headers.TryGetValue("UserType", out var userType))
            {
                return Unauthorized();
            }

            ZoneDTO? zone;

            if (userType == "org")
            {
                zone = await _zoneService.GetZoneForOrgId(zoneId, userId!);
            }
            else
            {
                zone = await _zoneService.GetZoneForUserId(zoneId, userId!);
            }

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
            if (!Request.Headers.TryGetValue("UserId", out var userId) || !Request.Headers.TryGetValue("UserType", out var userType))
            {
                return Unauthorized();
            }

            ZoneDTO? zone;

            if (userType == "org")
            {
                zone = await _zoneService.GetZoneForOrgId(zoneId, userId!);
            }
            else
            {
                zone = await _zoneService.GetZoneForUserId(zoneId, userId!);
            }

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
