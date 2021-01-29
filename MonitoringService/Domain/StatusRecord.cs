using System;
using System.ComponentModel.DataAnnotations;

namespace MonitoringService.Domain
{
    public class StatusRecord
    {
        [Required] public Guid Id { get; set; }

        public DockerContainer DockerContainer { get; set; }

        [Required] public string State { get; set; }

        [Required] public string Status { get; set; }

        public string Health { get; set; }

        [Required] public DateTime UpdateTime { get; set; }
    }
}