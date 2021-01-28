using System;

namespace MonitoringService.Application.Parameters
{
    public class CreateStatsRecordParameters
    {
        public CreateStatsRecordParameters(string containerId, string serverName, ulong cpuUsage, int numOfCpu,
            ulong systemCpuUsage, double cpuPercentage, double memoryPercentage, ulong netInputBytes,
            ulong netOutputBytes, ulong diskInputBytes, ulong diskOutputBytes, DateTime updateTime)
        {
            ContainerId = containerId;
            ServerName = serverName;
            CpuUsage = cpuUsage;
            NumOfCpu = numOfCpu;
            SystemCpuUsage = systemCpuUsage;
            CpuPercentage = cpuPercentage;
            MemoryPercentage = memoryPercentage;
            NetInputBytes = netInputBytes;
            NetOutputBytes = netOutputBytes;
            DiskInputBytes = diskInputBytes;
            DiskOutputBytes = diskOutputBytes;
            UpdateTime = updateTime;
        }

        public string ContainerId { get; }
        public string ServerName { get; }
        public ulong CpuUsage { get; }
        public int NumOfCpu { get; }
        public ulong SystemCpuUsage { get; }
        public double CpuPercentage { get; }
        public double MemoryPercentage { get; }
        public ulong NetInputBytes { get; }
        public ulong NetOutputBytes { get; }
        public ulong DiskInputBytes { get; }
        public ulong DiskOutputBytes { get; }
        public DateTime UpdateTime { get; }
    }
}