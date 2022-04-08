using Microsoft.EntityFrameworkCore;

using DAL_Layer.Model;
using Abstraction_Layer;

namespace DAL_Layer
{
    public class UserInterestDAL : IUserInterestCollection
    {
        private readonly UserContext _context;

        public UserInterestDAL(DbContext context)
        {
            _context = context as UserContext ?? throw new ArgumentNullException(nameof(context)); 
        }

        public List<int> GetAllUserInterests(int userID)
        {
            User user = _context.Users.FirstOrDefault(x => x.Id == userID);

            if (user == null)
                return null;

            return user.ToDTO().InterestIDs;
        }

        public bool AddUserInterest(int userID, int interestID)
        {
            User user = _context.Users
                .Include(x => x.Interests)
                .FirstOrDefault(x => x.Id == userID);

            if (user == null)
                return false;

            if (user.Interests.FirstOrDefault(x => x.InterestID == interestID) != null)
                return false;

            user.Interests.Add(new UserInterest(interestID));
            return _context.SaveChanges() > 0;           
        }        

        public bool RemoveUserInterest(int userID, int interestID)
        {
            User user = _context.Users
                .Include(x => x.Interests)
                .FirstOrDefault(x => x.Id == userID);

            if (user == null)
                return false;

            UserInterest interest = user.Interests.FirstOrDefault(x => x.InterestID == interestID);
            if (interest == null)
                return false;

            user.Interests.Remove(interest);
            return _context.SaveChanges() > 0;
        }    
    }
}
