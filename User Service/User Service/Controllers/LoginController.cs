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
                

        public LoginController(IUserCollection? userCollection = null)
        {
             _userCollection = userCollection ?? IUserCollectionFactory.Get();
        }


        [HttpGet]        
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("GetUser")] 
        public IActionResult GetUser(int Id)
        {
            UserDTO? userDTO = _userCollection.GetUser(Id);

            if (userDTO == null)
            {
                return BadRequest("A user with this ID does not exist");
            }
            return Ok(new User(userDTO));
        }

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
    }
}
