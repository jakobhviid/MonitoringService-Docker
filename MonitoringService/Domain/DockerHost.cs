using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MonitoringService.Domain
{
    public class DockerHost
    {
        [Key] public Guid Id { get; set; }

        [Required] public string ServerName { get; set; }

        [Required] public string CommandRequestTopic { get; set; }

        [Required] public string CommandResponseTopic { get; set; }

        public ICollection<DockerContainer> DockerContainers { get; set; }
    }
}