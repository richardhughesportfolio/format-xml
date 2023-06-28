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

    #endregion
    
    #endregion
    
    #region Helper Methods

    public ApplicationTests(ITestOutputHelper outputHelper)
    {
        this.OutputHelper = outputHelper;
    }

    #endregion
}
