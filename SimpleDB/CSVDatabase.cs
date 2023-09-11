using System.Globalization;
using CsvHelper;


namespace SimpleDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    public string filePath;
    List<T> cheepCollection;

    public CSVDatabase(string filePath)
    {
        this.filePath = filePath;
        
        // TODO
        // This might not be the correct way to handle the issue of not exiting constructor with null-value
        cheepCollection = new();
    }

    public IEnumerable<T> Read(int limit)
    {
        cheepCollection = new(); 
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<T>();
        foreach (var T in records)
        {
            cheepCollection.Add(T);
        }
        
        return cheepCollection;
    }

    public void Store(T record)
    {
        throw new NotImplementedException();
    }

    

}
   

