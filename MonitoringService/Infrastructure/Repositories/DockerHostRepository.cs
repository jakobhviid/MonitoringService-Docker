using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ICollection<ContainerStatistics>> ListContainerStatistics(string serverName, DateTime periodFrom, DateTime periodTo)
        {
            var containerHealthList = await _context.StatusRecords
                .Include(sr => sr.DockerContainer)
                .Where(sr => sr.DockerContainer.DockerHost.ServerName.Equals(serverName))
                .Where(sr => sr.Health != null)
                .Where(sr => sr.UpdateTime > periodFrom && sr.UpdateTime < periodTo)
                .GroupBy(sr => new {sr.DockerContainer.Id, sr.Health})
                .Select(sr => new {sr.Key, Count = sr.Count()})
                .ToListAsync();
            
            var containerStateList = await _context.StatusRecords
                .Include(sr => sr.DockerContainer)
                .Where(sr => sr.DockerContainer.DockerHost.ServerName.Equals(serverName))
                .Where(sr => sr.State != null)
                .Where(sr => sr.UpdateTime > periodFrom && sr.UpdateTime < periodTo)
                .GroupBy(sr => new {sr.DockerContainer.Id, sr.State})
                .Select(sr => new {sr.Key, Count = sr.Count()})
                .ToListAsync();

            var returnDictionary = new Dictionary<Guid, ContainerStatistics>();
            foreach (var healthCount in containerHealthList)
            {
                if (!returnDictionary.ContainsKey(healthCount.Key.Id))
                {
                    returnDictionary.Add(healthCount.Key.Id, new ContainerStatistics());
                    returnDictionary[healthCount.Key.Id].DockerContainerId = healthCount.Key.Id;
                    returnDictionary[healthCount.Key.Id].HealthCounts = new Dictionary<string, int>();
                    returnDictionary[healthCount.Key.Id].StateCounts = new Dictionary<string, int>();
                }

                returnDictionary[healthCount.Key.Id].HealthCounts.Add(healthCount.Key.Health, healthCount.Count);
            }
            foreach (var stateCount in containerStateList)
            {
                if (!returnDictionary.ContainsKey(stateCount.Key.Id))
                {
                    returnDictionary.Add(stateCount.Key.Id, new ContainerStatistics());
                    returnDictionary[stateCount.Key.Id].DockerContainerId = stateCount.Key.Id;
                    returnDictionary[stateCount.Key.Id].HealthCounts = new Dictionary<string, int>();
                    returnDictionary[stateCount.Key.Id].StateCounts = new Dictionary<string, int>();
                }

                returnDictionary[stateCount.Key.Id].StateCounts.Add(stateCount.Key.State, stateCount.Count);
            }
            
            return returnDictionary.Values;
        }
    }
}