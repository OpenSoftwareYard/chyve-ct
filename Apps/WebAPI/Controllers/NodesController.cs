using ChyveClient;
using Microsoft.AspNetCore.Mvc;
using Persistence.DTOs;
using Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodesController(INodeService nodeService, IZoneService zoneService, Client chyveClient) : ControllerBase
    {
        private readonly INodeService _nodeService = nodeService;
        private readonly IZoneService _zoneService = zoneService;
        private readonly Client _chyveClient = chyveClient;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NodeDTO>>> GetNodes()
        {
            var nodes = await _nodeService.GetAll();

            return Ok(nodes);
        }

        [HttpGet("{nodeId:guid}/zones")]
        public async Task<ActionResult<IEnumerable<ZoneDTO>>> GetZonesForNode(Guid nodeId)
        {
            var node = await _nodeService.GetById(nodeId);

            if (node == null)
            {
                return NotFound();
            }

            var zones = await _zoneService.PopulateZonesForNode(node);

            return Ok(zones);
        }

        [HttpPut("{nodeId:guid}/updateKey")]
        public async Task<ActionResult<NodeDTO>> UpdateNodeKey(Guid nodeId, [FromBody] string connectionKey)
        {
            var node = await _nodeService.GetById(nodeId);

            if (node == null)
            {
                return NotFound();
            }

            // TODO: Remove client reference here, move implementation to NodeService
            node.EncryptConnectionKey(_chyveClient.EncryptionKey, connectionKey);

            var updatedNode = await _nodeService.Update(nodeId, node);

            return Ok(updatedNode);
        }
    }
}
