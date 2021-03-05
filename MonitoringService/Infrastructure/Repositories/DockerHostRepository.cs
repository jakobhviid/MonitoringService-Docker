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

        public async Task Create(DockerHost dockerHost)
        {
            _context.DockerHosts.Add(dockerHost);
            await _context.SaveChangesAsync();
        }

        public async Task<DockerHost> Get(string serverName)
        {
            return await _context.DockerHosts.FirstOrDefaultAsync(host => host.ServerName.Equals(serverName));
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