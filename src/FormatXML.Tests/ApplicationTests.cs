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

        Assert.Throws<ArgumentNullException>(() => new Application(commandLineArguments));
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
