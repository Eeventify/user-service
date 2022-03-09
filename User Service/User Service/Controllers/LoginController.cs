using Microsoft.AspNetCore.Mvc;

using Abstraction_Layer;
using DTO_Layer;
using Factory_Layer;

namespace User_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserCollection _userCollection;
        private readonly IUserRegistration _userRegistration;
        private readonly IIdentifierRecursionChecker _identifierRecursionChecker;
        private readonly IIdentifierValidator _identifierValidator;


        public LoginController(IUserCollection? userCollection = null, IUserRegistration? userRegistration = null, IIdentifierRecursionChecker? identifierRecursionChecker = null, IIdentifierValidator? identifierValidator = null)
        {
            _userCollection = userCollection ?? IUserCollectionFactory.Get();
            _userRegistration = userRegistration ?? IUserRegistrationFactory.Get();
            _identifierRecursionChecker = identifierRecursionChecker ?? IIdentifierRecursionCheckerFactory.Get(); 
            _identifierValidator = identifierValidator ?? new RegexValidator();
        }

        /// <summary>
        /// Attempt Login
        /// </summary>
        /// <remarks>
        /// Attempt a login with given user authentification
        /// </remarks>
        /// <param name="email">The email for attempted login</param>
        /// <param name="password">The password for attempted login</param>
        /// <response code="200">The login details are correct and an identification token will be returned</response>
        /// <response code="400">The given input was invalid</response>
        /// <response code="401">Invalid login-detail were given</response>        
        [HttpGet]        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult AttemptLogin(string email, string password)
        {
            if (email == null || password == null)
            {
                return BadRequest("No valid email and password were supplied");
            }

            UserDTO? user = _userCollection.GetUserByEmail(email);           

            if (user != null && HashManager.CompareStringToHash(password, user.PasswordHash))
            {
                return Ok(new User(user));
            } else
            {
                return Unauthorized("Username or password is incorrect");
            }
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <remarks>
        /// Register a new user account with given user information
        /// </remarks>
        /// <param name="username">The username for the created account</param>
        /// <param name="password">The password for the created account</param>
        /// <param name="email">The email for the created account</param>
        /// <response code="200">The account was created. An authentification token is returned in the body</response> 
        /// <response code="202">An input is invalid or of the wrong format. What specific input will be given in the body</response>
        [HttpGet]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult Register(string email, string username, string password)
        {
            if (!_identifierValidator.Username(username))
            {
                return Accepted("Username contains illegal characters");
            }

            if (!_identifierValidator.Email(email))
            {
                return Accepted("Email contains illegal characters");
            }

            if (!_identifierValidator.Password(password))
            {
                return Accepted("Password contains illegal characters");
            }

            if (!_identifierRecursionChecker.IsEmailUnique(email))
            {
                return Accepted("Email is already in use");
            }

            if (!_identifierRecursionChecker.IsUsernameUnique(username))
            {
                return Accepted("Username is already in use");
            }

            _userRegistration.AddUser(new DTO_Layer.UserDTO() { Name = username, PasswordHash = HashManager.GetHash(password), Email = email });
            return Ok();
        }
    }
}
