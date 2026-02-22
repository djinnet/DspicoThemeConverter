using DspicoThemeForms.Core.Enums;
using DspicoThemeForms.Core.ThemeNormalizationLayer;
using System;
using System.Collections.Generic;
using System.Text;

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
