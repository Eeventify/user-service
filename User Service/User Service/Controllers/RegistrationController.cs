using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

using Abstraction_Layer;
using Factory_Layer;
using DTO_Layer;

namespace User_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {

        private readonly IUserRegistration _userRegistration;
        private readonly IIdentifierValidation _identifierValidation;

        public RegistrationController()
        {
            _userRegistration = IUserRegistrationFactory.Get();
            _identifierValidation = IIdentifierValidationFactory.Get();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDTO))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Register(string email, string username, string password)
        {
            if (!ValidateUsername(username))
            {
                return Ok("Username contains illegal characters");
            }

            if (!ValidatePassword(password))
            {
                return Ok("Password contains illegal characters");
            }

            if (!ValidateEmail(email))
            {
                return Ok("Invalid email was given");
            }

            if (!_identifierValidation.IsEmailUnique(email))
            {
                return Conflict("Email is already in use");
            }

            if (!_identifierValidation.IsUsernameUnique(username))
            {
                return Conflict("Username is already in use");
            }            


            UserDTO userDTO = new() { Name = username, PasswordHash = HashManager.GetHash(password), Email = email, RegistrationDate = DateTime.Now };
            _userRegistration.AddUser(userDTO);
            return Created("", userDTO);
        }

        // Helper Methods
        private static bool ValidateUsername(string username)
        {
            Regex regex = new("^(?=.{4,50}$)[a-zA-Z0-9_\\-]+$");
            return regex.IsMatch(username);
        }        

        private static bool ValidatePassword(string password)
        {
            Regex regex = new("^(?=.{4,50}$)[a-zA-Z0-9!?@#$_&*\\-]+$");
            return regex.IsMatch(password);
        }

        private static bool ValidateEmail(string email)
        {
            Regex regex = new("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$");
            return regex.IsMatch(email);
        }
    }
}
