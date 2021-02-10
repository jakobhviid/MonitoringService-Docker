using System;
using System.Collections.Generic;
using MonitoringService.Domain;

namespace MonitoringService.Application.Results
{
    public class ListContainerStatisticResult
    {
        public DockerContainer DockerContainer { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
        public Dictionary<string, int> HealthCounts { get; set; }
        public Dictionary<string, int> StateCounts { get; set; }
        public double? HealthyPercentage { get; set; }
    }
}