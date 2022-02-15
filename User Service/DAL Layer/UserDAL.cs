using Abstraction_Layer;
using DTO_Layer;
using System.Data;
using System.Data.SqlClient;

namespace DAL_Layer
{
    public class UserDAL : BaseDAL, IUserCollection
    {                
        public UserDAL()
        {
            
        }

        public UserDTO? GetUser(int Id)
        {
            string query = "SELECT * FROM dbo.Users WHERE ID=@Id";

            SqlParameter idParameter = new("@Id", SqlDbType.Int) { Value = Id };

            SqlCommand cmd = base.commandBuilder(query, idParameter);
            DataTable dataTable = base.runQuery(cmd);

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            return new UserDTO
            {
                Id = (int)dataTable.Rows[0]["Id"],
                Name = (string)dataTable.Rows[0]["Username"],
                Email = (string)dataTable.Rows[0]["Email"],
                PasswordHash = (string)dataTable.Rows[0]["PasswordHash"],
                RegistrationDate = (DateTime)dataTable.Rows[0]["RegistrationDate"]
            };
        }

        public UserDTO? GetUserByUsername(string username)
        {
            string query = "SELECT * FROM dbo.Users WHERE Username=@Username";

            SqlParameter idParameter = new("@Username", SqlDbType.VarChar, 50) { Value = username };

            SqlCommand cmd = base.commandBuilder(query, idParameter);
            DataTable dataTable = base.runQuery(cmd);

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            return new UserDTO
            {
                Id = (int)dataTable.Rows[0]["Id"],
                Name = (string)dataTable.Rows[0]["Username"],
                Email = (string)dataTable.Rows[0]["Email"],
                PasswordHash = (string)dataTable.Rows[0]["PasswordHash"],
                RegistrationDate = (DateTime)dataTable.Rows[0]["RegistrationDate"]
            };
        }
    }
}
