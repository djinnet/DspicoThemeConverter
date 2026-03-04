using DspicoThemeForms.Core.Enums;
using DspicoThemeForms.Core.ThemeNormalizationLayer;

namespace DspicoThemeForms.Core.ThemeImporters.Importers;

/// <summary>
/// Provides functionality to import Moonshell themes by implementing the IThemeImporter interface.
/// </summary>
/// <remarks>Use this class to determine if a Moonshell theme can be imported from a specified folder and to
/// attempt the import. The importer is specific to the Moonshell theme format and does not support other theme
/// types. And this does not currently support importing functionality.</remarks>
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
