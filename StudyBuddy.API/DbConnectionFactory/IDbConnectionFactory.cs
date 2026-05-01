using System.Data;

namespace StudyBuddy.API.DbConnectionFactory
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}