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
    
    #endregion
    
    #region Public Methods

    /// <summary>
    /// Creates this class
    /// </summary>
    /// <param name="commandLineArguments">The command line arguments that control how this application should behave</param>
    /// <param name="stdin">The stream to use for `stdin`</param>
    /// <param name="stdout">The stream to use for `stdout`</param>
    /// <param name="stdout">The stream to use for `stderr`</param>
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
    }

    /// <summary>
    /// Runs this application based on the command line arguments passed into the constructor.
    /// </summary>
    public async Task Run()
    {
        var inputXml = await this.Stdin.ReadToEndAsync();

        var result = await Formatter.Format(inputXml);
        if (result is null)
        {
            var errorMessage = $"Failed to format xml:\n{inputXml}";
            await this.WriteToStream(errorMessage, this.Stderr);

            await this.WriteToStream(inputXml, this.Stdout);
            return;
        }

        await this.WriteToStream(result, this.Stdout);
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

    #endregion
}
