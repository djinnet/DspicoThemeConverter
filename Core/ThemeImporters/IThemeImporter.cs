using DspicoThemeForms.Core.Enums;
using DspicoThemeForms.Core.ThemeNormalizationLayer;
namespace DspicoThemeForms.Core.ThemeImporters;

public interface IThemeImporter
{
    EThemeType Name { get; }
    NormalizedTheme? Import(string Folderpath);
}
