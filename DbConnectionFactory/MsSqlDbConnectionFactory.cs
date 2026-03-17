using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace StudyBuddy.API.DbConnecitionFactory
{
    public class MsSqlDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public MsSqlDbConnectionFactory(IConfiguration connectionString)
        {
            _connectionString = connectionString.GetConnectionString("Mssql");
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}