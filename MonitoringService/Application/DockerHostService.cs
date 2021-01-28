using System;
using System.Threading.Tasks;
using MonitoringService.Application.Parameters;
using MonitoringService.Domain;

namespace MonitoringService.Application
{
    public class DockerHostService : IDockerHostService
    {
        private readonly IDockerHostRepository _dockerHostRepository;

        public DockerHostService(IDockerHostRepository dockerHostRepository)
        {
            _dockerHostRepository = dockerHostRepository;
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

            var newDockerHost = await _dockerHostRepository.Create(dockerHost);
            if (newDockerHost == null) throw new ArgumentNullException(nameof(newDockerHost));
            return newDockerHost;
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

        public async Task<DockerHost> AddDockerHostContainer(AddDockerHostContainerParameters parameters)
        {
            if (!await _dockerHostRepository.ServerNameExists(parameters.ServerName))
            {
                throw new ArgumentNullException(parameters.ServerName + "\" Docker host does not exist!");
            }

            return await _dockerHostRepository.AddContainer(parameters.ServerName, parameters.DockerContainer);
        }
    }
}