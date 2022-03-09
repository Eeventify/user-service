using Abstraction_Layer;
using DTO_Layer;
using System.Data;
using System.Data.SqlClient;

namespace DAL_Layer
{
    public class TokenDAL: BaseDAL
    {
        public TokenDAL() : base("Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = User_Service; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {

        }

        
    }
}
