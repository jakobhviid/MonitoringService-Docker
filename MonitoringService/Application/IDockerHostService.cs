using System.Collections.Generic;
using System.Threading.Tasks;
using MonitoringService.Application.Parameters;
using MonitoringService.Application.Results;
using MonitoringService.Domain;

namespace MonitoringService.Application
{
    public interface IDockerHostService
    {
        public Task<DockerHost> Create(CreateDockerHostParameters parameters);
        public Task<DockerHost> CreateIfNotExists(CreateDockerHostParameters parameters);

        public Task<DockerHost> Get(GetDockerHostParameters parameters);
        public Task<ICollection<ListContainerStatisticResult>> ListContainerStatistic(ListContainerStatisticParameters parameters);
    }
}