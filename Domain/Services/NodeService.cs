using AutoMapper;
using Persistence.Data;
using Persistence.DTOs;
using Persistence.Entities;
using static SchedulerCore.SchedulerCore;
using System.Collections.Immutable;
using ChyveClient.Models;
using SchedulerCore;
using System.Net;

namespace Services
{
    public class NodeService(INodeRepository repository, IMapper mapper, ChyveClient.Client chyveClient) : GenericService<Node, NodeDTO>(repository, mapper), INodeService
    {
        private readonly INodeRepository _nodeRepository = repository;
        private readonly ChyveClient.Client _chyveClient = chyveClient;

        public async Task<AllocateZoneResponse> AllocateZoneToOptimalNode(ZoneDTO zone)
        {
            await _repository.BeginTransaction();
            try
            {
                var nodes = await GetAllWithUsage() ?? throw new Exception("No nodes configured to schedule zones");

                var selectedNode = nodes
                    .Where(node => FilterNode(node, zone))
                    .Select(node => new ScoredNode(node, ScoreNode(node, zone)))
                    .OrderByDescending(sn => sn.Score)
                    .Select(sn => sn.Node)
                    .FirstOrDefault() ?? throw new Exception($"Failed to schedule zone {zone.Id}");

                selectedNode.UsedCpu += zone.CpuCount;
                selectedNode.UsedDiskGB += zone.DiskGB;
                selectedNode.UsedRamGB += zone.RamGB;

                var efNode = await _nodeRepository.LoadZonesInNode(_mapper.Map<Node>(selectedNode));
                var usedAddresses = efNode.Zones.Where(z => z.InternalIPAddress != null).Select(z => z.InternalIPAddress!).ToImmutableHashSet();

                var availableIpAddress = selectedNode.PrivateZoneNetwork.GetNextAvailableAddress(usedAddresses);

                var vnic = new Vnic()
                {
                    Link = zone.Id.ToString(),
                    Over = selectedNode.InternalStubDevice,
                };

                vnic.SetShortLinkName(zone.Id.ToString());

                var createdVnic = await _chyveClient.CreateVnic(selectedNode, vnic);

                var zonePath = $"{selectedNode.ZoneBasePath}/{zone.Id}";
                var brand = "pkgsrc";
                var ipType = "exclusive";

                var zoneToCreate = new ChyveClient.Models.Zone
                {
                    Autoboot = "false",
                    BootArgs = "",
                    Name = zone.Id.ToString(),
                    Path = zonePath,
                    Brand = brand,
                    IPType = ipType,
                    CappedCpu = new CappedCpu()
                    {
                        Ncpus = zone.CpuCount,
                    },
                    CappedMemory = new CappedMemory()
                    {
                        Physical = $"{zone.RamGB}G",
                    },
                    Net =
                    [
                        new Net()
                        {
                            Physical = createdVnic.Link,
                            AllowedAddress = $"{availableIpAddress}/{selectedNode.PrivateZoneNetwork.PrefixLength}",
                            DefRouter = selectedNode.DefRouter!.ToString(),
                        }
                    ],
                    FsAllowed = "",
                    HostId = "",
                    LimitPriv = "",
                    Pool = "",
                    SchedulingClass = "",
                    Resolvers = ["1.1.1.1", "1.0.0.1"],
                };

                _ = await _chyveClient.CreateZone(selectedNode, zoneToCreate);

                await _repository.CommitTransaction();
                return new AllocateZoneResponse
                {
                    Node = selectedNode,
                    IpAddress = availableIpAddress,
                    VnicName = createdVnic.Link,
                    Path = zonePath,
                    Brand = brand,
                    IPType = ipType,
                };
            }
            catch
            {
                await _repository.RollbackTransaction();
                throw;
            }
        }

        public async Task<IEnumerable<NodeDTO>> GetAllWithUsage()
        {
            var nodes = await _nodeRepository.GetAll();
            var nodesWithCPU = await Task.WhenAll(
                nodes.Select(async n =>
                {
                    var nodeDTO = _mapper.Map<NodeDTO>(n);

                    return nodeDTO with
                    {
                        UsedCpu = await _nodeRepository.GetUsedCPU(n),
                        UsedRamGB = await _nodeRepository.GetUsedRAM(n),
                        UsedDiskGB = await _nodeRepository.GetUsedDisk(n),
                    };
                })
            );

            return nodesWithCPU;
        }

        public async Task<NodeDTO?> UpdateNodeConnectionKey(Guid nodeId, string connectionKey)
        {
            var node = await GetById(nodeId);

            if (node == null)
            {
                return null;
            }

            node.EncryptConnectionKey(_chyveClient.EncryptionKey, connectionKey);

            var updatedNode = await Update(node.Id, node);

            return updatedNode;
        }
    }

    public record AllocateZoneResponse
    {
        public required NodeDTO Node { get; set; }
        public required IPAddress IpAddress { get; set; }
        public required string VnicName { get; set; }
        public required string Path { get; set; }
        public required string Brand { get; set; }
        public required string IPType { get; set; }
    }
}
