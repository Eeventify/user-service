#pragma warning disable 8618

using Microsoft.EntityFrameworkCore;

namespace DTO_Layer
{
    public class UserDTO
    {
        public UserDTO()
        {
            EventIDs ??= new();
            InterestIDs ??= new();
        }

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? ProfileImg { get; set; }

        public List<int> EventIDs { get; set; }
        public List<int> InterestIDs { get; set; }
    }
}