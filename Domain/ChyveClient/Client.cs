using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ChyveClient.Models;
using Microsoft.VisualBasic.FileIO;
using Persistence.DTOs;
using Persistence.Entities;
using Renci.SshNet;
using Zone = ChyveClient.Models.Zone;

namespace ChyveClient;

public class Client(string encryptionKey, string projectPath)
{
    public readonly string EncryptionKey = encryptionKey;
    public async Task<IEnumerable<ZoneDTO>> GetZones(NodeDTO node)
    {
        var key = await node.DecryptConnectionKey(EncryptionKey);
        var sshKeyStream = new MemoryStream(Encoding.UTF8.GetBytes(key));

        using SshClient client = new(node.Address, node.Port, node.ConnectionUser!, new PrivateKeyFile(sshKeyStream));

        var source = new CancellationTokenSource();
        await client.ConnectAsync(source.Token);

        var zones = new List<ZoneDTO>();

        using var reader = new StreamReader($"{projectPath}/Scripts/GetZones.sh");
        using var cmd = client.RunCommand(await reader.ReadToEndAsync(source.Token));
        Console.WriteLine(cmd.Result);

        var parsedZones = JsonSerializer.Deserialize<IDictionary<string, Zone>>(cmd.Result)?.Values ?? [];

        foreach (var zone in parsedZones)
        {
            if (!Guid.TryParse(zone.Name, out var parsedZoneId))
            {
                parsedZoneId = Guid.Empty;
            }

            zones.Add(new ZoneDTO()
            {
                Name = zone.Name,
                CpuCount = (int)(zone.CappedCpu?.Ncpus ?? 0),
                DiskGB = 4,
                Id = parsedZoneId,
                NodeId = node.Id,
                OrganizationId = Guid.Empty,
                RamGB = int.Parse(zone.CappedMemory?.Physical.Trim('G') ?? "0"),
                Status = ZoneStatus.SCHEDULED,
            });
        }

        return zones;
    }

    public async Task<Vnic> CreateVnic(NodeDTO node, Vnic vnic)
    {
        var key = await node.DecryptConnectionKey(EncryptionKey);
        var sshKeyStream = new MemoryStream(Encoding.UTF8.GetBytes(key));

        var source = new CancellationTokenSource();

        using SshClient client = new(node.Address, node.Port, node.ConnectionUser!, new PrivateKeyFile(sshKeyStream));

        await client.ConnectAsync(source.Token);

        using var reader = new StreamReader($"{projectPath}/Scripts/CreateVnic.sh");
        var createVnicScript = await reader.ReadToEndAsync(source.Token);

        createVnicScript = createVnicScript
            .Replace("$1", vnic.Over)
            .Replace("$2", vnic.Link);

        using var cmd = client.RunCommand(createVnicScript);
        Console.WriteLine(cmd.Result);
        Console.WriteLine(cmd.Error);

        return vnic;
    }

    public async Task<Zone> CreateZone(NodeDTO node, Zone zone)
    {
        var key = await node.DecryptConnectionKey(EncryptionKey);
        var zoneJson = JsonSerializer.Serialize(zone);

        var remotePath = $"/tmp/{zone.Name}.json";
        var source = new CancellationTokenSource();

        using var sftpKeyStream = new MemoryStream(Encoding.UTF8.GetBytes(key));

        using (SftpClient sftpClient = new(node.Address, node.Port, node.ConnectionUser!, new PrivateKeyFile(sftpKeyStream)))
        {
            await sftpClient.ConnectAsync(source.Token);

            await using (var remoteFile = sftpClient.AppendText(remotePath))
            {
                await remoteFile.WriteAsync(zoneJson);
            }
        }

        using var sshKeyStream = new MemoryStream(Encoding.UTF8.GetBytes(key));

        using SshClient client = new(node.Address, node.Port, node.ConnectionUser!, new PrivateKeyFile(sshKeyStream));

        await client.ConnectAsync(source.Token);

        using var reader = new StreamReader($"{projectPath}/Scripts/CreateZone.sh");
        var createZoneScript = await reader.ReadToEndAsync(source.Token);

        createZoneScript = createZoneScript
            .Replace("$1", zone.Brand)
            .Replace("$2", zone.Name)
            .Replace("$3", remotePath);

        using var cmd = client.RunCommand(createZoneScript);
        Console.WriteLine(cmd.Result);
        Console.WriteLine(cmd.Error);

        return zone;
    }
}
