using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MonitoringService.Application;
using MonitoringService.Application.Parameters;
using Newtonsoft.Json;

namespace MonitoringService.Infrastructure.Gateways
{
    public class KafkaConsumerBackgroundService : BackgroundService
    {
        public const string OverviewTopic = "f0e1e946-50d0-4a2b-b1a5-f21b92e09ac1-general_info";
        public const string StatsTopic = "33a325ce-b0c0-43a7-a846-4f46acdb367e-stats_info";

        public static readonly string BootstrapServers =
            Environment.GetEnvironmentVariable("MONITORING_KAFKA_URL") ??
            "kafka1.cfei.dk:9092,kafka2.cfei.dk:9092,kafka3.cfei.dk:9092";

        private readonly ILogger<KafkaConsumerBackgroundService> _logger;
        private readonly IServiceProvider _services;

        public KafkaConsumerBackgroundService(ILogger<KafkaConsumerBackgroundService> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumerConfig = new ConsumerConfig
            {
                GroupId = "monitoring-service-consumer",
                BootstrapServers = BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Latest,
            };

            using (var c = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
            {
                c.Subscribe(new List<string>
                    {OverviewTopic, StatsTopic});

                _logger.LogInformation("Listening for updates");
                while (!stoppingToken.IsCancellationRequested)
                {
                    // consumer does not have an async method. So it is wrapped in a task, so that the rest of the application doesn't hang here
                    var consumeResult = await Task.Factory.StartNew(() => c.Consume(stoppingToken));
                    switch (consumeResult.Topic)
                    {
                        case OverviewTopic:
                            var statusResult =
                                JsonConvert.DeserializeObject<StatusKafkaServer>(consumeResult.Message.Value);
                            await ConsumeStatusResult(statusResult);
                            break;
                        case StatsTopic:
                            var statsResult =
                                JsonConvert.DeserializeObject<StatsKafkaServer>(consumeResult.Message.Value);
                            await ConsumeStatsResult(statsResult);
                            break;
                    }
                }

                c.Close();
            }
        }

        private async Task ConsumeStatusResult(StatusKafkaServer statusResult)
        {
            using var scope = _services.CreateScope();

            var dockerHostService = scope.ServiceProvider.GetRequiredService<IDockerHostService>();
            var dockerContainerService = scope.ServiceProvider.GetRequiredService<IDockerContainerService>();
            var statusRecordService = scope.ServiceProvider.GetRequiredService<IStatusRecordService>();

            var dockerHost = await dockerHostService.CreateIfNotExists(new CreateDockerHostParameters(
                statusResult.ServerName, statusResult.CommandRequestTopic,
                statusResult.CommandResponseTopic));

            foreach (var statusResultContainer in statusResult.Containers)
            {
                try
                {
                    var container = await dockerContainerService.CreateIfNotExists(
                        new CreateDockerContainerParameters(statusResultContainer.Id,
                            statusResultContainer.Name, statusResultContainer.Image,
                            statusResult.ServerName, statusResultContainer.CreationTime));
                    await statusRecordService.Create(new CreateStatusRecordParameters(
                        statusResultContainer.Id, statusResult.ServerName, statusResultContainer.State,
                        statusResultContainer.Status, statusResultContainer.Health,
                        statusResultContainer.UpdateTime));
                }
                catch (DbUpdateException ex)
                {
                    Console.Write("Ignoring status row for " + statusResultContainer.Id);
                }
            }
        }

        private async Task ConsumeStatsResult(StatsKafkaServer statsResult)
        {
            using var scope = _services.CreateScope();

            var dockerHostService = scope.ServiceProvider.GetRequiredService<IDockerHostService>();
            var dockerContainerService = scope.ServiceProvider.GetRequiredService<IDockerContainerService>();
            var statsRecordService = scope.ServiceProvider.GetRequiredService<IStatsRecordService>();

            var dockerHost = await dockerHostService.CreateIfNotExists(new CreateDockerHostParameters(
                statsResult.ServerName, statsResult.CommandRequestTopic,
                statsResult.CommandResponseTopic));

            foreach (var statsResultContainer in statsResult.Containers)
            {
                try
                {
                    var container = await dockerContainerService.CreateIfNotExists(
                        new CreateDockerContainerParameters(statsResultContainer.Id,
                            statsResultContainer.Name, statsResultContainer.Image,
                            statsResult.ServerName, DateTime.UnixEpoch));

                    await statsRecordService.Create(new CreateStatsRecordParameters(statsResultContainer.Id,
                        statsResult.ServerName, statsResultContainer.CpuUsage, statsResultContainer.NumOfCpu,
                        statsResultContainer.SystemCpuUsage, statsResultContainer.CpuPercentage,
                        statsResultContainer.MemoryPercentage, statsResultContainer.NetInputBytes,
                        statsResultContainer.NetOutputBytes, statsResultContainer.DiskInputBytes,
                        statsResultContainer.DiskOutputBytes, statsResultContainer.UpdateTime));
                }
                catch (DbUpdateException ex)
                {
                    Console.Write("Ignoring status row for " + statsResultContainer.Id);
                }
            }
        }
    }
}