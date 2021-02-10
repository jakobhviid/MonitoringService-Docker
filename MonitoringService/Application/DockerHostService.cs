using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonitoringService.Application.Parameters;
using MonitoringService.Application.Results;
using MonitoringService.Domain;

namespace MonitoringService.Application
{
    public class DockerHostService : IDockerHostService
    {
        private readonly IDockerHostRepository _dockerHostRepository;
        private readonly IDockerContainerRepository _dockerContainerRepository;

        public DockerHostService(IDockerHostRepository dockerHostRepository, IDockerContainerRepository dockerContainerRepository)
        {
            _dockerHostRepository = dockerHostRepository;
            _dockerContainerRepository = dockerContainerRepository;
        }

        public async Task<DockerHost> Create(CreateDockerHostParameters parameters)
        {
            if (await _dockerHostRepository.ServerNameExists(parameters.ServerName))
            {
                throw new ArgumentException("Docker host \"" + parameters.ServerName + "\" already exists!");
            }

            var dockerHost = new DockerHost
            {
                Id = Guid.NewGuid(),
                ServerName = parameters.ServerName,
                CommandRequestTopic = parameters.CommandRequestTopic,
                CommandResponseTopic = parameters.CommandResponseTopic
            };

            await _dockerHostRepository.Create(dockerHost);
            return dockerHost;
        }

        public async Task<DockerHost> CreateIfNotExists(CreateDockerHostParameters parameters)
        {
            try
            {
                return await Create(parameters);
            }
            catch (ArgumentException ex)
            {
                return await Get(new GetDockerHostParameters(parameters.ServerName));
            }
        }

        public async Task<DockerHost> Get(GetDockerHostParameters parameters)
        {
            var dockerHost = await _dockerHostRepository.Get(parameters.ServerName);
            if (dockerHost == null)
                throw new ArgumentNullException(nameof(dockerHost) + " \"" + parameters.ServerName +
                                                "\" does not exist!");
            return dockerHost;
        }

        public async Task<ICollection<ListContainerStatisticResult>> ListContainerStatistic(ListContainerStatisticParameters parameters)
        {
            var containerStatistics = await _dockerHostRepository.ListContainerStatistics(parameters.ServerName,
                parameters.PeriodFrom ?? DateTime.UnixEpoch, parameters.PeriodTo ?? DateTime.MaxValue);
            
            var result = new List<ListContainerStatisticResult>();
            foreach (var containerStatistic in containerStatistics)
            {
                double? healthyPercentage = null;
                if (containerStatistic.HealthCounts.Count != 0)
                {
                    var totalHealthCountSum = containerStatistic.HealthCounts.Sum(healthCounts => healthCounts.Value);
                    var healthyCount = containerStatistic.HealthCounts["healthy"];
                    healthyPercentage = (healthyCount * 1.0) / totalHealthCountSum; // The "* 1.0" converts int to double
                }
                
                result.Add(new ListContainerStatisticResult
                {
                    DockerContainer = await _dockerContainerRepository.Get(containerStatistic.DockerContainerId),
                    HealthCounts = containerStatistic.HealthCounts,
                    StateCounts = containerStatistic.StateCounts,
                    HealthyPercentage = healthyPercentage,
                    PeriodFrom = parameters.PeriodFrom ?? DateTime.UnixEpoch,
                    PeriodTo = parameters.PeriodTo ?? DateTime.MaxValue
                });
            }
            return result;
        }
    }
}