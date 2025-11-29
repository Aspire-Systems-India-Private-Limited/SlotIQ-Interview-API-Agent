using System.Data;

namespace SlotIQ.Interview.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnectionFactory _connectionFactory;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public UnitOfWork(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IDbConnection Connection
    {
        get
        {
            if (_connection == null)
            {
                _connection = _connectionFactory.CreateConnection();
                _connection.Open();
            }
            return _connection;
        }
    }

    public IDbTransaction? Transaction => _transaction;

    public Task BeginTransactionAsync()
    {
        _transaction = Connection.BeginTransaction();
        return Task.CompletedTask;
    }

    public Task CommitAsync()
    {
        _transaction?.Commit();
        return Task.CompletedTask;
    }

    public Task RollbackAsync()
    {
        _transaction?.Rollback();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
    }
}
