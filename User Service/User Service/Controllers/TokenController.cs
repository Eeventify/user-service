using Microsoft.AspNetCore.Mvc;

namespace User_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        public TokenController()
        {

        }

        /// <summary>
        /// Request Authentification Token
        /// </summary>
        /// <remarks>
        /// Request an authentification token for a user.
        /// If one already exists, this one will be returned. Otherwise a new one will be generated.
        /// </remarks>
        /// <param name="id">ID for user that the authentification token is being requested for</param>
        /// <response code="200"></response>
        /// <response code="404"></response>
        [HttpGet]
        [Route("Request")]
        public IActionResult RequestToken(int id)
        { 
            return Ok();
        }
    }
}
