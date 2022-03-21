using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DTO_Layer;

namespace Abstraction_Layer
{
    public enum RegisterState
    {
        Success,
        UsernameInUse,
        EmailInUse,
        PasswordInvalid
    }

    public interface IUserRegistration
    {
        public bool AddUser(UserDTO userDTO);
    }
}
