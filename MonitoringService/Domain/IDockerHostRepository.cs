using System.Threading.Tasks;

namespace MonitoringService.Domain
{
    public interface IDockerHostRepository
    {
        public Task<bool> ServerNameExists(string serverName);
        public Task<DockerHost> Create(DockerHost dockerHost);
        public Task<DockerHost> Get(string serverName);
        public Task<DockerHost> AddContainer(string serverName, DockerContainer container);
    }
}