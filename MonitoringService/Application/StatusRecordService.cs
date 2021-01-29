using System;
using System.Threading.Tasks;
using MonitoringService.Application.Parameters;
using MonitoringService.Domain;

namespace MonitoringService.Application
{
    public class StatusRecordService : IStatusRecordService
    {
        private readonly IDockerContainerService _dockerContainerService;
        private readonly IStatusRecordRepository _statusRecordRepository;

        public StatusRecordService(IDockerContainerService dockerContainerService,
            IStatusRecordRepository statusRecordRepository)
        {
            _dockerContainerService = dockerContainerService;
            _statusRecordRepository = statusRecordRepository;
        }

        public async Task<StatusRecord> Create(CreateStatusRecordParameters parameters)
        {
            var dockerContainer =
                await _dockerContainerService.Get(new GetDockerContainerParameters(parameters.ContainerId,
                    parameters.ServerName));

            var statusRecord = new StatusRecord
            {
                Id = Guid.NewGuid(),
                DockerContainer = dockerContainer,
                Health = parameters.Health,
                State = parameters.State,
                Status = parameters.Status,
                UpdateTime = parameters.UpdateTime
            };

            await _statusRecordRepository.Create(statusRecord);
            return statusRecord;
        }
    }
}