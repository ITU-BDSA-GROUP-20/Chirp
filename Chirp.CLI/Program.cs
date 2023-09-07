using System.Text.RegularExpressions;
using Chirp.CLI;
using SimpleDB;

string csvFilePath = "chirp_cli_db.csv";
IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>(csvFilePath);


if (args.Length != 2)
{
    Console.WriteLine("Command usage: <command> <action>");
    return;
}
if (args[0].Equals("read"))
{
    int noOfLines = int.Parse(args[1]);

    foreach (Cheep singleCheep in database.Read(noOfLines))
    {
        Console.WriteLine(singleCheep);
    }
} else if(args[0] == "cheep" )
{
    string message = args[1];
    
    database.Store(new Cheep(message));
}


