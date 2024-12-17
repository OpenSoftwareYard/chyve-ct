namespace Persistence.Entities;

public class Service
{
    public required string Name { get; set;}
    public required string Description { get; set; }
    public required string WorkingDir { get; set; }
    public required string Command { get; set; }
    public string[]? Arguments { get; set; }
    public required string User { get; set; }
    public Dictionary<string, string>? Environment { get; set; }
}
