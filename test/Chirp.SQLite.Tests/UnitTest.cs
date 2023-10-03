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
        Assert.Equal(32, messages.Count);
    }
}