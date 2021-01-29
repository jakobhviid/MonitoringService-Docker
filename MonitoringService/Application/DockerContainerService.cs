﻿using System;
using System.Threading.Tasks;
using MonitoringService.Application.Parameters;
using MonitoringService.Domain;

namespace MonitoringService.Application
{
    public class DockerContainerService : IDockerContainerService
    {
        private readonly IDockerContainerRepository _dockerContainerRepository;
        private readonly IDockerHostService _dockerHostService;

        public DockerContainerService(IDockerHostService dockerHostService,
            IDockerContainerRepository dockerContainerRepository)
        {
            _dockerHostService = dockerHostService;
            _dockerContainerRepository = dockerContainerRepository;
        }

        public async Task<DockerContainer> Create(CreateDockerContainerParameters parameters)
        {
            if (await _dockerContainerRepository.ContainerIdExists(parameters.ContainerId, parameters.ServerName))
            {
                throw new ArgumentException(nameof(parameters.ContainerId) + " and " + nameof(parameters.ServerName) +
                                            " combination already exists in the database!");
            }

            var dockerHost = await _dockerHostService.Get(new GetDockerHostParameters(parameters.ServerName));

            var newDockerContainer = new DockerContainer
            {
                Id = Guid.NewGuid(),
                DockerHost = dockerHost,
                ContainerId = parameters.ContainerId,
                Name = parameters.Name,
                Image = parameters.Image,
                CreationTime = DateTime.Now,
                LastUpdateTime = DateTime.Now
            };
            await _dockerContainerRepository.Create(newDockerContainer);
            return new DockerContainer();
        }

        public async Task<DockerContainer> CreateIfNotExists(CreateDockerContainerParameters parameters)
        {
            try
            {
                return await Create(parameters);
            }
            catch (ArgumentException ex)
            {
                return await Get(new GetDockerContainerParameters(parameters.ContainerId, parameters.ServerName));
            }
        }

        public async Task<DockerContainer> Get(GetDockerContainerParameters parameters)
        {
            if (await _dockerContainerRepository.ContainerIdExists(parameters.ContainerId, parameters.ServerName))
            {
                return await _dockerContainerRepository.Get(parameters.ContainerId, parameters.ServerName);
            }

            throw new ArgumentException(nameof(parameters.ContainerId) + " and " + nameof(parameters.ServerName) +
                                        " combination was not recognized in the database!");
        }
    }
}