using Microsoft.EntityFrameworkCore;

using Abstraction_Layer;
using DAL_Layer.Model;

namespace DAL_Layer
{
    public class UserEventDAL : IUserEventCollection
    {
        private readonly UserContext _context;

        public UserEventDAL(DbContext context)
        {
            _context = context as UserContext ?? throw new ArgumentNullException(nameof(context));
        }

        public List<int> GetAllEvents(int userID)
        {
            User user = _context.Users.FirstOrDefault(x => x.Id == userID);

            if (user == null)
                return null;

            return user.ToDTO().EventIDs;
        }

        public bool AttendEvent(int userID, int eventID)
        {
            User user = _context.Users
                .Include(x => x.Events)
                .FirstOrDefault(x => x.Id == userID);

            if (user == null)
                return false;

            if (user.Events.FirstOrDefault(x => x.EventID == eventID) != null)
                return false;

            user.Events.Add(new UserEvent(eventID));
            return _context.SaveChanges() > 0;

        }
        
        public bool UnattendEvent(int userID, int eventID)
        {
            User user = _context.Users
                .Include(x => x.Events)
                .FirstOrDefault(x => x.Id == userID);

            if (user == null)
                return false;

            UserEvent _event = user.Events.FirstOrDefault(x => x.EventID == eventID);
            if (_event == null)
                return false;

            user.Events.Remove(_event);
            return _context.SaveChanges() > 0;
        }
    }
}
