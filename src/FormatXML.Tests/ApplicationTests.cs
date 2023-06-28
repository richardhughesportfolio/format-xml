using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace FormatXML.Tests;

public class ApplicationTests
{
    #region Properties

    /// <summary>
    /// Writes to stdout
    /// </summary>
    private ITestOutputHelper OutputHelper { get; set; }

    #endregion

    #region Test Methods

    #region Constructor

    [Fact]
    public void Constructor_NullCommandLineArguments_ThrowsArgumentNullException()
    {
        string commandLineArguments = null;
        var stdin = Console.In;
        var stdout = Console.Out;

        Assert.Throws<ArgumentNullException>(() => new Application(commandLineArguments, stdin, stdout));
    }

    [Fact]
    public void Constructor_NullStdin_ThrowsArgumentNullException()
    {
        var commandLineArguments = String.Empty;
        TextReader stdin = null;
        var stdout = Console.Out;

        Assert.Throws<ArgumentNullException>(() => new Application(commandLineArguments, stdin, stdout));
    }

    [Fact]
    public void Constructor_NullStdout_ThrowsArgumentNullException()
    {
        var commandLineArguments = String.Empty;
        var stdin = Console.In;
        TextWriter stdout = null;

        Assert.Throws<ArgumentNullException>(() => new Application(commandLineArguments, stdin, stdout));
    }

    #endregion

    #region Behavior Tests

    [Fact]
    public void GivenEmptyStdinNothingIsWrittenToStdout()
    {
        var commandLineArguments = String.Empty;

        using MemoryStream stdinStream = new();
        using StreamReader stdin = new(stdinStream);

        using MemoryStream stdoutStream = new();
        using StreamWriter stdout = new(stdinStream);

        Application application = new(commandLineArguments, stdin, stdout);
        application.Run();

        var expected = 0;
        Assert.Equal(expected, stdout.BaseStream.Length);
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

        Application application = new(commandLineArguments, stdin, stdout);
        await application.Run();

        var formattedXml = "<tag />";
        var expected = formattedXml;

        var result = await this.ReadStringFromStream(stdoutStream);
        Assert.Equal(expected, result);
    }

    #endregion
    
    #endregion
    
    #region Helper Methods

    public ApplicationTests(ITestOutputHelper outputHelper)
    {
        this.OutputHelper = outputHelper;
    }

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
