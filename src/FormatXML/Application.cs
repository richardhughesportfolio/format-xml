namespace FormatXML;

/// <summary>
/// Application runs this application. It processes all command line arguments and runs the formatter
/// accordingly.
/// </summary>
public class Application
{
    #region Public Methods

    /// <summary>
    /// Creates this class
    /// </summary>
    /// <param name="commandLineArguments">The command line arguments that control how this application should behave</param>
    public Application(string commandLineArguments)
    {
        if (commandLineArguments is null)
        {
            throw new ArgumentNullException(nameof(commandLineArguments));
        }
    }

    /// <summary>
    /// Runs this application based on the command line arguments passed into the constructor.
    /// </summary>
    public void Run()
    {
        
    }
    
    #endregion
}
