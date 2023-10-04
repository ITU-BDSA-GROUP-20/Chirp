namespace Chirp.SQLite.Tests;

public class UnitTest
{
    
    [Fact]
    public void GetAllMessagesTest()
    {
        // test that we get all messages
        // Arrange
        DBFacade db = new();
        // Act
        List<Object> messages = db.GetAllMessages(1);
        // Assert
        //32 cheeps pr page amounts to 96 messages from the db. 
        Assert.Equal(96, messages.Count);
        //Messages are the building blocks of the cheeps ie author, content, timestamp
    }

    [Theory]
    [InlineData("Helge", "Hello, BDSA students!")]
    [InlineData("Rasmus", "Hej, velkommen til kurset.")]
    public void GetAuthorMessages_checkAuthorAndMessage(string author, string message){
        //Arrange
        DBFacade db = new();
        //Act
        List<Object> messages = db.getAuthorsMessages(author, 1);
        //Assert
        Assert.Equal(author, messages[0]);
        Assert.Equal(message,messages[1]);
    }

    [Theory]
    [InlineData("Helge", "Hello, BDSA students!")]
    [InlineData("Rasmus", "Hej, velkommen til kurset.")]
    public void GetAuthorsMessages_SecondPage(string author, string message){
        //Arrange
        DBFacade db = new();
        //Act
        List<Object> messages = db.getAuthorsMessages(author, 2);
        //Assert
        Assert.Equal(0, messages.Count());
    }

}