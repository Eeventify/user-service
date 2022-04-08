using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Abstraction_Layer;
using DTO_Layer;
using Factory_Layer;
using DAL_Layer;

namespace User_Service.Controllers
{
#pragma warning disable 8618
    public class RegisterUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
#pragma warning restore 8618

    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserCollection _userCollection;
        private readonly IUserRegistration _userRegistration;
        private readonly IIdentifierRecursionChecker _identifierRecursionChecker;
        private readonly IIdentifierValidator _identifierValidator;
        private readonly ITokenGenerator _tokenGenerator;



        public LoginController(UserContext? userContext, IUserCollection? userCollection = null, IUserRegistration? userRegistration = null, IIdentifierRecursionChecker? identifierRecursionChecker = null, IIdentifierValidator? identifierValidator = null, ITokenGenerator? tokenGenerator = null)
        {
            _userCollection = userCollection ?? IUserCollectionFactory.Get(userContext);
            _userRegistration = userRegistration ?? IUserRegistrationFactory.Get(userContext);
            _identifierRecursionChecker = identifierRecursionChecker ?? IIdentifierRecursionCheckerFactory.Get(userContext);

            _identifierValidator = identifierValidator ?? new RegexValidator();
            _tokenGenerator = tokenGenerator ?? new JWTGenerator("letmein", TimeSpan.FromDays(14).TotalSeconds);
        }

        /// <summary>
        /// Attempt Login
        /// </summary>
        /// <remarks>
        /// Attempt a login with given user authentification
        /// </remarks>
        /// <param name="email">The email for attempted login</param>
        /// <param name="password">The password for attempted login</param>
        /// <response code="200">The login details are correct and an JWT identification token will be returned</response>
        /// <response code="400">The given input was invalid</response>
        /// <response code="401">Invalid login-detail were given</response>        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
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
                string authToken = GenerateAuthToken(user);
                return Ok(authToken);
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
        /// <param name="user">The user-information in the POST body for which to create an account</param>
        /// <response code="200">The account was created. An authentification token is returned in the body</response> 
        /// <response code="202">An input is invalid or of the wrong format. What specific input will be given in the body</response>
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult Register(RegisterUser user)
        {
            if (!_identifierValidator.Username(user.Username))
            {
                return Accepted("Username contains illegal characters");
            }

            if (!_identifierValidator.Email(user.Email))
            {
                return Accepted("Email contains illegal characters");
            }

            if (!_identifierValidator.Password(user.Password))
            {
                return Accepted("Password contains illegal characters");
            }

            if (!_identifierRecursionChecker.IsEmailUnique(user.Email))
            {
                return Accepted("Email is already in use");
            }

            if (!_identifierRecursionChecker.IsUsernameUnique(user.Username))
            {
                return Accepted("Username is already in use");
            }

            int userID = _userRegistration.AddUser(new UserDTO() { Username = user.Username, PasswordHash = HashManager.GetHash(user.Password), Email = user.Email });

            UserDTO userDTO = _userCollection.GetUser(userID) ?? throw new Exception();

            string authToken = GenerateAuthToken(userDTO);
            return Ok(authToken);
        }

        // Helper Methods
        private string GenerateAuthToken(UserDTO user)
        {
            string userKey = HashManager.GetHash(user.Username + user.PasswordHash);

            Dictionary<string, object> data = new();
            data.Add("ID", user.Id);
            data.Add("key", userKey);

            return _tokenGenerator.Create(data);
        }
    }
}
