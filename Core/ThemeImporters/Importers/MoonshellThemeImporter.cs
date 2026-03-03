using DspicoThemeForms.Core.Enums;
using DspicoThemeForms.Core.ThemeNormalizationLayer;

namespace DspicoThemeForms.Core.ThemeImporters.Importers;

public class MoonshellThemeImporter : IThemeImporter
{
    public EThemeType Name => EThemeType.Moonshell;
    public NormalizedTheme? Import(string Folderpath)
    {
        return null;
    }

    public bool CanImport(string Folderpath, EgatesFormat format = EgatesFormat.AND)
    {
        return false;
    }
}
