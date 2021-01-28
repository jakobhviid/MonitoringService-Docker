using MonitoringService.Domain;

namespace MonitoringService.Application.Parameters
{
    public class AddDockerHostContainerParameters
    {
        public AddDockerHostContainerParameters(string serverName, DockerContainer dockerContainer)
        {
            ServerName = serverName;
            DockerContainer = dockerContainer;
        }

        public string ServerName { get; }
        public DockerContainer DockerContainer { get; }
    }
}