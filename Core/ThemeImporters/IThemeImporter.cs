using DspicoThemeForms.Core.Enums;
using DspicoThemeForms.Core.ThemeNormalizationLayer;
namespace DspicoThemeForms.Core.ThemeImporters;

public interface IThemeImporter
{
    EThemeType Name { get; }
    bool CanImport(string Folderpath, EgatesFormat format = EgatesFormat.AND);
    NormalizedTheme? Import(string Folderpath);
}
