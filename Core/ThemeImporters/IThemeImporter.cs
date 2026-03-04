using DspicoThemeForms.Core.Enums;
using DspicoThemeForms.Core.ThemeNormalizationLayer;
namespace DspicoThemeForms.Core.ThemeImporters;

/// <summary>
/// Defines a contract for importing themes from a specified folder path and format.
/// </summary>
/// <remarks>Implementations of this interface should provide logic to determine whether a theme can be imported
/// from a given folder and format, and to perform the import operation. The interface supports extensibility for
/// different theme types and formats. Callers should check import capability using CanImport before invoking Import to
/// avoid errors.</remarks>
public interface IThemeImporter
{
    /// <summary>
    /// Gets the theme type associated with the current context.
    /// </summary>
    /// <remarks>This property provides the current theme type, which can be used to customize the appearance
    /// of UI elements based on the selected theme.</remarks>
    EThemeType Name { get; }

    /// <summary>
    /// Determines whether the specified folder can be imported using the given format.
    /// </summary>
    /// <remarks>This method validates the folder's contents and structure to ensure they are compatible with
    /// the specified import format. An exception may be thrown if the folder path is invalid.</remarks>
    /// <param name="Folderpath">The path to the folder to check for import compatibility. This parameter cannot be null or empty.</param>
    /// <param name="format">The format to use for the import operation. Defaults to EgatesFormat.AND if not specified.</param>
    /// <returns>true if the folder can be imported in the specified format; otherwise, false.</returns>
    bool CanImport(string Folderpath, EgatesFormat format = EgatesFormat.AND);

    /// <summary>
    /// Imports a theme from the specified folder path and returns a normalized theme object if successful.
    /// </summary>
    /// <remarks>Ensure that the folder contains all required theme files in the expected format. If the
    /// folder is empty or contains invalid files, the method returns null.</remarks>
    /// <param name="Folderpath">The path to the folder containing the theme files. The path must be valid and accessible.</param>
    /// <returns>A NormalizedTheme object representing the imported theme, or null if the import fails.</returns>
    NormalizedTheme? Import(string Folderpath);
}
