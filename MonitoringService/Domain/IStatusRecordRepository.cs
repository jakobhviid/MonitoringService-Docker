namespace MonitoringService.Domain
{
    public interface IStatusRecordRepository
    {
        public void Create(StatusRecord statusRecord);
    }
}