using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace Persistence.DTOs;

public record class NodeDTO
{
    public Guid Id { get; set; }
    public required string Address { get; set; }
    [JsonIgnore]
    public byte[]? ConnectionKey { get; set; }
    [JsonIgnore]
    public string? ConnectionUser { get; set; }
    public int TotalCpu { get; set; }
    public int UsedCpu { get; set; }
    public int TotalRamGB { get; set; }
    public int UsedRamGB { get; set; }
    public int TotalDiskGB { get; set; }
    public int UsedDiskGB { get; set; }
    public required List<ZoneDTO> Zones { get; set; }
    [JsonIgnore]
    public IPNetwork PrivateZoneNetwork { get; set; }
    [JsonIgnore]
    public IPAddress? DefRouter { get; set; }
    public required string ExternalNetworkDevice { get; set; }
    public required string InternalStubDevice { get; set; }
    public required string ZoneBasePath { get; set; }

    public async Task<string> DecryptConnectionKey(string key)
    {
        var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);

        var iv = new byte[aes.BlockSize / 8];
        var cipherText = new byte[ConnectionKey!.Length - iv.Length];

        Array.Copy(ConnectionKey, iv, iv.Length);
        Array.Copy(ConnectionKey, iv.Length, cipherText, 0, cipherText.Length);

        aes.IV = iv;
        aes.Mode = CipherMode.CBC;

        var decipher = aes.CreateDecryptor(aes.Key, aes.IV);

        using MemoryStream ms = new(cipherText);
        await using CryptoStream cs = new(ms, decipher, CryptoStreamMode.Read);
        using StreamReader sr = new(cs);

        return await sr.ReadToEndAsync();
    }

    public byte[] EncryptConnectionKey(string key, string connectionKey)
    {
        byte[] cipherData;
        var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.GenerateIV();

        aes.Mode = CipherMode.CBC;
        var cipher = aes.CreateEncryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new())
        {
            using (CryptoStream cs = new(ms, cipher, CryptoStreamMode.Write))
            {
                using StreamWriter sw = new(cs);
                sw.Write(connectionKey);
            }

            cipherData = ms.ToArray();
        }

        var combinedData = new byte[aes.IV.Length + cipherData.Length];
        Array.Copy(aes.IV, 0, combinedData, 0, aes.IV.Length);
        Array.Copy(cipherData, 0, combinedData, aes.IV.Length, cipherData.Length);

        ConnectionKey = combinedData;

        return combinedData;
    }
}
