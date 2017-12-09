using System.Data;

namespace Sample.Repository.Database
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection Create();
    }
}