#pragma warning disable 8618

using DTO_Layer;
namespace DAL_Layer.Model
{
    public class User
    {
        // Constructors
        public User()
        {

        }

        public User(UserDTO userDTO)
        {
            Id = userDTO.Id;
            Username = userDTO.Username;
            Email = userDTO.Email;
            PasswordHash = userDTO.PasswordHash;
            RegistrationDate = userDTO.RegistrationDate;
        }

        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }

        // Foreign Keys

        // Navigational Properties

        // Methods
        public UserDTO ToDTO()
        {
            return new UserDTO
            {
                Id = Id,
                Username = Username,
                Email = Email,
                PasswordHash = PasswordHash,
                RegistrationDate = RegistrationDate
            };
        }
    }
}
