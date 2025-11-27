namespace SlotIQ.Interview.Data;

/// <summary>
/// SQL query loader interface for loading external SQL files
/// </summary>
public interface ISqlQueryLoader
{
    string LoadQuery(string queryName);
}
