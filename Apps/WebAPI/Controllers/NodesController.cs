using ChyveClient;
using Microsoft.AspNetCore.Mvc;
using Persistence.DTOs;
using Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodesController(INodeService nodeService) : ControllerBase
    {
        private readonly INodeService _nodeService = nodeService;

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

            var zones = await Client.GetZones(node.WebApiUri, node.AccessToken);

            return Ok(zones);
        }
    }
}
