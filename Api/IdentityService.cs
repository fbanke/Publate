using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Api
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _accessor;

        public IdentityService(IHttpContextAccessor accessor) {
            _accessor = accessor;
        }
        
        public string GetUserId() {
            var userId = _accessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new InvalidOperationException("UserId not available");
            }

            return userId;
        }
    }
}