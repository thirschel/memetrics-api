using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace MeMetrics.Infrastructure.Helpers
{
    public class SqlConnectionBuilder
    {
        public static IDbConnection Build(string connectionString)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}