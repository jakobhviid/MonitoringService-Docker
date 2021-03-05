using System.Threading.Tasks;

namespace MonitoringService.Domain
{
    public interface IDockerContainerRepository
    {
        public Task<bool> ContainerIdExists(string containerId, string serverName);
        public Task Create(DockerContainer dockerContainer);
        public Task<DockerContainer> Get(string containerId, string serverName);
    }
}