using DTO_Layer;

namespace User_Service
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Img { get; set; }

        public List<int> Events { get; set; }
        public List<int> Interests { get; set; }


        public User(int id, string name, string email, DateTime registrationDate, List<int> events, List<int> interests, string img)
        {
            Id = id;
            Name = name;
            Email = email;
            RegistrationDate = registrationDate;
            Img = img;

            Events = events;
            Interests = interests;
        }

        public User(UserDTO userDTO)
        {
            Id = userDTO.Id;
            Name = userDTO.Username;
            Email = userDTO.Email;
            RegistrationDate = userDTO.RegistrationDate;
            Img = userDTO.ProfileImg;

            Interests = userDTO.InterestIDs;
            Events = userDTO.EventIDs;
        }
    }
}
