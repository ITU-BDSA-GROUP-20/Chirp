﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Markup;
using CommandLine;

using Chirp.CLI;
using CommandLine.Text;
using CsvHelper;
using SimpleDB;

public class Program
{
    class Options
    {
        //Collection of acceptable commands: 
        
        
        //Read Command
        [Option( "read", HelpText="Outputs the specified number of cheeps(NOC): read <NOC>")]
        public int noOfCheeps { get; set; }
        
        //Cheap Command
        [Option("cheep", HelpText = "Stores Message, Author and Timestamp of a cheep: cheep <message>")]
        public string message { get; set; }
        
      
    }
    
    
    
    static string csvFilePath = "chirp_cli_db.csv";
    private static IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>(csvFilePath);
    
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


