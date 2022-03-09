using DTO_Layer;

namespace User_Service
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }


        public User(int id, string name, string email, DateTime registrationDate)
        {
            Id = id;
            Name = name;
            Email = email;
            RegistrationDate = registrationDate;
        }

        public User(UserDTO userDTO)
        {
            Id = userDTO.Id;
            Name = userDTO.Name;
            Email = userDTO.Email;
            RegistrationDate = userDTO.RegistrationDate;
        }
    }
}
