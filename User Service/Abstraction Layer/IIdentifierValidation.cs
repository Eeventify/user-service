using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction_Layer
{
    public interface IIdentifierValidation
    {
        public bool IsUsernameUnique(string username);
        public bool IsEmailUnique(string email);
    }
}
