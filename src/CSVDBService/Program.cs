using CSVDBService;
    
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<Cheep> database = CSVDatabase<Cheep>.Instance;
app.MapGet("/cheeps", (int limit) =>
{
    try
    {
        return database.Read(limit);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

    return null;
});

app.MapPost("/cheep", (Cheep cheep) => { database.Store(cheep); });

app.Map("/", () => "You have reached Group 20's Chirp Server");

app.Run();


public record Cheep(string Author, string Message, long Timestamp);
