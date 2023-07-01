using System.Reflection;

namespace FormatXML;

/// <summary>
/// Application runs this application. It processes all command line arguments and runs the formatter
/// accordingly.
/// </summary>
public class Application
{
    #region Properties

    /// <summary>
    /// Reads from `stdin`
    /// </summary>
    private TextReader Stdin { get; }

    /// <summary>
    /// Writes to `stdout`
    /// </summary>
    private TextWriter Stdout { get; }

    /// <summary>
    /// Writes to `stderr`
    /// </summary>
    private TextWriter Stderr { get; }

    /// <summary>
    /// Processes the command line arguments
    /// </summary>
    private CommandLineArgumentsParser CommandLineArguments { get; }

    #endregion
    
    #region Public Methods

    /// <summary>
    /// Creates this class
    /// </summary>
    /// <param name="commandLineArguments">The command line arguments that control how this application should behave</param>
    /// <param name="stdin">The stream to use for `stdin`</param>
    /// <param name="stdout">The stream to use for `stdout`</param>
    /// <param name="stderr">The stream to use for `stderr`</param>
    public Application(
        string commandLineArguments,
        TextReader stdin,
        TextWriter stdout,
        TextWriter stderr)
    {
        if (commandLineArguments is null)
        {
            throw new ArgumentNullException(nameof(commandLineArguments));
        }

        if (stdin is null)
        {
            throw new ArgumentNullException(nameof(stdin));
        }

        if (stdout is null)
        {
            throw new ArgumentNullException(nameof(stdout));
        }

        if (stderr is null)
        {
            throw new ArgumentNullException(nameof(stderr));
        }

        this.Stdin = stdin;
        this.Stdout = stdout;
        this.Stderr = stderr;

        this.CommandLineArguments = new(commandLineArguments);
    }

    /// <summary>
    /// Runs this application based on the command line arguments passed into the constructor.
    /// </summary>
    public async Task<int> Run()
    {
        if (this.CommandLineArguments.Help)
        {
            await this.OutputHelp();

            return 0;
        }

        if (this.CommandLineArguments.Version)
        {
            await this.OutputVersion();

            return 0;
        }

        var inputXml = await this.Stdin.ReadToEndAsync();

        var result = await Formatter.Format(inputXml);
        if (result is null)
        {
            var errorMessage = $"Failed to format xml:\n{inputXml}";
            await this.WriteToStream(errorMessage, this.Stderr);

            await this.WriteToStream(inputXml, this.Stdout);

            return this.CommandLineArguments.Strict ? 1 : 0;
        }

        await this.WriteToStream(result, this.Stdout);

        return 0;
    }
    
    #endregion

    #region Private Methods

    /// <summary>
    /// Writes the passed string to the passed stream
    /// </summary>
    /// <param name="content">The string to write</param>
    /// <param name="writer">The stream to write to</param>
    private async Task WriteToStream(string content, TextWriter writer)
    {
        await writer.WriteAsync(content);
        await writer.FlushAsync();
    }

    /// <summary>
    /// Writes this application's version to stdout
    /// </summary>
    private async Task OutputVersion()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version ?? new(1, 0);
        await this.WriteToStream(version.ToString(), this.Stdout);
    }

    /// <summary>
    /// Writes help text to stdout
    /// </summary>
    private async Task OutputHelp()
    {
        const string helpText =
"""
FormatXML takes XML via `stdin` and outputs the formatted XML via `stdout`. Errors are written to `stderr`.

Empty input results in empty output.

If the XML is invalid or cannot be formatted, an error will be written to `stderr` and the input will be written to `stdout`.

If an error occurs, `fxml` will still return `0`. If you would like a non-zero value returned, you can pass the `--strict` flag.

=====

Usage:
    echo "<xml/>" | fxml [runtime-options] > output.xml
    cat input.xml | fxml [runtime-options] > output.xml

runtime-options:
    --strict/-s    On error, return a non-zero value

Usage:
    fxml [additional-commands]

additional-commands:
    --version/-v   Displays this application's version
    --help/-h      Displays this application's help text

See: https://github.com/richardjhughes/format-xml

""";

        await this.WriteToStream(helpText, this.Stdout);
    }
    
    #endregion
}
