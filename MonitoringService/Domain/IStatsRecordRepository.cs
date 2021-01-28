namespace MonitoringService.Domain
{
    public interface IStatsRecordRepository
    {
        public void Create(StatsRecord statsRecord);
    }
}