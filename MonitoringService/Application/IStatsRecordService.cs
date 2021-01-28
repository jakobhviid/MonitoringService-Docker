using System.Threading.Tasks;
using MonitoringService.Application.Parameters;
using MonitoringService.Domain;

namespace MonitoringService.Application
{
    public interface IStatsRecordService
    {
        public Task<StatsRecord> Create(CreateStatsRecordParameters parameters);
    }
}