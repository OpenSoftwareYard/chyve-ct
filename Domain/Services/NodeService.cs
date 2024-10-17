using AutoMapper;
using Persistence.Data;
using Persistence.DTOs;
using Persistence.Entities;
using static SchedulerCore.SchedulerCore;

namespace Services
{
    public class NodeService(IGenericRepository<Node> repository, IMapper mapper) : GenericService<Node, NodeDTO>(repository, mapper), INodeService
    {
        public async Task<NodeDTO> AllocateZoneToOptimalNode(ZoneDTO zone)
        {
            await _repository.BeginTransaction();
            try
            {
                var nodes = await GetAll() ?? throw new Exception("No nodes configured to schedule zones");

                var selectedNode = nodes
                    .Where(node => FilterNode(node, zone))
                    .Select(node => new ScoredNode(node, ScoreNode(node, zone)))
                    .OrderByDescending(sn => sn.Score)
                    .Select(sn => sn.Node)
                    .FirstOrDefault() ?? throw new Exception($"Failed to schedule zone {zone.Id}");

                selectedNode.UsedCpu += zone.CpuCount;
                selectedNode.UsedDiskGB += zone.DiskGB;
                selectedNode.UsedRamGB += zone.RamGB;

                await Update(selectedNode.Id, selectedNode);

                await _repository.CommitTransaction();
                return selectedNode;
            }
            catch
            {
                await _repository.RollbackTransaction();
                throw;
            }
        }
    }
}
