namespace DspicoThemeForms.Core.Exceptions;

/// <summary>
/// Represents an error that occurs when an exported DSpico theme is provided, which is not supported by the converter.
/// </summary>
/// <remarks>This exception is thrown to indicate that only DSpico theme source files (PNG files) are supported,
/// and exported themes cannot be processed. Use this exception to handle cases where an unsupported theme format is
/// encountered.</remarks>
public class DSpicoExportedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the DSpicoExportedException class with a predefined error message indicating that
    /// the DSpico theme is only supported for the theme source.
    /// </summary>
    /// <remarks>This exception is thrown when an operation is attempted on an exported DSpico theme, which is
    /// not supported. It is intended to inform the caller that only the DSpico theme source, specifically the PNG
    /// files, are supported.</remarks>
    public DSpicoExportedException() : base("This looks like an exported DSpico theme. We only supported for the DSpico theme source (which means the png files).")
    {
    }

    public DSpicoExportedException(string message) : base(message)
    {
    }
    public DSpicoExportedException(string message, Exception inner) : base(message, inner)
    {
    }
}
