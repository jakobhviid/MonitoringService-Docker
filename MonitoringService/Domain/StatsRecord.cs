using System;
using System.ComponentModel.DataAnnotations;

namespace MonitoringService.Domain
{
    public class StatsRecord
    {
        [Required] public Guid Id { get; set; }
        [Required] public DockerContainer DockerContainer { get; set; }

        [Required] public ulong CpuUsage { get; set; }
        [Required] public int NumOfCpu { get; set; }
        [Required] public ulong SystemCpuUsage { get; set; }
        [Required] public double CpuPercentage { get; set; }
        [Required] public double MemoryPercentage { get; set; }
        [Required] public ulong NetInputBytes { get; set; }
        [Required] public ulong NetOutputBytes { get; set; }
        [Required] public ulong DiskInputBytes { get; set; }
        [Required] public ulong DiskOutputBytes { get; set; }
        [Required] public DateTime UpdateTime { get; set; }
    }
}