using System.Data;

namespace SlotIQ.Interview.Data;

/// <summary>
/// Factory for creating database connections
/// </summary>
public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
