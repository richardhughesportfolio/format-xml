namespace FormatXML;

/// <summary>
/// Formatter formats XML into a set standard. There is no configuration for how the output
/// should look.
/// </summary>
public static class Formatter
{
    #region Public Methods

    /// <summary>
    /// Formats the passed XML.
    ///
    /// If empty XML is passed, an empty output is returned.
    /// </summary>
    /// <param name="xmlToFormat"></param>
    /// <returns></returns>
    public static string Format(string xmlToFormat)
    {
        if (xmlToFormat is null)
        {
            throw new ArgumentNullException(nameof(xmlToFormat));
        }

        if (xmlToFormat.Length <= 0)
        {
            return String.Empty;
        }

        return String.Empty;
    }
    
    #endregion
}
