using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace FormatXML.Tests;

public class ApplicationTests
{
    #region Test Methods

    #region Constructor

    [Fact]
    public void Constructor_NullCommandLineArguments_ThrowsArgumentNullException()
    {
        string commandLineArguments = null;
        var stdin = Console.In;
        var stdout = Console.Out;

        Assert.Throws<ArgumentNullException>(() => new Application(commandLineArguments, stdin, stdout, stdout));
    }

    [Fact]
    public void Constructor_NullStdin_ThrowsArgumentNullException()
    {
        var commandLineArguments = String.Empty;
        TextReader stdin = null;
        var stdout = Console.Out;
        var stderr = Console.Error;

        Assert.Throws<ArgumentNullException>(() => new Application(commandLineArguments, stdin, stdout, stdout));
    }

    [Fact]
    public void Constructor_NullStdout_ThrowsArgumentNullException()
    {
        var commandLineArguments = String.Empty;
        var stdin = Console.In;
        TextWriter stdout = null;
        var stderr = Console.Error;

        Assert.Throws<ArgumentNullException>(() => new Application(commandLineArguments, stdin, stdout, stdout));
    }

    [Fact]
    public void Constructor_NullStderr_ThrowsArgumentNullException()
    {
        var commandLineArguments = String.Empty;
        var stdin = Console.In;
        var stdout = Console.Out;
        TextWriter stderr = null;

        Assert.Throws<ArgumentNullException>(() => new Application(commandLineArguments, stdin, stdout, stderr));
    }

    #endregion

    #region Behavior Tests

    [Fact]
    public async Task GivenEmptyStdinNothingIsWrittenToStdout()
    {
        var commandLineArguments = String.Empty;

        using MemoryStream stdinStream = new();
        using StreamReader stdin = new(stdinStream);

        using MemoryStream stdoutStream = new();
        await using StreamWriter stdout = new(stdinStream);

        using MemoryStream stderrStream = new();
        await using StreamWriter stderr = new(stderrStream);

        Application application = new(commandLineArguments, stdin, stdout, stderr);
        await application.Run();

        var expected = 0;
        Assert.Equal(expected, stdout.BaseStream.Length);
    }

