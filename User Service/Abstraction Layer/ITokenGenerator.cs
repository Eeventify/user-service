using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction_Layer
{
    public interface ITokenGenerator
    {
        public string Create(object payload, params object[] parameters);

        public Dictionary<string, object> Decode(string token, params object[] parameters);
    }
}
