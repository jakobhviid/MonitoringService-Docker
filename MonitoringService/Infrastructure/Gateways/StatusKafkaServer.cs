using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MonitoringService.Infrastructure.Gateways
{
    public class StatusKafkaServer
    {
        [JsonProperty("servername")] public string ServerName { get; set; }
        [JsonProperty("commandRequestTopic")] public string CommandRequestTopic { get; set; }
        [JsonProperty("commandResponseTopic")] public string CommandResponseTopic { get; set; }
        [JsonProperty("containers")] public ICollection<StatusKafkaContainer> Containers { get; set; }
    }

    public class StatusKafkaContainer
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("image")] public string Image { get; set; }
        [JsonProperty("state")] public string State { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("health")] public string Health { get; set; }
        [JsonProperty("creationTime")] public DateTime CreationTime { get; set; }
        [JsonProperty("updateTime")] public DateTime UpdateTime { get; set; }
    }
}