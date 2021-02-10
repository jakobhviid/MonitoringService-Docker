using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonitoringService.Application;
using MonitoringService.Application.Parameters;

namespace MonitoringService.Api.Controllers
{
    
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class StatisticController : ControllerBase
    {
        private readonly IDockerHostService _dockerHostService;

        public StatisticController(IDockerHostService dockerHostService)
        {
            _dockerHostService = dockerHostService;
        }

        [HttpGet("status")]
        public async Task<ActionResult> ListStatusCounts(string serverName, DateTime? from, DateTime? to)
        {
            if (string.IsNullOrEmpty(serverName))
            {
                return StatusCode(StatusCodes.Status400BadRequest, $"{nameof(serverName)} parameter is required!");
            }
            Console.WriteLine("Input: " + serverName + ";" + from + ";" + to);
            
            var parameters = new ListContainerStatisticParameters
            {
                ServerName = serverName,
                PeriodFrom = from,
                PeriodTo = to
            };
            var result = await _dockerHostService.ListContainerStatistic(parameters);
            return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}