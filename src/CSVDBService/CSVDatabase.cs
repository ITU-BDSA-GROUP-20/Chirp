using System.Globalization;
using CsvHelper;

namespace CSVDBService;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    private static string filePath = @"./data/cheeps.csv";
    List<T> cheepCollection;
    private static CSVDatabase<T> instance;

    private CSVDatabase()
    {
        // TODO
        // This might not be the correct way to handle the issue of not exiting constructor with null-value
        cheepCollection = new List<T>();
    }

    public static CSVDatabase<T> Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CSVDatabase<T>();
            }

            return instance;
        }
    }

    public IEnumerable<T> Read(int limit)
    {
        cheepCollection = new List<T>(); 
        using StreamReader reader = new StreamReader(filePath);
        using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<T>();
        
        foreach (var T in records)
        {
            cheepCollection.Add(T);
        }
        
        // returns entire collection if 'limit' is greater than amount of records in cheepCollection,
        // returns 'limit' newest cheeps otherwise.
        if (limit >= cheepCollection.Count)
        {
            Console.WriteLine($"{limit} exceeds the amount of cheeps in the database. Showing all {cheepCollection.Count()} cheeps on record instead.");
            return cheepCollection;
        }
        {
            Console.WriteLine($"Showing {limit} newest cheeps out of {cheepCollection.Count()} cheeps on record.");
            return cheepCollection.GetRange(cheepCollection.Count()-limit, limit);
        }
    }

    public void Store(T record)
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            writer.WriteLine();
            csv.WriteRecord(record);
        }
    }
}