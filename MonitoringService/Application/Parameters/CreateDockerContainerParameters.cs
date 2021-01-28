namespace MonitoringService.Application.Parameters
{
    public class CreateDockerContainerParameters
    {
        public CreateDockerContainerParameters(string containerId, string name, string image, string serverName)
        {
            ContainerId = containerId;
            Name = name;
            Image = image;
            ServerName = serverName;
        }

        public string ContainerId { get; }
        public string Name { get; }
        public string Image { get; }
        public string ServerName { get; }
    }
}