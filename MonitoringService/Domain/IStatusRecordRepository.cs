using System.Threading.Tasks;

namespace MonitoringService.Domain
{
    public interface IStatusRecordRepository
    {
        public Task Create(StatusRecord statusRecord);
    }
}