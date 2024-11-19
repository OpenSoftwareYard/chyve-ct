using Persistence.DTOs;
using Persistence.Entities;

namespace Services
{
    public interface INodeService : IGenericService<Node, NodeDTO>
    {
        public Task<AllocateZoneResponse> AllocateZoneToOptimalNode(ZoneDTO zone);
        public Task<NodeDTO?> UpdateNodeConnectionKey(Guid nodeId, string connectionKey);
        public Task<IEnumerable<NodeDTO>> GetAllWithUsage();
    }
}
