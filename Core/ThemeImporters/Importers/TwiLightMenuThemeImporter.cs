using DspicoThemeForms.Core.Enums;
using DspicoThemeForms.Core.ThemeNormalizationLayer;

namespace DspicoThemeForms.Core.ThemeImporters.Importers;

public class TwiLightMenuThemeImporter : IThemeImporter
{
    public EThemeType Name => EThemeType.TwiLightMenu;
    public NormalizedTheme? Import(string Folderpath)
    {
        return null;
    }
}
