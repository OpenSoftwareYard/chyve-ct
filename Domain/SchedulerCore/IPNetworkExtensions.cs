using System.Collections.Immutable;
using System.Net;

namespace SchedulerCore;

public static class IPNetworkExtensions
{
    public static IPAddress GetNextAvailableAddress(this IPNetwork network, ImmutableHashSet<IPAddress> usedAddresses)
    {
        byte[] firstAddressBytes = network.BaseAddress.GetAddressBytes();
        int prefixLength = network.PrefixLength;

        byte[] lastAddressBytes = network.BaseAddress.GetAddressBytes();
        for (int i = prefixLength; i < lastAddressBytes.Length * 8; i++)
        {
            int byteIndex = i / 8;
            int bitIndex = 7 - (i % 8);
            lastAddressBytes[byteIndex] |= (byte)(1 << bitIndex);
        }

        byte[] currentBytes = [.. firstAddressBytes];
        IncrementIPAddress(currentBytes);
        IncrementIPAddress(currentBytes);

        while (CompareBytes(currentBytes, lastAddressBytes) < 0)
        {
            IPAddress currentAddress = new IPAddress(currentBytes);

            if (!usedAddresses.Contains(currentAddress))
            {
                return currentAddress;
            }

            IncrementIPAddress(currentBytes);
        }

        throw new InvalidOperationException("No available IP addresses in the network");
    }

    private static void IncrementIPAddress(byte[] address)
    {
        for (int i = address.Length - 1; i >= 0; i--)
        {
            if (address[i] == 255)
            {
                address[i] = 0;
            }
            else
            {
                address[i]++;
                break;
            }
        }
    }

    private static int CompareBytes(byte[] a, byte[] b)
    {
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
            {
                return a[i].CompareTo(b[i]);
            }
        }

        return 0;
    }
}
