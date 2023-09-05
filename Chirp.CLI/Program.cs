using System.Text.RegularExpressions;

if (args.Length != 2)
{
    Console.WriteLine("Command usage: <command> <action>");
    return;
}
var csvFilePath = "chirp_cli_db.csv";
if (args[0].Equals("read"))
{
    int noOfLines = int.Parse(args[1]);

    using (StreamReader reader = new StreamReader(csvFilePath)) 
    {
        while (!reader.EndOfStream)
        {
            string author;
            string message;
            int timeStamp;
            Regex regex = new Regex("[,\"][\",]");
            string line = reader.ReadLine()!;
            string[] splitLine = regex.Split(line);
            
            author = splitLine[0];
            message = splitLine[1];
            timeStamp = int.Parse(splitLine[2]);
            DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(timeStamp);
            
            Console.WriteLine($"{author} @ {time}: {message}");
        }
    }
} else if(args[0] == "cheep" )
{
    string message = args[1];
    //Src: "user142162" https://stackoverflow.com/questions/1240373/how-do-i-get-the-current-username-in-net-using-c
    string author = Environment.UserName; //Gets the current users username. 
   
    //Src: "Andrew" https://stackoverflow.com/questions/17632584/how-to-get-the-unix-timestamp-in-c-sharp 
    long unixTimeStamp = (long)DateTime.Now.Subtract(DateTime.UnixEpoch).TotalSeconds;
    
    string cheep = $"{author},\"{message}\",{unixTimeStamp}";

    //Write to CSV file

    using StreamWriter sw = File.AppendText(csvFilePath);
    sw.WriteLine(cheep);

}

