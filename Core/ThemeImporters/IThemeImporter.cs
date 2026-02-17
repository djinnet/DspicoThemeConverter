using DspicoThemeForms.Core.ThemeNormalizationLayer;
namespace DspicoThemeForms.Core.ThemeImporters;
public interface IThemeImporter
{
    string Name { get; }
    NormalizedTheme? Import(string Folderpath);
}
