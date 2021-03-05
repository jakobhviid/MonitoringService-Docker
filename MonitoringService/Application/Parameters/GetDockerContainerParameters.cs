namespace MonitoringService.Application.Parameters
{
    public class GetDockerContainerParameters
    {
        public GetDockerContainerParameters(string containerId, string serverName)
        {
            ContainerId = containerId;
            ServerName = serverName;
        }

        public string ContainerId { get; }
        public string ServerName { get; }
    }
}