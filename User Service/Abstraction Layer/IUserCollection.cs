using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTO_Layer;

namespace Abstraction_Layer
{
    public interface IUserCollection
    {
        public UserDTO? GetUser(int Id);
        public UserDTO? GetUserByUsername(string username);
        public UserDTO? GetUserByEmail(string email);
    }
}
