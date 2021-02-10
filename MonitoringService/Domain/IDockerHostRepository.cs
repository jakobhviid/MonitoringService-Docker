using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitoringService.Domain
{
    public interface IDockerHostRepository
    {
        public Task<bool> ServerNameExists(string serverName);
        public Task Create(DockerHost dockerHost);

        public Task<DockerHost> Get(string serverName);

        public Task<ICollection<ContainerStatistics>> ListContainerStatistics(string serverName, DateTime periodFrom, DateTime periodTo);
    }
}