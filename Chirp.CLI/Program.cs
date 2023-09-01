using System.Text.RegularExpressions;

if (args.Length != 2)
{
    Console.WriteLine("Command usage: <command> <action>");
    return;
}

if (args[0].Equals("read"))
{
    int noOfLines = int.Parse(args[1]);

    using (StreamReader reader = new StreamReader("C:\\ITU\\Semester 3\\Software Arch\\Chirp\\Chirp.CLI\\chirp_cli_db.csv")) 
    {
        while (!reader.EndOfStream)
        {
            string author;
            string message;
            int timeStamp;
            Regex regex = new Regex("[,\"][\",]");
            string line = reader.ReadLine();
            string[] splitLine = regex.Split(line);
            
            author = splitLine[0];
            message = splitLine[1];
            timeStamp = int.Parse(splitLine[2]);
            DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(timeStamp);
            
            Console.WriteLine($"{author} @ {time}: {message}");
        }
    }
}

