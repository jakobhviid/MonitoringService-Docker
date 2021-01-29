using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonitoringService.Domain;

namespace MonitoringService.Infrastructure.Repositories
{
    public class DockerContainerRepository : IDockerContainerRepository
    {
        private readonly DockerHostContext _context;

        public DockerContainerRepository(DockerHostContext context)
        {
            _context = context;
        }

        public async Task<bool> ContainerIdExists(string containerId, string serverName)
        {
            return await _context.DockerContainers.AnyAsync(container =>
                container.ContainerId.Equals(containerId) && container.DockerHost.ServerName.Equals(serverName));
        }

        public async Task Create(DockerContainer dockerContainer)
        {
            if (dockerContainer.DockerHost == null)
            {
                throw new ArgumentNullException(nameof(dockerContainer.DockerHost));
            }

            _context.DockerContainers.Add(dockerContainer);
            await _context.SaveChangesAsync();
        }

        public async Task<DockerContainer> Get(string containerId, string serverName)
        {
            return await _context.DockerContainers.FirstOrDefaultAsync(container =>
                container.ContainerId.Equals(containerId) && container.DockerHost.ServerName.Equals(serverName));
        }
    }
}