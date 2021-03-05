using System.Threading.Tasks;

namespace MonitoringService.Domain
{
    public interface IStatsRecordRepository
    {
        public Task Create(StatsRecord statsRecord);
    }
}