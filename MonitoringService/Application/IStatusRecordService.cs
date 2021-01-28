using System.Threading.Tasks;
using MonitoringService.Application.Parameters;
using MonitoringService.Domain;

namespace MonitoringService.Application
{
    public interface IStatusRecordService
    {
        public Task<StatusRecord> Create(CreateStatusRecordParameters parameters);
    }
}