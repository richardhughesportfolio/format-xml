using System.Text;
using System.Text.Unicode;
using System.Xml;
using System.Xml.Serialization;

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

        XmlDocument doc = new();
        doc.LoadXml(xmlToFormat);

        using MemoryStream outputStream = new();
        doc.Save(outputStream);

        using StreamReader reader = new(outputStream);
        var formattedXml = await reader.ReadToEndAsync();

        return formattedXml;
    }
    
    #endregion
}
