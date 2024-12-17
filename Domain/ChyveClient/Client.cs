using System.Text;
using System.Text.Json;
using ChyveClient.Models;
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
        using SshClient client = NewClient<SshClient>(node.Address, node.Port, node.ConnectionUser!, key);

        var source = new CancellationTokenSource();
        await client.ConnectAsync(source.Token);

        var zones = new List<ZoneDTO>();

        using var reader = new StreamReader($"{projectPath}/Scripts/GetZones.sh");

        var command = await reader.ReadToEndAsync(source.Token);
        command = command.ReplaceLineEndings("\n");

        using var cmd = client.RunCommand(command);
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
                Brand = zone.Brand,
                CpuCount = (int)(zone.CappedCpu?.Ncpus ?? 0),
                DiskGB = 4,
                Id = parsedZoneId,
                NodeId = node.Id,
                OrganizationId = Guid.Empty,
                RamGB = int.Parse(zone.CappedMemory?.Physical.Trim('G') ?? "0"),
                Status = ZoneStatus.SCHEDULED,
                ZoneServices = [],
            });
        }

        return zones;
    }

    public async Task<Zone?> GetZone(NodeDTO node, string zoneId)
    {
        var key = await node.DecryptConnectionKey(EncryptionKey);
        using SshClient client = NewClient<SshClient>(node.Address, node.Port, node.ConnectionUser!, key);

        var source = new CancellationTokenSource();
        await client.ConnectAsync(source.Token);

        using var reader = new StreamReader($"{projectPath}/Scripts/GetZone.sh");
        var getZoneScript = await reader.ReadToEndAsync(source.Token);

        getZoneScript = getZoneScript
            .Replace("$1", zoneId)
            .ReplaceLineEndings("\n");

        using var cmd = client.RunCommand(getZoneScript);
        Console.WriteLine(cmd.Result);
        Console.WriteLine(cmd.Error);

        var parsedZone = JsonSerializer.Deserialize<Zone>(cmd.Result);

        return parsedZone;
    }

    public async Task<Vnic> CreateVnic(NodeDTO node, Vnic vnic)
    {
        var key = await node.DecryptConnectionKey(EncryptionKey);
        using SshClient client = NewClient<SshClient>(node.Address, node.Port, node.ConnectionUser!, key);

        var source = new CancellationTokenSource();

        await client.ConnectAsync(source.Token);

        using var reader = new StreamReader($"{projectPath}/Scripts/CreateVnic.sh");
        var createVnicScript = await reader.ReadToEndAsync(source.Token);

        createVnicScript = createVnicScript
            .Replace("$1", vnic.Over)
            .Replace("$2", vnic.Link)
            .ReplaceLineEndings("\n");

        using var cmd = client.RunCommand(createVnicScript);
        Console.WriteLine(cmd.Result);
        Console.WriteLine(cmd.Error);

        return vnic;
    }

    public async Task<string> DeleteVnic(NodeDTO node, string vnicName)
    {
        var key = await node.DecryptConnectionKey(EncryptionKey);
        using SshClient client = NewClient<SshClient>(node.Address, node.Port, node.ConnectionUser!, key);

        var source = new CancellationTokenSource();

        await client.ConnectAsync(source.Token);

        using var reader = new StreamReader($"{projectPath}/Scripts/DeleteVnic.sh");
        var deleteVnicScript = await reader.ReadToEndAsync(source.Token);

        deleteVnicScript = deleteVnicScript
            .Replace("$1", vnicName)
            .ReplaceLineEndings("\n");

        using var cmd = client.RunCommand(deleteVnicScript);
        Console.WriteLine(cmd.Result);
        Console.WriteLine(cmd.Error);

        return vnicName;
    }

    public async Task<Zone> CreateZone(NodeDTO node, Zone zone, Uri imageUri)
    {
        var key = await node.DecryptConnectionKey(EncryptionKey);
        var zoneJson = JsonSerializer.Serialize(zone);

        var remotePath = $"/tmp/{zone.Name}.json";
        var source = new CancellationTokenSource();

        using (SftpClient sftpClient = NewClient<SftpClient>(node.Address, node.Port, node.ConnectionUser!, key))
        {
            await sftpClient.ConnectAsync(source.Token);

            await using var remoteFile = sftpClient.AppendText(remotePath);
            await remoteFile.WriteAsync(zoneJson);
        }

        using SshClient client = NewClient<SshClient>(node.Address, node.Port, node.ConnectionUser!, key);

        await client.ConnectAsync(source.Token);

        using var reader = new StreamReader($"{projectPath}/Scripts/CreateZone.sh");
        var createZoneScript = await reader.ReadToEndAsync(source.Token);

        createZoneScript = createZoneScript
            .Replace("$1", zone.Brand)
            .Replace("$2", imageUri.ToString())
            .Replace("$3", zone.Name)
            .Replace("$4", remotePath)
            .ReplaceLineEndings("\n");

        using var cmd = client.RunCommand(createZoneScript);
        Console.WriteLine(cmd.Result);
        Console.WriteLine(cmd.Error);

        return zone;
    }

    public async Task<Zone> BootZone(NodeDTO node, string zoneId)
    {
        var key = await node.DecryptConnectionKey(EncryptionKey);
        using SshClient client = NewClient<SshClient>(node.Address, node.Port, node.ConnectionUser!, key);

        var source = new CancellationTokenSource();

        await client.ConnectAsync(source.Token);

        using var reader = new StreamReader($"{projectPath}/Scripts/BootZone.sh");
        var bootZoneScript = await reader.ReadToEndAsync(source.Token);

        bootZoneScript = bootZoneScript
            .Replace("$1", zoneId)
            .ReplaceLineEndings("\n");

        using var cmd = client.RunCommand(bootZoneScript);
        Console.WriteLine(cmd.Result);
        Console.WriteLine(cmd.Error);

        var zone = await GetZone(node, zoneId);

        if (zone == null)
        {
            Console.WriteLine("Failed to retrieve zone details");
        }

        return zone!;
    }

    public async Task<Zone> StopZone(NodeDTO node, string zoneId)
    {
        var key = await node.DecryptConnectionKey(EncryptionKey);
        using SshClient client = NewClient<SshClient>(node.Address, node.Port, node.ConnectionUser!, key);

        var source = new CancellationTokenSource();

        await client.ConnectAsync(source.Token);

        using var reader = new StreamReader($"{projectPath}/Scripts/StopZone.sh");
        var stopZoneScript = await reader.ReadToEndAsync(source.Token);

        stopZoneScript = stopZoneScript
            .Replace("$1", zoneId)
            .ReplaceLineEndings("\n");

        using var cmd = client.RunCommand(stopZoneScript);
        Console.WriteLine(cmd.Result);
        Console.WriteLine(cmd.Error);

        var zone = await GetZone(node, zoneId);

        if (zone == null)
        {
            Console.WriteLine("Failed to retrieve zone details");
        }

        return zone!;
    }

    public async Task<Zone> DeleteZone(NodeDTO node, string zoneId)
    {
        await StopZone(node, zoneId);

        var zone = await GetZone(node, zoneId);

        if (zone == null)
        {
            Console.WriteLine("Failed to retrieve zone details");
        }

        var key = await node.DecryptConnectionKey(EncryptionKey);
        using SshClient client = NewClient<SshClient>(node.Address, node.Port, node.ConnectionUser!, key);

        var source = new CancellationTokenSource();

        await client.ConnectAsync(source.Token);

        using var reader = new StreamReader($"{projectPath}/Scripts/DeleteZone.sh");
        var deleteZoneScript = await reader.ReadToEndAsync(source.Token);

        deleteZoneScript = deleteZoneScript
            .Replace("$1", zoneId)
            .ReplaceLineEndings("\n");

        using var cmd = client.RunCommand(deleteZoneScript);
        Console.WriteLine(cmd.Result);
        Console.WriteLine(cmd.Error);

        return zone!;
    }

    private static T NewClient<T>(string host, int port, string user, string key) where T : BaseClient
    {
        var sshKeyStream = new MemoryStream(Encoding.UTF8.GetBytes(key));

        return (T)Activator.CreateInstance(typeof(T), [host, port, user, new PrivateKeyFile(sshKeyStream)])!;
    }
}
