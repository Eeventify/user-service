using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction_Layer
{
    public interface IUserInterestCollection
    {
        public List<int> GetAllUserInterests(int userID);
        public bool AddUserInterest(int userID, int interestID);
        public bool RemoveUserInterest(int userID, int interestID);
    }
}
