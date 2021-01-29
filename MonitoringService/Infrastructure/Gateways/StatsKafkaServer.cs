using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MonitoringService.Infrastructure.Gateways
{
    public class StatsKafkaServer
    {
        [JsonProperty("servername")]
        public string ServerName { get; set; }
        [JsonProperty("commandRequestTopic")]
        public string CommandRequestTopic { get; set; }
        [JsonProperty("commandResponseTopic")]
        public string CommandResponseTopic { get; set; }
        [JsonProperty("containers")]
        public ICollection<StatsKafkaContainer> Containers { get; set; }
    }

    public class StatsKafkaContainer
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("cpuUsage")]
        public ulong CpuUsage { get; set; }
        [JsonProperty("numOfCpu")]
        public int NumOfCpu { get; set; }
        [JsonProperty("systemCpuUsage")]
        public ulong SystemCpuUsage { get; set; }
        [JsonProperty("cpuPercentage")]
        public double CpuPercentage { get; set; }
        [JsonProperty("memoryPercentage")]
        public double MemoryPercentage { get; set; }
        [JsonProperty("netInputBytes")]
        public ulong NetInputBytes { get; set; }
        [JsonProperty("netOutputBytes")]
        public ulong NetOutputBytes { get; set; }
        [JsonProperty("diskInputBytes")]
        public ulong DiskInputBytes { get; set; }
        [JsonProperty("diskOutputBytes")]
        public ulong DiskOutputBytes { get; set; }
        [JsonProperty("updateTime")]
        public DateTime UpdateTime { get; set; }
    }
}