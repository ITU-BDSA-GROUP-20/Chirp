namespace SimpleDB;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    public CSVDatabase(string filePath)
    {
        
    }

    public IEnumerable<T> Read(int limit)
    {
        throw new NotImplementedException();
    }

    public void Store(T record)
    {
        throw new NotImplementedException();
    }
}