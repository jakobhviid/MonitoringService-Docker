using Microsoft.EntityFrameworkCore;
using MonitoringService.Domain;

namespace MonitoringService.Infrastructure
{
    public class DockerHostContext : DbContext
    {
        public DockerHostContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DockerHost> DockerHosts { get; set; }
        public DbSet<DockerContainer> DockerContainers { get; set; }
        public DbSet<StatsRecord> StatsRecords { get; set; }
        public DbSet<StatusRecord> StatusRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DockerHost>()
                .HasIndex(host => host.ServerName).IsUnique();
            modelBuilder.Entity<DockerContainer>()
                .HasIndex("ContainerId", "DockerHostId").IsUnique();
            modelBuilder.Entity<StatsRecord>()
                .HasIndex("DockerContainerId", "UpdateTime").IsUnique();
            modelBuilder.Entity<StatusRecord>()
                .HasIndex("DockerContainerId", "UpdateTime").IsUnique();
        }
    }
}