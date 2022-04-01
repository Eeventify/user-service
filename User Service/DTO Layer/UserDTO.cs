#pragma warning disable 8618

using Microsoft.EntityFrameworkCore;

namespace DTO_Layer
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}