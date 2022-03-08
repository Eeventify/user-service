using Abstraction_Layer;
using DTO_Layer;
using System.Data;
using System.Data.SqlClient;

namespace DAL_Layer
{
    public class UserDAL : BaseDAL, IUserCollection, IUserRegistration
    {
        public UserDAL() : base("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = \"User Service Database\"; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {
        }

        public UserDTO? GetUser(int Id)
        {
            string query = "SELECT * FROM dbo.Users WHERE ID=@Id";

            SqlParameter idParameter = new("@Id", SqlDbType.Int) { Value = Id };

            SqlCommand cmd = BaseDAL.CommandBuilder(query, idParameter);
            DataTable dataTable = base.RunQuery(cmd);

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

            SqlParameter usernameParameter = new("@Username", SqlDbType.VarChar, 50) { Value = username };

            SqlCommand cmd = BaseDAL.CommandBuilder(query, usernameParameter);
            DataTable dataTable = base.RunQuery(cmd);

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

        public UserDTO? GetUserByEmail(string email)
        {
            string query = "SELECT* FROM dbo.Users WHERE Email = @Email";
            SqlParameter emailParameter = new("@Email", SqlDbType.VarChar, 50) { Value = email };

            SqlCommand cmd = BaseDAL.CommandBuilder(query, emailParameter);
            DataTable dataTable = base.RunQuery(cmd);

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

        public bool AddUser(UserDTO userDTO)
        {
            string query = "INSERT INTO dbo.Users VALUES (@Username, @Email, @PasswordHash, @RegistrationDate)";

            SqlParameter usernameParam = new("@Username", SqlDbType.VarChar, 50) { Value = userDTO.Name };
            SqlParameter emailParam = new("@Email", SqlDbType.VarChar, 100) { Value = userDTO.Email };
            SqlParameter passwordParam = new("@PasswordHash", SqlDbType.Char, 64) { Value = userDTO.PasswordHash };
            SqlParameter regdateParam = new("@RegistrationDate", SqlDbType.Date) { Value = userDTO.RegistrationDate };

            SqlCommand cmd = BaseDAL.CommandBuilder(query, usernameParam, emailParam, passwordParam, regdateParam);
            return base.RunNonQuery(cmd) == 1;
        }        
    }
}
