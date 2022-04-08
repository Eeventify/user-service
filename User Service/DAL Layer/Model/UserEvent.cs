#pragma warning disable CS8618

namespace DAL_Layer.Model
{
    public class UserEvent
    {
        // Constructors
        public UserEvent()
        {

        }

        public UserEvent(int eventID)
        {
            EventID = eventID;
        }

        // Primary Key
        public int Id { get; set; }

        // Properties
        public int EventID { get; set; }

        // Foreign Keys
        public int UserID { get; set; }

        // Navigational Properties
        public User User { get; set; }

        // Methods
    }
}
