using System;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Data;

public class NodeRepository(ChyveContext context) : GenericRepository<Node>(context), INodeRepository
{
    public async Task<int> GetUsedCPU(Node node)
    {
        return await _context.Zones.Where(z => z.NodeId == node.Id).SumAsync(z => z.CpuCount);
    }

    public async Task<int> GetUsedDisk(Node node)
    {
        return await _context.Zones.Where(z => z.NodeId == node.Id).SumAsync(z => z.DiskGB);
    }

    public async Task<int> GetUsedRAM(Node node)
    {
        return await _context.Zones.Where(z => z.NodeId == node.Id).SumAsync(z => z.RamGB);
    }

    public async Task<Node> LoadZonesInNode(Node node)
    {
        await _context.Entry(node).Collection(n => n.Zones).LoadAsync();
        return node;
    }
}
