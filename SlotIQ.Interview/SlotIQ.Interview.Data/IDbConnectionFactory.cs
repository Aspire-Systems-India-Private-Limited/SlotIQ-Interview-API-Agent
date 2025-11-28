using System.Data;

namespace SlotIQ.Interview.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
