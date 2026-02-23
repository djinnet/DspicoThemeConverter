using DspicoThemeForms.Core.Enums;

namespace DspicoThemeForms.Core.Exceptions;

public class ThemeTypeNotSupportedException : NotSupportedException
{
    public EThemeType? Theme { get; }

    public ThemeTypeNotSupportedException(string message, EThemeType theme) : base(message)
    {
        Theme = theme;
    }

    public ThemeTypeNotSupportedException(string message, Exception inner, EThemeType theme) : base(message, inner)
    {
        Theme = theme;
    }
}
