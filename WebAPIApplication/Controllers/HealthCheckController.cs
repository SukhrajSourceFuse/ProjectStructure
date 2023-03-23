using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebAPIApplication.Controllers
{
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthCheckController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        [Route("health")]
        public async Task<HealthCheckResult> CheckHealth()
        {
            var healthCheckResult = await _healthCheckService.CheckHealthAsync();

            if (healthCheckResult.Status == HealthStatus.Healthy)
            {
                return HealthCheckResult.Healthy($"API is running.");
            }

            return HealthCheckResult.Unhealthy("API is not running");
        }
    }

}
