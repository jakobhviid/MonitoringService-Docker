using System;
using System.Threading.Tasks;
using MonitoringService.Application.Parameters;
using MonitoringService.Domain;

namespace MonitoringService.Application
{
    public class StatsRecordService
    {
        private readonly IDockerContainerService _dockerContainerService;
        private readonly IStatsRecordRepository _statsRecordRepository;

        public StatsRecordService(IDockerContainerService dockerContainerService,
            IStatsRecordRepository statsRecordRepository)
        {
            _dockerContainerService = dockerContainerService;
            _statsRecordRepository = statsRecordRepository;
        }

        public async Task<StatsRecord> Create(CreateStatsRecordParameters parameters)
        {
            var dockerContainer =
                await _dockerContainerService.Get(new GetDockerContainerParameters(parameters.ContainerId,
                    parameters.ServerName));

            var statsRecord = new StatsRecord
            {
                Id = Guid.NewGuid(),
                DockerContainer = dockerContainer,
                CpuPercentage = parameters.CpuPercentage,
                CpuUsage = parameters.CpuUsage,
                DiskInputBytes = parameters.DiskInputBytes,
                DiskOutputBytes = parameters.DiskOutputBytes,
                MemoryPercentage = parameters.MemoryPercentage,
                NetInputBytes = parameters.NetInputBytes,
                NetOutputBytes = parameters.NetOutputBytes,
                NumOfCpu = parameters.NumOfCpu,
                SystemCpuUsage = parameters.SystemCpuUsage,
                UpdateTime = parameters.UpdateTime
            };
            _statsRecordRepository.Create(statsRecord);
            return statsRecord;
        }
    }
}