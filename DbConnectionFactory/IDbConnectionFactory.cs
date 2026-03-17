using System.Data;

namespace StudyBuddy.API.DbConnecitionFactory
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}