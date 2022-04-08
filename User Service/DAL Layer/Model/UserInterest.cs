#pragma warning disable CS8618

namespace DAL_Layer.Model
{
    public class UserInterest
    {
        // Constructors
        public UserInterest()
        {

        }

        public UserInterest(int interestID)
        {
            InterestID = interestID;
        }

        // Primary Key
        public int Id { get; set; }

        // Properties
        public int InterestID { get; set; }

        // Foreign Keys
        public int UserID { get; set; }

        // Navigational Properties
        public User User { get; set; }

        // Methods
    }
}
