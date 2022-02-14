using Microsoft.AspNetCore.Mvc;
using System.Net;

using Abstraction_Layer;
using DTO_Layer;
using Factory_Layer;

namespace User_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        private readonly IUserCollection _userCollection;
                

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;

            _userCollection = IUserCollectionFactory.Get();
        }


        [HttpGet]
        [Route("GetUser")]
        public User GetUsers(int Id)
        {
            return new User(_userCollection.GetUser(Id));
        }

        [HttpGet]
        public User AttemptLogin(string username, string password)
        {
            UserDTO user = _userCollection.GetUserByUsername(username);
            
            if (user == null)
            {
                return null;
            }

            if (HashManager.CompareStringToHash(password, user.PasswordHash))
            {
                return new User(user);
            }
            return null;
        }
    }
}
