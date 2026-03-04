using DspicoThemeForms.Core.Enums;
using DspicoThemeForms.Core.ThemeNormalizationLayer;

namespace DspicoThemeForms.Core.ThemeImporters.Importers;

/// <summary>
/// Provides an implementation of the IThemeImporter interface for TwiLightMenu themes.
/// </summary>
/// <remarks>This class identifies TwiLightMenu as a supported theme type but does not currently support importing
/// functionality. All import-related methods return default values, indicating that TwiLightMenu theme import is not
/// available.</remarks>
public class TwiLightMenuThemeImporter : IThemeImporter
{
    public EThemeType Name => EThemeType.TwiLightMenu;
    public NormalizedTheme? Import(string Folderpath)
    {
        return null;
    }

    public bool CanImport(string Folderpath, EgatesFormat format = EgatesFormat.AND)
    {
        return false;
    }
}
