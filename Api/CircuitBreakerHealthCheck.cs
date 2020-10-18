using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Api
{
    public class CircuitBreakerHealthCheck : IHealthCheck
    {
        public static bool Healthy { get; set; } = true;
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken)
        {
            if(Healthy)
            {
                return HealthCheckResult.Healthy();
            }
            
            return HealthCheckResult.Unhealthy();
        }
    }
}