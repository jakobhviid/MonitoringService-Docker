using System;
using MonitoringService.Domain;

namespace MonitoringService.Infrastructure.Repositories
{
    public class StatusRecordRepository : IStatusRecordRepository
    {
        private readonly DockerHostContext _context;

        public StatusRecordRepository(DockerHostContext context)
        {
            _context = context;
        }

        public async void Create(StatusRecord statusRecord)
        {
            if (statusRecord.DockerContainer == null)
            {
                throw new ArgumentNullException(nameof(statusRecord.DockerContainer));
            }

            _context.StatusRecords.Add(statusRecord);
            await _context.SaveChangesAsync();
        }
    }
}