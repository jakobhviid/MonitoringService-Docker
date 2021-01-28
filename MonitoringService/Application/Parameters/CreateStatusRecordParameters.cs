using System;

namespace MonitoringService.Application.Parameters
{
    public class CreateStatusRecordParameters
    {
        public CreateStatusRecordParameters(string containerId, string serverName, string state, string status,
            string health, DateTime updateTime)
        {
            ContainerId = containerId;
            ServerName = serverName;
            State = state;
            Status = status;
            Health = health;
            UpdateTime = updateTime;
        }

        public string ContainerId { get; }
        public string ServerName { get; }
        public string State { get; }
        public string Status { get; }
        public string Health { get; }
        public DateTime UpdateTime { get; }
    }
}