using AutoMapper;
using Persistence.Data;
using Persistence.DTOs;
using Persistence.Entities;
using ChyveClient;
using static SchedulerCore.SchedulerCore;
using System.Collections.Immutable;
using Polly;
using SchedulerCore;

namespace Services
{
    public class NodeService(INodeRepository repository, IMapper mapper, Client chyveClient) : GenericService<Node, NodeDTO>(repository, mapper), INodeService
    {
        private readonly Client _chyveClient = chyveClient;
        private readonly INodeRepository _nodeRepository = repository;
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

                var vnicLink = $"{zone.Name}0";

                var vnicTask = await Client.CreateVnic(selectedNode.WebApiUri, selectedNode.AccessToken, new ChyveClient.Models.Vnic
                {
                    Link = vnicLink,
                    Over = selectedNode.InternalStubDevice
                });

                var policy = Policy.Handle<Exception>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

                var vnicCreated = await policy.ExecuteAsync(async () =>
                {
                    var task = await Client.GetTaskDetails(selectedNode.WebApiUri, selectedNode.AccessToken, vnicTask);

                    return task?.Status.ToLowerInvariant() switch
                    {
                        "completed" => true,
                        "failed" => false,
                        "cancelled" => false,
                        "timedOut" => false,
                        _ => throw new Exception($"Create vnic not finished {task?.Status}")
                    };
                });

                if (!vnicCreated)
                {
                    throw new Exception($"Failed to create vnic {vnicLink}");
                }

                var zoneToCreate = new ChyveClient.Models.Zone
                {
                    Id = zone.Id.ToString(),
                    Name = zone.Name,
                    Path = $"{selectedNode.ZoneBasePath}/{zone.Id}",
                    Brand = "pkgsrc",
                    IPType = "exclusive",
                    CpuCount = zone.CpuCount,
                    PhysicalDisk = $"4G",
                    PhysicalMemory = $"{zone.RamGB}G",
                    Net = new ChyveClient.Models.Net
                    {
                        Physical = vnicLink,
                        AllowedAddress = availableIpAddress.ToString(),
                        RouterAddress = selectedNode.DefRouter!.ToString(),
                    }
                };

                var createdZoneTask = await Client.CreateZone(selectedNode.WebApiUri, selectedNode.AccessToken, zoneToCreate);

                var zoneCreated = await policy.ExecuteAsync(async () =>
                {
                    var task = await Client.GetTaskDetails(selectedNode.WebApiUri, selectedNode.AccessToken, createdZoneTask);

                    return task?.Status.ToLowerInvariant() switch
                    {
                        "completed" => true,
                        "failed" => false,
                        "cancelled" => false,
                        "timedOut" => false,
                        _ => throw new Exception($"Create zone not finished {task?.Status}")
                    };
                });

                if (!zoneCreated)
                {
                    throw new Exception($"Failed to create zone {zoneToCreate.Name}");
                }

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
