using Persistence.DTOs;

namespace SchedulerCore;

public static class SchedulerCore
{
    public static bool FilterNode(NodeDTO node, ZoneDTO zone)
    {
        return node.TotalCpu - node.UsedCpu!.Value >= zone.CpuCount &&
            node.TotalRamGB - node.UsedRamGB!.Value >= zone.RamGB &&
            node.TotalDiskGB - node.UsedDiskGB!.Value >= zone.DiskGB;
    }

    public static double ScoreNode(NodeDTO node, ZoneDTO zone)
    {
        return (node.TotalCpu - node.UsedCpu!.Value) / (double)zone.CpuCount +
                (node.TotalRamGB - node.UsedRamGB!.Value) / (double)zone.RamGB +
                (node.TotalDiskGB - node.UsedDiskGB!.Value) / (double)zone.DiskGB;
    }

    public record ScoredNode(NodeDTO Node, double Score)
    {
        public readonly NodeDTO Node = Node;
        public readonly double Score = Score;
    }
}
