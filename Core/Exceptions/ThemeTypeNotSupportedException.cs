using DspicoThemeForms.Core.Enums;

namespace DspicoThemeForms.Core.Exceptions;

/// <summary>
/// Represents the exception that is thrown when an unsupported theme type is specified.
/// </summary>
/// <remarks>This exception provides access to the unsupported theme type through the Theme property. It is
/// typically thrown when an operation attempts to use a theme type that is not recognized or cannot be applied by the
/// system.</remarks>
public class ThemeTypeNotSupportedException : NotSupportedException
{
    public EThemeType? Theme { get; }

    /// <summary>
    /// Initializes a new instance of the ThemeTypeNotSupportedException class with a specified error message and the
    /// unsupported theme type.
    /// </summary>
    /// <remarks>This exception is thrown when an operation is attempted with a theme type that is not
    /// supported by the application.</remarks>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="theme">The theme type that is not supported, which caused the exception to be thrown.</param>
    public ThemeTypeNotSupportedException(string message, EThemeType theme) : base(message)
    {
        Theme = theme;
    }

    public ThemeTypeNotSupportedException(string message, Exception inner, EThemeType theme) : base(message, inner)
    {
        Theme = theme;
    }
}
