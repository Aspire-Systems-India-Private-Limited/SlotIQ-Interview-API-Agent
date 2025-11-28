using System.Reflection;

namespace SlotIQ.Interview.Data;

public class SqlQueryLoader : ISqlQueryLoader
{
    private readonly Dictionary<string, string> _queryCache = new();
    private readonly string _sqlPath;

    public SqlQueryLoader()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? throw new InvalidOperationException("Unable to determine assembly location.");
        _sqlPath = Path.Combine(assemblyPath, "Sql");
    }

    public string LoadQuery(string queryName)
    {
        if (_queryCache.TryGetValue(queryName, out var cachedQuery))
        {
            return cachedQuery;
        }

        var filePath = Path.Combine(_sqlPath, $"{queryName}.sql");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"SQL query file not found: {filePath}");
        }

        var query = File.ReadAllText(filePath);
        _queryCache[queryName] = query;
        
        return query;
    }
}
