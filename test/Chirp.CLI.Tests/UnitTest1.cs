using SimpleDB;

namespace Chirp.CLI.Tests;
using System;

using SimpleDB;
public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var expected = 1;

        // Act
        var actual = 1;

        // Assert
        Assert.Equal(expected, actual);
    }
}