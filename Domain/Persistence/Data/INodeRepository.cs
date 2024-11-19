using System;
using Persistence.Entities;

namespace Persistence.Data;

public interface INodeRepository : IGenericRepository<Node>
{
    public Task<Node> LoadZonesInNode(Node node);
    public Task<int> GetUsedCPU(Node node);
    public Task<int> GetUsedRAM(Node node);
    public Task<int> GetUsedDisk(Node node);
}
