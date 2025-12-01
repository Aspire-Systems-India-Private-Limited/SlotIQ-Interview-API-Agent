using System.Data;

namespace SlotIQ.Interview.Data;

/// <summary>
/// Unit of work implementation for transaction management
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnectionFactory _connectionFactory;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _disposed;

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

    public void BeginTransaction()
    {
        _transaction = Connection.BeginTransaction();
    }

    public Task BeginTransactionAsync()
    {
        BeginTransaction();
        return Task.CompletedTask;
    }

    public void Commit()
    {
        _transaction?.Commit();
        _transaction?.Dispose();
        _transaction = null;
    }

    public Task CommitAsync()
    {
        Commit();
        return Task.CompletedTask;
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
    }

    public Task RollbackAsync()
    {
        Rollback();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _transaction?.Dispose();
            _connection?.Dispose();
            _disposed = true;
        }
    }
}
