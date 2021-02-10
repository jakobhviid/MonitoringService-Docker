using System;
using System.Collections.Generic;

namespace MonitoringService.Domain
{
    public class ContainerStatistics
    {
        public Guid DockerContainerId { get; set; }
        public Dictionary<string, int> HealthCounts { get; set; }
        public Dictionary<string, int> StateCounts { get; set; }
    }
}