using Microsoft.Data.SqlClient;
using System.Data;

namespace StudyBuddy.API.DbConnectionFactory
{
    public class MsSqlDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public MsSqlDbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Mssql")
                ?? throw new InvalidOperationException("Connection string 'Mssql' not found.");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}