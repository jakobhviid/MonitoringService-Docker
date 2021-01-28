namespace MonitoringService.Application.Parameters
{
    public class DoesContainerIdExistParameters
    {
        public DoesContainerIdExistParameters(string containerId, string serverName)
        {
            ContainerId = containerId;
            ServerName = serverName;
        }

        public string ContainerId { get; }
        public string ServerName { get; }
    }
}