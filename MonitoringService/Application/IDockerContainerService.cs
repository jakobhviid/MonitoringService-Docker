using System.Threading.Tasks;
using MonitoringService.Application.Parameters;
using MonitoringService.Domain;

namespace MonitoringService.Application
{
    public interface IDockerContainerService
    {
        public Task<DockerContainer> Create(CreateDockerContainerParameters parameters);
        public Task<DockerContainer> CreateIfNotExists(CreateDockerContainerParameters parameters);
        public Task<DockerContainer> Get(GetDockerContainerParameters parameters);
    }
}