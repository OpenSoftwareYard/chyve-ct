using Persistence.DTOs;

namespace SchedulerCore;

public static class SchedulerCore
{
    public static bool FilterNode(NodeDTO node, ZoneDTO zone)
    {
        return node.TotalCpu - node.UsedCpu >= zone.CpuCount &&
            node.TotalRamGB - node.UsedRamGB >= zone.RamGB &&
            node.TotalDiskGB - node.UsedDiskGB >= zone.DiskGB;
    }

    public static double ScoreNode(NodeDTO node, ZoneDTO zone)
    {
        return (node.TotalCpu - node.UsedCpu) / (double)zone.CpuCount +
                (node.TotalRamGB - node.UsedRamGB) / (double)zone.RamGB +
                (node.TotalDiskGB - node.UsedDiskGB) / (double)zone.DiskGB;
    }

    public record ScoredNode(NodeDTO Node, double Score)
    {
        public readonly NodeDTO Node = Node;
        public readonly double Score = Score;
    }
}
