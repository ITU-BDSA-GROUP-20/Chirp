

using CSVDBService;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.Instance;
app.MapGet("/cheeps", () =>
{
    try
    {
        return database.Read(10);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

    return null;
});

app.Run();


public record Cheep(string Author, string Message, long Timestamp);
