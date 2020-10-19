using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api
{
    public class CircuitBreakerHealthCheck : IHealthCheck
    {
        public static bool Healthy { get; set; } = true;
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken)
        {
            if(Healthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            
            return Task.FromResult(HealthCheckResult.Unhealthy());
        }
    }
}