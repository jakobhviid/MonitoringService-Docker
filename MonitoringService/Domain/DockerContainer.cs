using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MonitoringService.Domain
{
    public class DockerContainer
    {
        [Key] public Guid Id { get; set; }

        [Required] public string ContainerId { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Image { get; set; }

        [Required] public DateTime CreationTime { get; set; }

        [Required] public DateTime LastUpdateTime { get; set; }

        public DockerHost DockerHost { get; set; }
        public ICollection<StatsRecord> StatsRecords { get; set; }
        public ICollection<StatusRecord> StatusRecords { get; set; }
    }
}