namespace ChyveClient.Models;

public record Vnic
{
    public required string Link { get; set; }
    public required string Over { get; set; }
    public string? Speed { get; set; }
    public string? MacAddress { get; set; }
    public string? MacAddressType { get; set; }
    public string? Vid { get; set; }
    public string? Zone { get; set; }
}
