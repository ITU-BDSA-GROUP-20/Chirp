using System.Diagnostics;
using Xunit.Abstractions;

namespace Chirp.CLI.Tests;
using Chirp.CLI;
using Xunit;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public void InitDatabase()
    {
        // clear database
        // add test data
    }

    [Fact]
    public void TestCheepToString()
    {
        // Arrange
        var cheep = new Cheep("test", "test", 1695108998);

        // Act
        var output = cheep.ToString();

        // Assert
        Assert.Equal("test" + " @ " +
                     DateTimeOffset.FromUnixTimeSeconds(1695108998).LocalDateTime + " "
                     + "test", output);
    }

    [Fact]
    public void ReadCheepEnd2EndTest()
    {
        // Arrange
        // No database arrangement needed as of now
        
        // Act
        string output = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run --project ../../src/Chirp.CLI/Chirp.CLI.csproj --read 1";
            process.StartInfo.WorkingDirectory = "../../../";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }

        string fstCheep = output.Split("\n")[1].TrimEnd();
        
        // Assert
        Assert.StartsWith("mikke", fstCheep);
        Assert.EndsWith("Hello There good Sir", fstCheep);
    }
   /* 
    [Fact]
    public void WriteCheepEnd2EndTest()
    {
        // Arrange
        // No database arrangement needed as of now
        
        // Act
        string output = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run --project ../../src/Chirp.CLI/Chirp.CLI.csproj --read 1";
            process.StartInfo.WorkingDirectory = "../../../";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }

        string fstCheep = output.Split("\n")[1].TrimEnd();
        
        // Assert
        Assert.StartsWith("mikke", fstCheep);
        Assert.EndsWith("Hello There good Sir", fstCheep);
    }*/
}