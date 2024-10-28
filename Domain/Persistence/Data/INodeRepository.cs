using System;
using Persistence.Entities;

namespace Persistence.Data;

public interface INodeRepository : IGenericRepository<Node>
{
    public Task<Node> LoadZonesInNode(Node node);
}
