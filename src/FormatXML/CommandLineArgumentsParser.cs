namespace FormatXML;

/// <summary>
/// CommandLineArgumentsParser parses command line arguments and sets flags for known arguments.
/// Default values for arguments is that they are not set. Unknown arguments are ignored. Arguments
/// are not case sensitive.
///
/// The known arguments are:
///
/// -version/-v
/// -help/-h
/// -strict/-s
///
/// The meanings of these arguments is defined by the application.
///
/// As there are no values set for arguments, this class simply checks for the arguments existence.
///
/// Only a single dash `-` prefix is required for an argument. Extra dashes are ignored.
/// </summary>
public class CommandLineArgumentsParser
{
    #region Properties

    /// <summary>
    /// Set by the -strict argument
    /// </summary>
    public bool Strict { get; private set; }

    /// <summary>
    /// Set by the -version argument
    /// </summary>
    public bool Version { get; private set; }

    /// <summary>
    /// Set by the -help argument
    /// </summary>
    public bool Help { get; private set; }

    #endregion
    
    #region Public Methods

    /// <summary>
    /// Constructs this object and parses the passed arguments.
    /// </summary>
    /// <param name="arguments">The arguments to parse</param>
    public CommandLineArgumentsParser(string arguments)
    {
        if (arguments is null)
        {
            throw new ArgumentNullException(nameof(arguments));
        }

        this.ParseArguments(arguments);
    }
    
    #endregion

    #region Private Methods

    /// <summary>
    /// Parses the passed arguments and sets related properties if arguments are found
    /// </summary>
    /// <param name="arguments">The arguments to pass</param>
    private void ParseArguments(string arguments)
    {
        this.Strict = CommandLineArgumentsParser.DoesContainArgument(
            new() { "strict", "s" },
            arguments);

        this.Help = CommandLineArgumentsParser.DoesContainArgument(
            new() { "help", "h" },
            arguments);

        this.Version = CommandLineArgumentsParser.DoesContainArgument(
            new() { "version", "v" },
            arguments);
    }

    /// <summary>
    /// Returns true if one of the passed argument variants is in the passed arguments.
    /// Arguments are not case sensitive.
    ///
    /// Each variant is checked against one dash `-`. Extra dashes are ignored.
    /// </summary>
    /// <param name="variants">The variants of the argument to check for</param>
    /// <param name="arguments">The arguments to check</param>
    /// <returns>True if one of the variants is found, else false</returns>
    private static bool DoesContainArgument(List<string> variants, string arguments)
    {
        var loweredArguments = arguments.ToLower();

        foreach (var variant in variants)
        {
            var loweredVariant = variant.ToLower();
            if (loweredArguments.Contains($"-{loweredVariant}"))
            {
                return true;
            }
        }

        return false;
    }

    #endregion
}
