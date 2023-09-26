using System.Globalization;
using CsvHelper;

namespace CSVDBService;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    private static string filePath = "./data/cheeps.csv";
    private static CSVDatabase<T> instance;

    private CSVDatabase()
    {
        // Extract directory path from the file path
        string directoryPath = Path.GetDirectoryName(filePath);
    
        // Check if directory exists, if not, create it
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    
        // Check if file exists, if not, create it and write the header
        if (!File.Exists(filePath))
        {
            using StreamWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine("Author,Message,Timestamp");
        }
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
        using StreamReader reader = new StreamReader(filePath);
        using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        List<T> records = new List<T>(csv.GetRecords<T>());
        
        // returns entire collection if 'limit' is greater than amount of records in cheepCollection,
        // returns 'limit' newest cheeps otherwise.
        if (limit >= records.Count)
        {
            Console.WriteLine($"{limit} exceeds the amount of cheeps in the database. Showing all {records.Count()} cheeps on record instead.");
            return records;
        }
        else
        {
            Console.WriteLine($"Showing {limit} newest cheeps out of {records.Count()} cheeps on record.");
            return records.GetRange(records.Count()-limit, limit);
        }
    }

    public void Store(T record)
    {
        using StreamWriter writer = new StreamWriter(filePath, true);
        using CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecord(record);
        writer.WriteLine();
    }
}