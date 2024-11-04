namespace ChyveClient.Models;

public record TaskHandle
{
    public required string Id { get; set; }
    public required string Status { get; set; }
    public string? Result { get; set; }
}
