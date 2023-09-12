﻿using System.ComponentModel;
using Chirp.CLI;
using SimpleDB;

public class Program
{
    static string csvFilePath = "chirp_cli_db.csv";
    private static IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>(csvFilePath);
    
    public static void Main(string[] args)
    {
        try
        {
            ReadCommand(args);
        }
        catch (FormatException)
        {
            Console.WriteLine("Read command usage: read <integer>");
        }
        catch (Exception)
        {
            // Temporary
            Console.WriteLine("Command usage: <command> <action>");
        }
    }

    private static void ReadCommand(string[] args)
    {
        switch (args[0])
        {
            case "read":
                ReadCheeps(int.Parse(args[1]));
                break;
            case "cheep":
                WriteCheep(args[1]);
                break;
            default:
                throw new Exception();
        }
    }

    private static void ReadCheeps(int noOfLines)
    {
        UserInterface.PrintCheeps(database.Read(noOfLines));
    }

    private static void WriteCheep(string message)
    {
        string username = GetUsername();
        long dateTime = TimeStampConverter();
        database.Store(new Cheep(username, message, dateTime));
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


