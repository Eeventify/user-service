using System.Data;
using System.Data.SqlClient;

namespace DAL_Layer
{
    public abstract class BaseDAL
    {
        public string ConnectionString { get; private set; }

        protected BaseDAL(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected DataTable RunQuery(SqlCommand cmd)
        {
            using (SqlConnection sqlConnection = new(ConnectionString)) 
            {
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();

                DataTable dataTable = new();
                dataTable.Load(dataReader);
                
                return dataTable;
            }
        }

        protected int RunNonQuery(SqlCommand cmd)
        {
            using (SqlConnection sqlConnection = new(ConnectionString))
            {
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        protected object RunScalarQuery(SqlCommand cmd)
        {
            using (SqlConnection sqlConnection = new(ConnectionString))
            {
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                return cmd.ExecuteScalar();
            }
        }

        protected static SqlCommand CommandBuilder(string baseQuery, params SqlParameter[] parameters)
        {
            SqlCommand sqlCommand = new(baseQuery);
            foreach (SqlParameter sqlParameter in parameters)
            {
                sqlCommand.Parameters.Add(sqlParameter);
            }
            return sqlCommand;
        }
    }
}
