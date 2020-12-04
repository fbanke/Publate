using System.Threading.Tasks;
using Api.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IIdentityService _identityService;

        public UsersController(IIdentityService identityService, ILogger<UsersController> logger)
        {
            _logger = logger;
            _identityService = identityService;
        }
        
        [HttpGet]
        [Route("me/settings")]
        public Settings MySettings()
        {
            var id = _identityService.GetUserId();
            
            return new Settings
            {
                UserId = id
            };
        }
    }
}