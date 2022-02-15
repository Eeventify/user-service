using Microsoft.AspNetCore.Mvc;

namespace User_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(ILogger<RegistrationController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register(string username, string password, string? email)
        {
            return null;
        }
    }
}
