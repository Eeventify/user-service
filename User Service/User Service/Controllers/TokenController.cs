using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JWT.Exceptions;

using Abstraction_Layer;
using Factory_Layer;
using DTO_Layer;

using UserContext = DAL_Layer.UserContext;

namespace User_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IUserCollection _userCollection;
        private readonly ITokenGenerator _tokenGenerator;
                

        public TokenController(UserContext context, ITokenGenerator? tokenGenerator = null, IUserCollection? userCollection = null)
        {
            _userCollection = userCollection ?? IUserCollectionFactory.Get(context);

            _tokenGenerator = tokenGenerator ?? new JWTGenerator("letmein", TimeSpan.FromDays(14).TotalSeconds);
        }

        /// <summary>
        /// Get User From Auth Token
        /// </summary>
        /// <remarks>
        /// Get a user ID from a JWT Authentication token.
        /// This API call is mainly meant for internal use
        /// </remarks>
        /// <param name="token">JWT Authentication token for which to find a user ID</param>
        /// <response code="200">The token is valid and a matching User was found. The ID will be returned</response>
        /// <response code="404">An invalid token was given. A reason why will be given in the response body</response>
        [HttpGet]
        [Route("VerifyAuth")]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult JWTAuthVerify(string token)
        {
            Dictionary<string, object> data;
            try
            {
                data = (Dictionary<string, object>) _tokenGenerator.Decode(token);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }

            UserDTO? userModel = _userCollection.GetUser(Convert.ToInt32(data["userID"]));

            if (userModel == null)
            {
                return Unauthorized("No user matches the given Token");
            }

            if (HashManager.CompareStringToHash(userModel.Username + userModel.PasswordHash, (string)data["key"]))
            {
                return Ok(userModel.Id);
            }
            return Unauthorized("The password for this user account has changed since the Token was generated");
        }
    }
}
