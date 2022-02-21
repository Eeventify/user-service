using Microsoft.AspNetCore.Mvc;

using Abstraction_Layer;
using Factory_Layer;

namespace User_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {

        private readonly IUserRegistration _userRegistration;
        private readonly IUserCollection _userCollection;

        public RegistrationController()
        {
            _userCollection = IUserCollectionFactory.Get();
            _userRegistration = IUserRegistrationFactory.Get();
        }

        [HttpGet]
        public IActionResult Register(string username, string password, string? email = null)
        {
            if (!ValidateUsername(username))
            {
                return Accepted("Username is already in use");
            }

            if (!ValidatePassword(password))
            {
                return Accepted("Password is of incorrect format");
            }

            _userRegistration.AddUser(new DTO_Layer.UserDTO() { Name = username, PasswordHash = HashManager.GetHash(password), Email = email});
            throw new NotImplementedException();
        }

        private bool ValidateUsername(string username)
        {
            return _userCollection.GetUserByUsername(username) != null;
        }

        private bool ValidatePassword(string password)
        {
            throw new NotImplementedException();
        }
    }
}
