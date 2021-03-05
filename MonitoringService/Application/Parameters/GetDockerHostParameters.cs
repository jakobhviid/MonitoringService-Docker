namespace MonitoringService.Application.Parameters
{
    public class GetDockerHostParameters
    {
        public GetDockerHostParameters(string serverName)
        {
            ServerName = serverName;
        }

        public string ServerName { get; }
    }
}