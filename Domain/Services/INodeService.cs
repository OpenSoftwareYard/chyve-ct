using Persistence.DTOs;
using Persistence.Entities;

namespace Services
{
    public interface INodeService : IGenericService<Node, NodeDTO>
    {
        public Task<NodeDTO> AllocateZoneToOptimalNode(ZoneDTO zone);
        public Task<NodeDTO?> UpdateNodeConnectionKey(Guid nodeId, string connectionKey);
    }
}
