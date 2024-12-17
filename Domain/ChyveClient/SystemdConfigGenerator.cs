using Persistence.Entities;

namespace ChyveClient;

public class SystemdConfigGenerator(string projectPath) : IServiceConfigGenerator
{
    public async Task<string> GenerateConfig(ZoneService service)
    {
        using var reader = new StreamReader($"{projectPath}/Templates/systemd-unit.service");
        var systemdUnit = await reader.ReadToEndAsync();

        var command = service.Command;
        if (service.Arguments?.Length > 0)
        {
            command += " " + string.Join(" ", service.Arguments);
        }

        var envs = service.Environment?.Select(entry => string.Format("Environment=\"{0}={1}\"", entry.Key, entry.Value));
        var envsString = string.Join('\n', envs ?? []);

        systemdUnit = systemdUnit
            .Replace("$1", service.Description)
            .Replace("$2", service.User)
            .Replace("$3", service.WorkingDir)
            .Replace("$4", command)
            .Replace("$5", envsString)
            .ReplaceLineEndings("\n");

        return systemdUnit;
    }
}
