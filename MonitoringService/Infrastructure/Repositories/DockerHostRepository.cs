using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonitoringService.Domain;

namespace MonitoringService.Infrastructure.Repositories
{
    public class DockerHostRepository : IDockerHostRepository
    {
        private readonly DockerHostContext _context;

        public DockerHostRepository(DockerHostContext context)
        {
            _context = context;
        }

        public async Task<bool> ServerNameExists(string serverName)
        {
            return await _context.DockerHosts.AnyAsync(host => host.ServerName.Equals(serverName));
        }

        public async Task<DockerHost> Create(DockerHost dockerHost)
        {
            var newEntity = _context.DockerHosts.Add(dockerHost).Entity;
            await _context.SaveChangesAsync();
            return newEntity;
        }

        public async Task<DockerHost> Get(string serverName)
        {
            return await _context.DockerHosts.FirstOrDefaultAsync(host => host.ServerName.Equals(serverName));
        }

        public async Task<DockerHost> AddContainer(string serverName, DockerContainer container)
        {
            var dockerHost = await _context.DockerHosts.FirstAsync(host => host.ServerName.Equals(serverName));
            container.DockerHost = dockerHost;
            _context.DockerContainers.Add(container);
            await _context.SaveChangesAsync();
            return dockerHost;
        }

        public async Task<DockerHost> Get(Guid id)
        {
            return await _context.DockerHosts.FirstOrDefaultAsync(host => host.Id.Equals(id));
        }

        public async Task<ICollection<DockerHost>> List()
        {
            return await _context.DockerHosts.ToListAsync();
        }
    }
}