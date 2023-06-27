using System.Xml;

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
    public static async Task<string> Format(string xmlToFormat)
    {
        if (xmlToFormat is null)
        {
            throw new ArgumentNullException(nameof(xmlToFormat));
        }

        if (String.IsNullOrWhiteSpace(xmlToFormat))
        {
            return String.Empty;
        }

        try
        {
            XmlDocument doc = new();
            doc.LoadXml(xmlToFormat);

            using MemoryStream outputStream = new();
            doc.Save(outputStream);
            outputStream.Seek(0, SeekOrigin.Begin);

            using StreamReader reader = new(outputStream);
            var formattedXml = await reader.ReadToEndAsync();

            return formattedXml;
        }
        catch (XmlException)
        {
            await Console.Error.WriteLineAsync($"Failed to format xml:\n{xmlToFormat}");
            return xmlToFormat;
        }
    }
    
    #endregion
}