    [Fact]
    public async Task GivenEmptyStdinButNotStrictApplicationReturnsZero()
    {
        var commandLineArguments = String.Empty;

        using MemoryStream stdinStream = new();
        using StreamReader stdin = new(stdinStream);

        using MemoryStream stdoutStream = new();
        await using StreamWriter stdout = new(stdinStream);

        using MemoryStream stderrStream = new();
        await using StreamWriter stderr = new(stderrStream);

        Application application = new(commandLineArguments, stdin, stdout, stderr);
        var result = await application.Run();

        var expected = 0;
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GivenValidXmlInStdinFormattedXmlIsWrittenToStdout()
    {
        var commandLineArguments = String.Empty;

        using MemoryStream stdinStream = new();
        using StreamReader stdin = new(stdinStream);

        var unformattedXml = "<tag/>";
        await this.WriteStringToStream(unformattedXml, stdinStream);
        stdinStream.Seek(0, SeekOrigin.Begin);

        using MemoryStream stdoutStream = new();
        await using StreamWriter stdout = new(stdoutStream);

        using MemoryStream stderrStream = new();
        await using StreamWriter stderr = new(stderrStream);

        Application application = new(commandLineArguments, stdin, stdout, stderr);
        await application.Run();

        var formattedXml = "<tag />";
        var expected = formattedXml;

        var result = await this.ReadStringFromStream(stdoutStream);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GivenValidXmlButNotStrictApplicationReturnsZero()
    {
        var commandLineArguments = String.Empty;

        using MemoryStream stdinStream = new();
        using StreamReader stdin = new(stdinStream);

        var unformattedXml = "<tag/>";
        await this.WriteStringToStream(unformattedXml, stdinStream);
        stdinStream.Seek(0, SeekOrigin.Begin);

        using MemoryStream stdoutStream = new();
        await using StreamWriter stdout = new(stdoutStream);

        using MemoryStream stderrStream = new();
        await using StreamWriter stderr = new(stderrStream);

        Application application = new(commandLineArguments, stdin, stdout, stderr);
        var result = await application.Run();

        var expected = 0;
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GivenInvalidXmlInStdinThatSameXmlIsWrittenToStdout()
    {
        var commandLineArguments = String.Empty;

        using MemoryStream stdinStream = new();
        using StreamReader stdin = new(stdinStream);

        var unformattedXml = "<Invalid xml...";
        await this.WriteStringToStream(unformattedXml, stdinStream);
        stdinStream.Seek(0, SeekOrigin.Begin);

        using MemoryStream stdoutStream = new();
        await using StreamWriter stdout = new(stdoutStream);

        using MemoryStream stderrStream = new();
        await using StreamWriter stderr = new(stderrStream);

        Application application = new(commandLineArguments, stdin, stdout, stderr);
        await application.Run();

        var expected = unformattedXml;

        var result = await this.ReadStringFromStream(stdoutStream);
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GivenInvalidXmlInStdinAnErrorIsWrittenToStderr()
    {
        var commandLineArguments = String.Empty;

        using MemoryStream stdinStream = new();
        using StreamReader stdin = new(stdinStream);

        var unformattedXml = "<Invalid xml...";
        await this.WriteStringToStream(unformattedXml, stdinStream);
        stdinStream.Seek(0, SeekOrigin.Begin);

        using MemoryStream stdoutStream = new();
        await using StreamWriter stdout = new(stdoutStream);

        using MemoryStream stderrStream = new();
        await using StreamWriter stderr = new(stderrStream);

        Application application = new(commandLineArguments, stdin, stdout, stderr);
        await application.Run();

        var result = await this.ReadStringFromStream(stderrStream);
        Assert.False(String.IsNullOrWhiteSpace(result));
    }

    [Fact]
    public async Task GivenInvalidXmlButNotStrictApplicationReturnsZero()
    {
        var commandLineArguments = String.Empty;

        using MemoryStream stdinStream = new();
        using StreamReader stdin = new(stdinStream);

        var unformattedXml = "<Invalid xml...";
        await this.WriteStringToStream(unformattedXml, stdinStream);
        stdinStream.Seek(0, SeekOrigin.Begin);

        using MemoryStream stdoutStream = new();
        await using StreamWriter stdout = new(stdoutStream);

        using MemoryStream stderrStream = new();
        await using StreamWriter stderr = new(stderrStream);

        Application application = new(commandLineArguments, stdin, stdout, stderr);
        var result = await application.Run();

        var expected = 0;
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task GivenInvalidXmlButIsStrictApplicationReturnsNonZero()
    {
        var commandLineArguments = "--strict";

        using MemoryStream stdinStream = new();
        using StreamReader stdin = new(stdinStream);

        var unformattedXml = "<Invalid xml...";
        await this.WriteStringToStream(unformattedXml, stdinStream);
        stdinStream.Seek(0, SeekOrigin.Begin);

        using MemoryStream stdoutStream = new();
        await using StreamWriter stdout = new(stdoutStream);

        using MemoryStream stderrStream = new();
        await using StreamWriter stderr = new(stderrStream);

        Application application = new(commandLineArguments, stdin, stdout, stderr);
        var result = await application.Run();

        var expected = 0;
        Assert.NotEqual(expected, result);
    }

    [Fact]
    public async Task GivenHelpCommandLineArgumentHelpIsWrittenToStdout()
    {
        var commandLineArguments = "--help";

        using MemoryStream stdinStream = new();
        using StreamReader stdin = new(stdinStream);

        using MemoryStream stdoutStream = new();
        await using StreamWriter stdout = new(stdoutStream);

        using MemoryStream stderrStream = new();
        await using StreamWriter stderr = new(stderrStream);

        Application application = new(commandLineArguments, stdin, stdout, stderr);
        await application.Run();

        var result = await this.ReadStringFromStream(stdoutStream);
        Assert.False(String.IsNullOrWhiteSpace(result));
    }

    [Fact]
    public async Task GivenVersionCommandLineArgumentVersionIsWrittenToStdout()
    {
        var commandLineArguments = "--version";

        using MemoryStream stdinStream = new();
        using StreamReader stdin = new(stdinStream);

        using MemoryStream stdoutStream = new();
        await using StreamWriter stdout = new(stdoutStream);

        using MemoryStream stderrStream = new();
        await using StreamWriter stderr = new(stderrStream);

        Application application = new(commandLineArguments, stdin, stdout, stderr);
        await application.Run();

        var result = await this.ReadStringFromStream(stdoutStream);
        Assert.False(String.IsNullOrWhiteSpace(result));
    }

    #endregion
    
    #endregion
    
    #region Helper Methods

    /// <summary>
    /// Writes the passed string to the passed stream
    /// </summary>
    /// <param name="content">The string to write</param>
    /// <param name="destination">The stream to write the string to</param>
    private async Task WriteStringToStream(string content, Stream destination)
    {
        // we are purposely not `using` this stream because we don't want to close
        // `destination`
        StreamWriter writer = new(destination);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
    }

    /// <summary>
    /// Reads a string from the passed stream
    /// </summary>
    /// <param name="source">The stream to read the string from</param>
    private async Task<string> ReadStringFromStream(Stream source)
    {
        // we are purposely not `using` this stream because we don't want to close
        // `source`
        StreamReader reader = new(source);
        reader.BaseStream.Seek(0, SeekOrigin.Begin);

        var content = await reader.ReadToEndAsync();
        return content;
    }
    
    #endregion
}
