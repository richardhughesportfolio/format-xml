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
    
    #endregion
    
    #region Public Methods

    /// <summary>
    /// Creates this class
    /// </summary>
    /// <param name="commandLineArguments">The command line arguments that control how this application should behave</param>
    /// <param name="stdin">The stream to use for `stdin`</param>
    /// <param name="stdout">The stream to use for `stdout`</param>
    public Application(
        string commandLineArguments,
        TextReader stdin,
        TextWriter stdout)
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

        this.Stdin = stdin;
        this.Stdout = stdout;
    }

    /// <summary>
    /// Runs this application based on the command line arguments passed into the constructor.
    /// </summary>
    public async Task Run()
    {
        var inputXml = await this.Stdin.ReadToEndAsync();
        var result = await Formatter.Format(inputXml);

        await this.Stdout.WriteAsync(result);
        await this.Stdout.FlushAsync();
    }
    
    #endregion
}
