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

    public string SetShortLinkName(string longLinkName)
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        var code = unchecked((uint)longLinkName.GetHashCode());

        if (code == 0)
        {
            Link = alphabet[0].ToString();
            return Link;
        }

        var baseNum = (uint)alphabet.Length;
        var result = new char[64];
        var i = 0;

        while (code > 0)
        {
            var t = code % baseNum;
            code = code / baseNum;
            result[i] = alphabet[(int)t];
            i++;
        }

        result[i] = '0';

        Link = new string(result, 0, i + 1);

        return Link;
    }
}
