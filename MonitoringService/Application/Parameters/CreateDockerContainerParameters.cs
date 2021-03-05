using System;

namespace MonitoringService.Application.Parameters
{
    public class CreateDockerContainerParameters
    {
        public CreateDockerContainerParameters(string containerId, string name, string image, string serverName,
            DateTime creationTime)
        {
            ContainerId = containerId;
            Name = name;
            Image = image;
            ServerName = serverName;
            CreationTime = creationTime;
        }

        public string ContainerId { get; }
        public string Name { get; }
        public string Image { get; }
        public string ServerName { get; }
        public DateTime CreationTime { get; }
    }
}