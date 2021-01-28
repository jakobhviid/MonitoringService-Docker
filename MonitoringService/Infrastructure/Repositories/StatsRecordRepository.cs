using System;
using MonitoringService.Domain;

namespace MonitoringService.Infrastructure.Repositories
{
    public class StatsRecordRepository : IStatsRecordRepository
    {
        private readonly DockerHostContext _context;

        public StatsRecordRepository(DockerHostContext context)
        {
            _context = context;
        }

        public async void Create(StatsRecord statsRecord)
        {
            if (statsRecord.DockerContainer == null)
            {
                throw new ArgumentNullException(nameof(statsRecord.DockerContainer));
            }

            _context.StatsRecords.Add(statsRecord);
            await _context.SaveChangesAsync();
        }
    }
}