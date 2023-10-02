namespace Chirp.SQLite;

using System.Data;
using System.Reflection;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;




public class DBFacade{
    //Should these paths be hardcoded?
    readonly string sqlDBFilePath = "/tmp/chirp.db";
    readonly string environmentDBPath = Environment.GetEnvironmentVariable("CHIRPDBPATH")!;
    private string ChooseDBPath(){
        //Chose environment variable or default path
        string activeDBPath;

        if(environmentDBPath != null){
            activeDBPath = environmentDBPath;
        } else {
            activeDBPath = sqlDBFilePath;    
        }
        return activeDBPath;
    }

    private List<Object> ConnectAndQuery(string query){
        //Establishing connection and executing query against db
        string activeDBPath = ChooseDBPath();

        using (var connection = new SqliteConnection($"Data Source = {activeDBPath}")){
            //Connecting to db, and executing query against it
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = query;
            using var reader = command.ExecuteReader(); 

            
            return ReadQueryResult(reader);

        }
       

    }

    //Method for retrieving all cheeps, in abstracted form
    public List<Object> GetAllMessages(){
        //Query that retrieves all messages
        string query = @"SELECT user.username, message.text, message.pub_date FROM message JOIN user on message.author_id = user.user_id ORDER by message.pub_date desc";
        return ConnectAndQuery(query);
    }


    public List<Object> getAuthorsMessages(string author){

       //Only works if command gets parameterized
        var query = @"SELECT user.username, message.text, message.pub_date FROM message JOIN user ON message.author_id = user.user_id WHERE user.username = @author ORDER by message.pub_date desc";
        string activeDBPath = ChooseDBPath();
        using (var connection = new SqliteConnection($"Data Source = {activeDBPath}")){
            //Connecting to db, and executing query against it
            connection.Open();
            var command = connection.CreateCommand();
            //Adds the parameter for the query
            command.Parameters.AddWithValue("@author", author);
            command.CommandText = query;
            using var reader = command.ExecuteReader(); 

            //Iterating through query results and adding them to a list
            return ReadQueryResult(reader);

        }
        
    }

    private  List<Object> ReadQueryResult(SqliteDataReader reader){

        List<Object> messages = new List<Object>();
        while (reader.Read()){
                Object[] values = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(values);
                for (int i = 0; i < fieldCount; i++)
                    //messages.Add($"{reader.GetName(i)}: {values[i]}");
                    messages.Add($"{values[i]}");
         }
        return messages;
    }
    //Main is only there so I can test the fucntionality via dotnet run.
    public static void Main(){
        DBFacade db = new DBFacade();
        List<Object> messages = db.GetAllMessages();
         foreach (Object message in messages){
            Console.WriteLine(message);
        }
        List<Object> authorsMessages = db.getAuthorsMessages("Helge");
        foreach (Object message in authorsMessages){
            Console.WriteLine(message);
        }
        Console.WriteLine("-----------------");
    }
}







