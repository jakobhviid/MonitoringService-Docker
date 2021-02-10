using System;

namespace MonitoringService.Application.Parameters
{
    public class ListContainerStatisticParameters
    {
        public string ServerName { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
    }
}