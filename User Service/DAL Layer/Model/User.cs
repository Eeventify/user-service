#pragma warning disable 8618

using DTO_Layer;
namespace DAL_Layer.Model
{
    public class User
    {
        // Constructors
        public User()
        {
            Events ??= new();
            Interests ??= new();
        }

        public User(UserDTO userDTO)
        {
            Id = userDTO.Id;
            Username = userDTO.Username;
            Email = userDTO.Email;
            PasswordHash = userDTO.PasswordHash;
            RegistrationDate = userDTO.RegistrationDate;
            ProfileImg = userDTO.ProfileImg;

            List<UserEvent> _userEvents = new();
            foreach(int id in userDTO.EventIDs)
            {
                _userEvents.Add(new UserEvent(id));
            }
            Events = _userEvents;

            List<UserInterest> _userInterests = new();
            foreach (int id in userDTO.InterestIDs)
            {
                _userInterests.Add(new UserInterest(id));
            }
            Interests = _userInterests;
        }

        // Primary Key
        public int Id { get; set; }

        // Properties
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ProfileImg { get; set; }

        // Foreign Keys

        // Navigational Properties
        public List<UserEvent> Events { get; set; }
        public List<UserInterest> Interests { get; set; }

        // Methods
        public UserDTO ToDTO()
        {
            List<int> _userEvents = new();
            foreach (UserEvent _event in Events)
            {
                _userEvents.Add(_event.EventID);
            }

            List<int> _userInterests = new();
            foreach (UserInterest _interest in Interests)
            {
                _userInterests.Add(_interest.InterestID);
            }

            return new UserDTO
            {
                Id = Id,
                Username = Username,
                Email = Email,
                PasswordHash = PasswordHash,
                RegistrationDate = RegistrationDate,
                EventIDs = _userEvents,
                InterestIDs = _userInterests
            };
        }
    }
}
