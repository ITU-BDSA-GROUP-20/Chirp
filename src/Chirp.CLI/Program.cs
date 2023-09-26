using CommandLine;
using Chirp.CLI;
public class Program
{
    class Options
    {
        //Collection of acceptable commands: 
        
        //Read Command
        [Option("read", HelpText="Outputs the specified number of cheeps(NOC): read <NOC>")]
        public int noOfCheeps { get; set; }
        
        //Cheep Command
        [Option("cheep", HelpText = "Stores Message, Author and Timestamp of a cheep: cheep <message>")]
        public string message { get; set; }
    }
    
    private static IRESTConnection<Cheep> database = new RESTConnection<Cheep>("https://bdsagroup20chirpremotedb.azurewebsites.net");
    
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args).WithParsed(options => ReadCommand(options));
    }

    private static void ReadCommand(Options options)
    {
        if (!string.IsNullOrEmpty(options.message))
        {
            WriteCheep(options.message);
            
        } else if (options.noOfCheeps != null)
        {
            ReadCheeps(options.noOfCheeps);
        } 
    }

    private static void ReadCheeps(int noOfLines)
    {
        List<Cheep> cheeps = database.getRequest("cheeps?limit=" + noOfLines); 
        
        // takes the noOfLines of the last cheeps in the 'cheeps' list and stores them in a new list
        // Needs to be refactored to only read noOfLines from the database
        if (noOfLines >= cheeps.Count())
        {
            Console.WriteLine($"{noOfLines} exceeds the amount of cheeps in the database. Showing all {cheeps.Count()} cheeps on record instead.");
            UserInterface.PrintCheeps(cheeps);
        }
        else
        {
            Console.WriteLine($"Showing {noOfLines} newest cheeps out of {cheeps.Count()} cheeps on record.");
            List<Cheep>limitedCheeps = cheeps.GetRange(cheeps.Count()-noOfLines, noOfLines);
            UserInterface.PrintCheeps(limitedCheeps);
        }
    }

    private static void WriteCheep(string message)
    {
        string username = GetUsername();
        long dateTime = TimeStampConverter();
        database.postRequest("/cheep", new Cheep(username, message, dateTime));
    }

    private static string GetUsername()
    {
        return Environment.UserName;
    }

    private static long TimeStampConverter()
    {
        return (long)DateTime.Now.Subtract(DateTime.UnixEpoch).TotalSeconds;
    }
}


