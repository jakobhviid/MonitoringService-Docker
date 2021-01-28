namespace MonitoringService.Application.Parameters
{
    public class CreateDockerHostParameters
    {
        public CreateDockerHostParameters(string serverName, string commandRequestTopic, string commandResponseTopic)
        {
            ServerName = serverName;
            CommandRequestTopic = commandRequestTopic;
            CommandResponseTopic = commandResponseTopic;
        }

        public string ServerName { get; }
        public string CommandRequestTopic { get; }
        public string CommandResponseTopic { get; }
    }
}