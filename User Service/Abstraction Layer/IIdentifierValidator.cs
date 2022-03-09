using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction_Layer
{
    public interface IIdentifierValidator
    {
        public bool Email(string email);
        public bool Username(string username);        
        public bool Password(string password);
    }
}
