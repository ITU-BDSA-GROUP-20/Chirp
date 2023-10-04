namespace Chirp.SQLite;

using System.Diagnostics;
using System.Data;
using System.Reflection;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;

public class DBFacade
{
    //TODO: Consider adding the connection as a field, so the connections stays open for the duration of the program
    //Should these paths be hardcoded?
    
    private static readonly string assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    readonly string sqlDBFilePath = Path.Combine(assemblyDirectory, "data", "chirp.db");

    private List<Object> ConnectAndQuery(string query, int page)
    {
        //Establishing connection and executing query against db

        using (var connection = new SqliteConnection($"Data Source = {sqlDBFilePath}"))
        {
            //Connecting to db, and executing query against it
            connection.Open();
            var command = connection.CreateCommand();
            command.Parameters.AddWithValue("@PAGE", page);
            command.CommandText = query;
            using var reader = command.ExecuteReader();

            return ReadQueryResult(reader);
        }
    }

    //Method for retrieving all cheeps, in abstracted form
    public List<Object> GetAllMessages(int page)
    {
        //Query that retrieves all messages
        string query = @"SELECT 
                        user.username, message.text, message.pub_date 
                        FROM message 
                        JOIN user on message.author_id = user.user_id 
                        ORDER by message.pub_date 
                        desc 
                        LIMIT 32 OFFSET (@PAGE -1) * 32";

        return ConnectAndQuery(query, page);
    }


    public List<Object> getAuthorsMessages(string author, int page)
    {
        //Only works if command gets parameterized
        var query = @"SELECT 
                    user.username, message.text, message.pub_date 
                    FROM message 
                    JOIN user ON message.author_id = user.user_id 
                    WHERE user.username = @author 
                    ORDER by message.pub_date 
                    desc 
                    LIMIT 32 OFFSET (@PAGE -1) * 32";
        
        using (var connection = new SqliteConnection($"Data Source = {sqlDBFilePath}"))
        {
            //Connecting to db, and executing query against it
            connection.Open();
            var command = connection.CreateCommand();
            //Adds the parameter for the query
            command.Parameters.AddWithValue("@author", author);
            command.Parameters.AddWithValue("@PAGE", page);

            command.CommandText = query;
            using var reader = command.ExecuteReader();

            //Iterating through query results and adding them to a list
            return ReadQueryResult(reader);
        }
    }

    private List<Object> ReadQueryResult(SqliteDataReader reader)
    {
        List<Object> messages = new List<Object>();
        while (reader.Read())
        {
            Object[] values = new Object[reader.FieldCount];
            int fieldCount = reader.GetValues(values);
            for (int i = 0; i < fieldCount; i++)
                //messages.Add($"{reader.GetName(i)}: {values[i]}");
                messages.Add($"{values[i]}");
        }

        return messages;
    }
}