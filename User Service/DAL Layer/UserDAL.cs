using Abstraction_Layer;
using DTO_Layer;
using System.Data;
using System.Data.SqlClient;

namespace DAL_Layer
{
    public class UserDAL : BaseDAL, IUserCollection, IUserRegistration, IIdentifierRecursionChecker
    {
        public UserDAL() : base(@"Server=db;Database=User Service;User=sa;Password=R9QgoT#Pm8;")
        {

        }

        public UserDTO? GetUser(int Id)
        {
            string query = "SELECT * FROM dbo.Users WHERE ID=@Id";

            SqlParameter idParam = new("@Id", SqlDbType.Int) { Value = Id };

            SqlCommand cmd = BaseDAL.CommandBuilder(query, idParam);
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
            
            SqlParameter emailParam = new("@Email", SqlDbType.VarChar, 50) { Value = email };

            SqlCommand cmd = BaseDAL.CommandBuilder(query, emailParam);
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

        public int AddUser(UserDTO userDTO)
        {
            string query = "INSERT INTO dbo.Users VALUES (@Username, @Email, @PasswordHash, @RegistrationDate) SELECT SCOPE_IDENTITY()";

            SqlParameter usernameParam = new("@Username", SqlDbType.VarChar, 50) { Value = userDTO.Name };
            SqlParameter emailParam = new("@Email", SqlDbType.VarChar, 100) { Value = userDTO.Email };
            SqlParameter passwordParam = new("@PasswordHash", SqlDbType.Char, 64) { Value = userDTO.PasswordHash };
            SqlParameter regdateParam = new("@RegistrationDate", SqlDbType.DateTime) { Value = DateTime.Now };

            SqlCommand cmd = BaseDAL.CommandBuilder(query, usernameParam, emailParam, passwordParam, regdateParam);
            return Convert.ToInt32(base.RunScalarQuery(cmd));
        }        

        public bool IsUsernameUnique(string username)
        {
            string qeury = "SELECT * FROM dbo.Users WHERE Username=@Username";

            SqlParameter usernameParam = new("@Username", SqlDbType.VarChar, 50) { Value = username };
            SqlCommand cmd = BaseDAL.CommandBuilder(qeury, usernameParam);

            DataTable result = base.RunQuery(cmd);
            return result.Rows.Count == 0;
        }
        public bool IsEmailUnique(string email) 
        {
            string qeury = "SELECT * FROM dbo.Users WHERE Email=@Email";

            SqlParameter emailParam = new("@Email", SqlDbType.VarChar, 100) { Value = email };
            SqlCommand cmd = BaseDAL.CommandBuilder(qeury, emailParam);
            
            DataTable result = base.RunQuery(cmd);
            return result.Rows.Count == 0;
        }
    }
}
