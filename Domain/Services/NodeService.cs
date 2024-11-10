using AutoMapper;
using Persistence.Data;
using Persistence.DTOs;
using Persistence.Entities;
using ChyveClient;
using static SchedulerCore.SchedulerCore;
using System.Collections.Immutable;
using ChyveClient.Models;
using Polly;
using SchedulerCore;

namespace Services
{
    public class NodeService(INodeRepository repository, IMapper mapper, ChyveClient.Client chyveClient) : GenericService<Node, NodeDTO>(repository, mapper), INodeService
    {
        private readonly INodeRepository _nodeRepository = repository;
        private readonly ChyveClient.Client _chyveClient = chyveClient;

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

                var zoneToCreate = new ChyveClient.Models.Zone
                {
                    Autoboot = "false",
                    BootArgs = "",
                    Name = zone.Id.ToString(),
                    Path = $"{selectedNode.ZoneBasePath}/{zone.Id}",
                    Brand = "pkgsrc",
                    IPType = "exclusive",
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
                            AllowedAddress = $"{availableIpAddress.ToString()}/{selectedNode.PrivateZoneNetwork.PrefixLength}",
                            DefRouter = selectedNode.DefRouter!.ToString(),
                        }
                    ],
                    FsAllowed = "",
                    HostId = "",
                    LimitPriv = "",
                    Pool = "",
                    SchedulingClass = "",
                };

                _ = await _chyveClient.CreateZone(selectedNode, zoneToCreate);

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
