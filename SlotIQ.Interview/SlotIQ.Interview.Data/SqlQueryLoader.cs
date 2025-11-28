namespace SlotIQ.Interview.Data;

/// <summary>
/// SQL query loader implementation that loads SQL from external .sql files
/// </summary>
public class SqlQueryLoader : ISqlQueryLoader
{
    private readonly string _sqlPath;

    public SqlQueryLoader()
    {
        _sqlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql");
    }

    public string LoadQuery(string queryName)
    {
        var filePath = Path.Combine(_sqlPath, $"{queryName}.sql");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"SQL file not found: {filePath}");
        }

        return File.ReadAllText(filePath);
    }
}
