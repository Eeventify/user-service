using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        public UserController(UserContext context, IUserCollection? userCollection = null)
        {
            _userCollection = userCollection ?? IUserCollectionFactory.Get(context);
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
        public IActionResult UserDetails(int Id)
        {
            UserDTO? userModel = _userCollection.GetUser(Id);

            if (userModel == null)
            {
                return BadRequest("A user with this ID does not exist");
            }
            return Ok(new User(userModel));
        }
    }
}
