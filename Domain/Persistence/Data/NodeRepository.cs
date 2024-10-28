using System;
using Persistence.Entities;

namespace Persistence.Data;

public class NodeRepository(ChyveContext context) : GenericRepository<Node>(context), INodeRepository
{
    public async Task<Node> LoadZonesInNode(Node node)
    {
        await _context.Entry(node).Collection(n => n.Zones).LoadAsync();
        return node;
    }
}
