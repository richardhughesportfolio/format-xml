using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace FormatXML.Tests;

public class FormatterTests
{
    #region Properties

    /// <summary>
    /// The relative path to the XML samples
    /// </summary>
    private const string FormatSamplesPath = "TestData/FormatSamples/";

    /// <summary>
    /// The pattern to find unformatted files
    /// </summary>
    private const string UnformattedFileSearchPattern = "*_Unformatted.xml";
    
    /// <summary>
    /// The pattern to find formatted files
    /// </summary>
    private const string FormattedFileSearchPattern = "*_Formatted.xml";

    #endregion

    #region Properties

    /// <summary>
    /// Writes to stdout
    /// </summary>
    private ITestOutputHelper OutputHelper { get; set; }

    #endregion
    
    #region Test Methods

    #region Format

    [Fact]
    public async Task Format_NullXmlToFormat_ThrowsArgumentNullException()
    {
        string? input = null;

        await Assert.ThrowsAsync<ArgumentNullException>(() => Formatter.Format(input));
    }

    #endregion

    #region Behavior Tests

    [Fact]
    public async Task GivenEmptyXmlEmptyOutputIsReturned()
    {
        var input = String.Empty;

        var result = await Formatter.Format(input);
        
        var expected = String.Empty;

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task FormatsXmlCorrectly()
    {
        var paths = this.GetFormatSamplePaths();
        Assert.NotEmpty(paths);

        foreach (var path in paths)
        {
            this.OutputHelper.WriteLine($"Testing `{path.unformattedPath}` against `{path.formattedPath}`.");

            var unformattedXml = await File.ReadAllTextAsync(path.unformattedPath);
            var formattedXml = await File.ReadAllTextAsync(path.formattedPath);

            var result = await Formatter.Format(unformattedXml);

            Assert.Equal(formattedXml, result);
        }
    }

    [Fact]
    public async Task GivenInvalidXmlThatSameXmlIsReturned()
    {
        var invalidXml = "<invalid xml...";

        var result = await Formatter.Format(invalidXml);

        var expected = invalidXml;
        Assert.Equal(expected, result);
    }

    #endregion

    #endregion

    #region Helper Methods

    public FormatterTests(ITestOutputHelper outputHelper)
    {
        this.OutputHelper = outputHelper;
    }
    
    /// <summary>
    /// Returns the paths to all XML samples to test
    /// </summary>
    /// <returns>A list of the paths to the unformatted and formatted samples</returns>
    private IEnumerable<(string unformattedPath, string formattedPath)> GetFormatSamplePaths()
    {
        var basePath = Path.GetDirectoryName(typeof(FormatterTests).Assembly.Location);
        var formatSamplesPath = Path.Combine(basePath, FormatterTests.FormatSamplesPath);

        // get the paths then sort alphabetically so help match up the formatted and unformatted
        // files
        var unformattedPaths = Directory.GetFiles(
            formatSamplesPath,
            FormatterTests.UnformattedFileSearchPattern).Order().ToList();

        var formattedPaths = Directory.GetFiles(
            formatSamplesPath,
            FormatterTests.FormattedFileSearchPattern).Order().ToList();

        Assert.Equal(unformattedPaths.Count, formattedPaths.Count);

        List<(string, string)> samplePaths = new();

        for (var i = 0; i < unformattedPaths.Count; ++i)
        {
            samplePaths.Add((unformattedPaths[i], formattedPaths[i]));
        }

        return samplePaths;
    }

    #endregion
}
