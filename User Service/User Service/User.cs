using DTO_Layer;

namespace User_Service
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }


        public User(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public User(UserDTO userDTO)
        {
            Id = userDTO.Id;
            Name = userDTO.Name;
            Email = userDTO.Email;
        }
    }
}
