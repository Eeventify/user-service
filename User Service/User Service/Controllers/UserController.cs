using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JWT.Exceptions;

using Abstraction_Layer;
using DTO_Layer;
using Factory_Layer;

using UserContext = DAL_Layer.UserContext;

namespace User_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserCollection _userCollection;
        private readonly IUserEventCollection _userEventCollection;
        private readonly IUserInterestCollection _userInterestCollection;
        private readonly IIdentifierValidator _identifierValidator;
        private readonly IIdentifierRecursionChecker _identifierRecursionChecker;
        private readonly ITokenGenerator _tokenGenerator;

        private readonly IHttpClientFactory? _httpClientFactory;
        


        public UserController(UserContext context, IUserCollection? userCollection = null, ITokenGenerator? tokenGenerator = null, IUserEventCollection? userEventCollection = null, IUserInterestCollection? userInterestCollection = null, IHttpClientFactory? httpClientFactory = null)
        {
            _userCollection = userCollection ?? IUserCollectionFactory.Get(context);
            _userEventCollection = userEventCollection ?? IUserEventCollectionFactory.Get(context);
            _userInterestCollection = userInterestCollection ?? IUserInterestCollectionFactory.Get(context);
            _identifierRecursionChecker = IIdentifierRecursionCheckerFactory.Get(context);
            
            
            _tokenGenerator = tokenGenerator ?? new JWTGenerator("letmein", TimeSpan.FromDays(14).TotalSeconds);
            _identifierValidator = new RegexValidator();
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <remarks>
        /// Get user details based on a given user ID
        /// </remarks>
        /// <param name="userID">The ID of the user</param>
        /// <response code="200">User information is returned in Json format</response>
        /// <response code="400">User with given ID was not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("Details/{userID}")]
        public IActionResult? UserDetails(int userID)
        {
            UserDTO? userDTO = _userCollection.GetUser(userID);

            if (userDTO == null)
                return BadRequest("A user with this ID does not exist");

            return Ok(new User(userDTO));
        }

        /// <summary>
        /// Get own User Details
        /// </summary>
        /// <remarks>
        /// Get user details based on the provided login token. Supplies information about the user that makes the request
        /// </remarks>
        /// <response code="200">User information is returned in Json format</response>
        /// <response code="400">The user profile could not be found</response>
        /// <response code="401">No authentication header was provided or the given token was invalid</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("Details")]
        public IActionResult? UserDetails()
        {
            string? authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

            if (authHeader == null)
                return Unauthorized("No authorization token was provided");

            try
            {
                int userID = IdentifyJWTToken(authHeader);

                UserDTO? userDTO = _userCollection.GetUser(userID);

                if (userDTO == null)
                    return BadRequest("The user profile could not be found");

                return Ok(new User(userDTO));
            }
            catch (TokenExpiredException ex)
            {
                return Unauthorized(ex);
            }
            catch (SignatureVerificationException ex)
            {
                return Unauthorized(ex);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex);
            }
            catch (FormatException ex)
            {
                return BadRequest("An error has occured:\n" + ex.Message);
            }
        }


        // User Events
        /// <summary>
        /// Attend Event
        /// </summary>
        /// <remarks>
        /// Adds an event to the user's attending list. The user should be provided using the users authorization token in the authorization header
        /// </remarks>
        /// <param name="eventID">ID of the event the user is attending</param>
        /// <response code="200">The event has been added or was already presetn. More information will be given in the reponse</response>
        /// <response code="400">An error occured with the input. More information will be given in the reponse</response>
        /// <response code="401">No token or an invalid authorization token was provided</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]        
        [Route("AttendEvent/{eventID}")]
        public async Task<IActionResult?> AttendEvent(int eventID)
        {
            string? authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

            if (authHeader == null)
                return Unauthorized("No authorization token was provided");

            try
            {
                int userID = IdentifyJWTToken(authHeader);                   

                bool result = _userEventCollection.AttendEvent(userID, eventID);
                if (result)
                {
                    if (_httpClientFactory != null)
                    {
                        try { 
                        HttpClient httpClient = _httpClientFactory.CreateClient("EventService");
                        var httpResponseMessage = await httpClient.PutAsync($"/Member/{eventID}/Attend?userID={userID}", null);
                        Console.WriteLine(httpResponseMessage.StatusCode);
                        }
                        catch
                        {
                            Console.WriteLine("Connection to Event Service could not be made, is the service down?");
                        }
                    }
                    return Ok("Event has been attended");
                }
                else
                    return Ok("Event was already attended");
            } 
            catch(TokenExpiredException ex)
            {
                return Unauthorized(ex);
            } 
            catch(SignatureVerificationException ex)
            {
                return Unauthorized(ex);
            } 
            catch(KeyNotFoundException ex)
            {
                return BadRequest(ex);
            }
            catch (FormatException ex)
            {
                return BadRequest("An error has occured:\n" + ex.Message);
            }
        }

        /// <summary>
        /// Unttend Event
        /// </summary>
        /// <remarks>
        /// Removes an event from the user's attending list. The user should be provided using the users authorization token in the authorization header
        /// </remarks>
        /// <param name="eventID">ID of the event the user is attending</param>
        /// <response code="200">The event has been removed or was not attended. More information will be provided in the response</response>
        /// <response code="400">An error occured with the input. More information will be given in the reponse</response>
        /// <response code="401">No token or an invalid authorization token was provided</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("UnattendEvent/{eventID}")]
        public async Task<IActionResult?> UnattendEvent(int eventID)
        {
            string? authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

            if (authHeader == null)
                return Unauthorized("No authorization token was provided");

            try
            {
                int userID = IdentifyJWTToken(authHeader);

                bool result = _userEventCollection.UnattendEvent(userID, eventID);
                if (result)
                {
                    if (_httpClientFactory != null)
                    {
                        try
                        {
                            HttpClient httpClient = _httpClientFactory.CreateClient("EventService");
                            var httpResponseMessage = await httpClient.PutAsync($"/Member/{eventID}/Unattend?userID={userID}", null);
                            Console.WriteLine(httpResponseMessage.StatusCode);
                        } catch
                        {
                            Console.WriteLine("Connection to Event Service could not be made, is the service down?");
                        }
                    }
                    return Ok("Event has been unattended");
                }
                else
                    return Ok("Event was never attended");
            }
            catch (TokenExpiredException ex)
            {
                return Unauthorized(ex);
            }
            catch (SignatureVerificationException ex)
            {
                return Unauthorized(ex);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex);
            }
            catch (FormatException ex)
            {
                return BadRequest("An error has occured:\n" + ex.Message);
            }
        }

        // User Interests
        /// <summary>
        /// Add Interest
        /// </summary>
        /// <remarks>
        /// Adds an interest to the user's interest list. The user should be provided using the users authorization token in the authorization header
        /// </remarks>
        /// <param name="interestID">ID of the interest the user is adding</param>
        /// <response code="200">The interest has been added or was already present. More information will be provided in the response</response>
        /// <response code="400">An error occured with the input. More information will be given in the reponse</response>
        /// <response code="401">No token or an invalid authorization token was provided</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("AddInterest/{interestID}")]
        public IActionResult? AddInterest(int interestID)
        {
            string? authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

            if (authHeader == null)
                return Unauthorized("No authorization token was provided");

            try
            {
                int userID = IdentifyJWTToken(authHeader);

                bool result = _userInterestCollection.AddUserInterest(userID, interestID);
                if (result)
                    return Ok("Interest has been added");
                else
                    return Ok("Interest is already selected");
            }
            catch (TokenExpiredException ex)
            {
                return Unauthorized(ex);
            }
            catch (SignatureVerificationException ex)
            {
                return Unauthorized(ex);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex);
            }
            catch (FormatException ex)
            {
                return BadRequest("An error has occured:\n" + ex.Message);
            }
        }

        /// <summary>
        /// Remove Interest
        /// </summary>
        /// <remarks>
        /// Removes an interest from the user's interest list. The user should be provided using the users authorization token in the authorization header
        /// </remarks>
        /// <param name="interestID">ID of the interest the user is removing</param>
        /// <response code="200">The interest has been removed or was never present. More information will be provided in the response</response>
        /// <response code="400">An error occured with the input. More information will be given in the reponse</response>
        /// <response code="401">No token or an invalid authorization token was provided</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("RemoveInterest/{interestID}")]
        public IActionResult? RemoveInterest(int interestID)
        {
            string? authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

            if (authHeader == null)
                return Unauthorized("No authorization token was provided");

            try
            {
                int userID = IdentifyJWTToken(authHeader);

                bool result = _userInterestCollection.RemoveUserInterest(userID, interestID);
                if (result)
                    return Ok("Interest has been removed");
                else
                    return Ok("Interest was not selected");
            }
            catch (TokenExpiredException ex)
            {
                return Unauthorized(ex);
            }
            catch (SignatureVerificationException ex)
            {
                return Unauthorized(ex);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex);
            }
            catch (FormatException ex)
            {
                return BadRequest("An error has occured:\n" + ex.Message);
            }
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("UpdateUsername")]
        public IActionResult? UpdateUsername(string username)
        {
            string? authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

            if (authHeader == null)
                return Unauthorized("No authorization token was provided");
            if (!_identifierRecursionChecker.IsUsernameUnique(username))
            {
                return Accepted("Username is already in use");
            }
            if (!_identifierValidator.Username(username))
            {
                return Accepted("Username contains illegal characters");
            }
            try
            {
                int userID = IdentifyJWTToken(authHeader);

                UserDTO? userDTO = _userCollection.GetUser(userID);

                if (userDTO == null)
                    return BadRequest("This user account does not exist anymore");

                userDTO.Username = username;

                bool state = _userCollection.UpdateUser(userDTO);

                if (state)
                    return Ok("Username changed");
                else
                    return BadRequest("Error: Something went wrong");
            }
            catch (TokenExpiredException ex)
            {
                return Unauthorized(ex);
            }
            catch (SignatureVerificationException ex)
            {
                return Unauthorized(ex);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex);
            }
            catch (FormatException ex)
            {
                return BadRequest("An error has occured:\n" + ex.Message);
            }
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("UpdatePassword")]
        public IActionResult? UpdatePassword(string password)
        {
            string? authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

            if (authHeader == null)
                return Unauthorized("No authorization token was provided");
            if (!_identifierValidator.Password(password))
            {
                return Accepted("Password contains illegal characters");
            }
            try
            {
                int userID = IdentifyJWTToken(authHeader);

                UserDTO? userDTO = _userCollection.GetUser(userID);

                if (userDTO == null)
                    return BadRequest("This user account does not exist anymore");

                userDTO.PasswordHash = HashManager.GetHash(password);

                bool state = _userCollection.UpdateUser(userDTO);

                if (state)
                    return Ok("Password changed");
                else
                    return BadRequest("Error: Something went wrong");
            }
            catch (TokenExpiredException ex)
            {
                return Unauthorized(ex);
            }
            catch (SignatureVerificationException ex)
            {
                return Unauthorized(ex);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex);
            }
            catch (FormatException ex)
            {
                return BadRequest("An error has occured:\n" + ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("UpdateEmail")]
        public IActionResult? UpdateEmail(string email)
        {
            string? authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

            if (authHeader == null)
                return Unauthorized("No authorization token was provided");
            if (!_identifierRecursionChecker.IsEmailUnique(email))
            {
                return Accepted("Email is already in use");
            }
            if (!_identifierValidator.Email(email))
            {
                return Accepted("Email contains illegal characters");
            }
            try
            {
                int userID = IdentifyJWTToken(authHeader);

                UserDTO? userDTO = _userCollection.GetUser(userID);

                if (userDTO == null)
                    return BadRequest("This user account does not exist anymore");

                userDTO.Email = email;

                bool state = _userCollection.UpdateUser(userDTO);

                if (state)
                    return Ok("Email changed");
                else
                    return BadRequest("Error: Something went wrong");
            }
            catch (TokenExpiredException ex)
            {
                return Unauthorized(ex);
            }
            catch (SignatureVerificationException ex)
            {
                return Unauthorized(ex);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex);
            }
            catch (FormatException ex)
            {
                return BadRequest("An error has occured:\n" + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("UpdateImage")]
        public IActionResult? UpdateProfilePicture(string picture)
        {
            string? authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

            if (authHeader == null)
                return Unauthorized("No authorization token was provided");

            try
            {
                int userID = IdentifyJWTToken(authHeader);

                UserDTO? userDTO = _userCollection.GetUser(userID);

                if (userDTO == null)
                    return BadRequest("This user account does not exist anymore");

                userDTO.ProfileImg = picture;

                bool state = _userCollection.UpdateUser(userDTO);

                if (state)
                    return Ok("Profile image changed");
                else
                    return BadRequest("Error: Something went wrong");
            }
            catch (TokenExpiredException ex)
            {
                return Unauthorized(ex);
            }
            catch (SignatureVerificationException ex)
            {
                return Unauthorized(ex);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex);
            }
            catch (FormatException ex)
            {
                return BadRequest("An error has occured:\n" + ex.Message);
            }
        }

        // Helper Methods
        private int IdentifyJWTToken(string token)
        {
            int userID = Convert.ToInt32(_tokenGenerator.Decode(token.Split().Last(), true)["ID"]);

            if (_userCollection.GetUser(userID) == null)
                throw new KeyNotFoundException("A user for the provided token does not exist");

            return userID;
        }
    }
}
