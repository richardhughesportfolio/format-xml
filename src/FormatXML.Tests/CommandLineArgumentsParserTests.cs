using Xunit;
using Assert = Xunit.Assert;

namespace FormatXML.Tests;

public class CommandLineArgumentsParserTests
{
    #region Test Methods

    #region Constructor

    [Fact]
    public void Constructor_NullCommandLineArguments_ThrowsArgumentNullException()
    {
        string commandLineArguments = null;

        Assert.Throws<ArgumentNullException>(() => new CommandLineArgumentsParser(commandLineArguments));
    }

    #endregion

    #region Behavior Tests

    [Theory]
    [InlineData("--strict")]
    [InlineData("-s")]
    public void GivenStrictArgumentSetsStrictProperty(string commandLineArguments)
    {
        CommandLineArgumentsParser parser = new(commandLineArguments);

        Assert.True(parser.Strict);
    }

    [Theory]
    [InlineData("--version")]
    [InlineData("-v")]
    public void GivenVersionArgumentSetsVersionProperty(string commandLineArguments)
    {
        CommandLineArgumentsParser parser = new(commandLineArguments);

        Assert.True(parser.Version);
    }

    [Theory]
    [InlineData("--help")]
    [InlineData("-h")]
    public void GivenHelpArgumentSetsHelpProperty(string commandLineArguments)
    {
        CommandLineArgumentsParser parser = new(commandLineArguments);

        Assert.True(parser.Help);
    }

    #endregion

    #endregion
}
