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

        private readonly ITokenGenerator _tokenGenerator;
        


        public UserController(UserContext context, IUserCollection? userCollection = null, ITokenGenerator? tokenGenerator = null, IUserEventCollection? userEventCollection = null, IUserInterestCollection? userInterestCollection = null)
        {
            _userCollection = userCollection ?? IUserCollectionFactory.Get(context);
            _userEventCollection = userEventCollection ?? IUserEventCollectionFactory.Get(context);
            _userInterestCollection = userInterestCollection ?? IUserInterestCollectionFactory.Get(context);

            _tokenGenerator = tokenGenerator ?? new JWTGenerator("letmein", TimeSpan.FromDays(14).TotalSeconds);
            
        }

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <remarks>
        /// Get user details based on a given user ID
        /// </remarks>
        /// <param name="Id">The ID of the user</param>
        /// <response code="200">User information is returned in Json format</response>
        /// <response code="400">User with given ID was not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("Details")]
        public IActionResult? UserDetails(int Id)
        {
            UserDTO? userModel = _userCollection.GetUser(Id);

            if (userModel == null)
            {
                return BadRequest("A user with this ID does not exist");
            }
            return Ok(new User(userModel));
        }

        private int IdentifyJWTToken(string token)
        {
            int userID = Convert.ToInt32(_tokenGenerator.Decode(token.Split().Last(), true)["ID"]);

            if (_userCollection.GetUser(userID) == null)
                throw new KeyNotFoundException("A user for the provided token does not exist");
            
            return userID;
        }

        // User Events
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]        
        [Route("AttendEvent/{ID}")]
        public IActionResult? AttendEvent(int ID)
        {
            string? authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

            if (authHeader == null)
                return Unauthorized("No authorization token was provided");

            try
            {
                int userID = IdentifyJWTToken(authHeader);                   

                bool result = _userEventCollection.AttendEvent(userID, ID);
                if (result)
                    return Ok("Event has been attended");
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

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("UnattendEvent/{ID}")]
        public IActionResult? UnattendEvent(int ID)
        {
            string? authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

            if (authHeader == null)
                return Unauthorized("No authorization token was provided");

            try
            {
                int userID = IdentifyJWTToken(authHeader);

                bool result = _userEventCollection.UnattendEvent(userID, ID);
                if (result)
                    return Ok("Event has been unattended");
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("AddInterest/{ID}")]
        public IActionResult? AddInterest(int ID)
        {
            string? authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

            if (authHeader == null)
                return Unauthorized("No authorization token was provided");

            try
            {
                int userID = IdentifyJWTToken(authHeader);

                bool result = _userInterestCollection.AddUserInterest(userID, ID);
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

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Route("RemoveInterest/{ID}")]
        public IActionResult? RemoveInterest(int ID)
        {
            string? authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

            if (authHeader == null)
                return Unauthorized("No authorization token was provided");

            try
            {
                int userID = IdentifyJWTToken(authHeader);

                bool result = _userInterestCollection.RemoveUserInterest(userID, ID);
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
    }
}
