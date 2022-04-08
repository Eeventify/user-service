using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction_Layer
{
    public interface IUserEventCollection
    {
        public List<int> GetAllEvents(int userID);
        public bool AttendEvent(int userID, int eventID);
        public bool UnattendEvent(int userID, int eventID);
    }
}
