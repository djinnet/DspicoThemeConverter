using DspicoThemeForms.Core.Enums;
using DspicoThemeForms.Core.Exceptions;
using DspicoThemeForms.Core.Helper;
using DspicoThemeForms.Core.ThemeImporters.Importers;
namespace DspicoThemeForms.Core.ThemeImporters;

/// <summary>
/// Provides factory methods for creating instances of theme importers based on specified theme resource paths and
/// types.
/// </summary>
/// <remarks>ThemeImporterFactory enables both automatic detection and explicit specification of theme types when
/// importing themes. It ensures that the correct importer is selected according to the provided parameters, and
/// supports multiple theme formats. For DSpico themes, only the source format is supported for import; exported formats
/// are not supported. Exceptions are thrown for invalid arguments or unsupported theme types.</remarks>
public class ThemeImporterFactory
{
    /// <summary>
    /// Creates an instance of an <see cref="IThemeImporter"/> for the specified theme resource path, optionally using a
    /// provided theme type override.
    /// </summary>
    /// <remarks>If <paramref name="themeOverrideType"/> is not provided, the method will attempt to determine
    /// the theme type automatically based on the specified path.</remarks>
    /// <param name="path">The file path to the theme resource. This parameter must not be null or empty.</param>
    /// <param name="themeOverrideType">An optional theme type to use for importing. If not specified, the method will attempt to auto-detect the theme
    /// type. Must not be <see cref="EThemeType.None"/>.</param>
    /// <returns>An instance of <see cref="IThemeImporter"/> if the theme importer is successfully created; otherwise, <see
    /// langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="path"/> is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="themeOverrideType"/> is <see cref="EThemeType.None"/>.</exception>
    public static IThemeImporter? Create(string path, EThemeType? themeOverrideType)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentNullException(nameof(path));

        if (EThemeType.None == themeOverrideType)
        {
            throw new ArgumentException("Theme type must be provided.", nameof(themeOverrideType));
        }

        themeOverrideType ??= EThemeType.Auto_Detect;

        if (themeOverrideType != EThemeType.Auto_Detect)
        {
            return CreateFromOverride(path, (EThemeType)themeOverrideType);
        }

        return AutoDetect(path);
    }

    /// <summary>
    /// Creates an instance of a theme importer for the specified theme type using the provided folder path. This method
    /// is intended for override scenarios where the theme type is explicitly specified.
    /// </summary>
    /// <remarks>For DSpico themes, only the source format (PNG files) is supported; attempting to import the
    /// exported format (BIN files) will result in an exception. This method does not perform auto-detection of theme
    /// type and requires an explicit type to be provided.</remarks>
    /// <param name="folderPath">The path to the folder containing the theme files. The contents and structure of this folder are used to
    /// determine the appropriate importer for the theme type.</param>
    /// <param name="type">The type of theme to import. Must be a valid theme type other than 'Auto_Detect'.</param>
    /// <returns>An instance of <see cref="IThemeImporter"/> that corresponds to the specified theme type and folder contents.
    /// Returns <see langword="null"/> if the theme type is not supported.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="type"/> is set to <see cref="EThemeType.Auto_Detect"/>, which is not valid for
    /// override operations.</exception>
    /// <exception cref="DSpicoExportedException">Thrown when the folder path appears to contain an exported DSpico theme, which is not supported for import.</exception>
    /// <exception cref="ThemeTypeNotSupportedException">Thrown when the specified theme type is not supported by the importer factory.</exception>
    private static IThemeImporter? CreateFromOverride(string folderPath, EThemeType type)
    {
        if (type == EThemeType.Auto_Detect)
        {
            throw new ArgumentException("Theme type cannot be 'Auto-detect' when using override.", nameof(type));
        }

        if (type == EThemeType.DSpico)
        {
            // For DSpico, we only support the source format (png files), not the exported format (bin files)
            if (LooksLikeDSpicoSource(folderPath))
            {
                return new DSpicoThemeImporter();
            }

            if (LooksLikeDSpicoExported(folderPath))
            {
                throw new DSpicoExportedException();
            }
        }

        return type switch
        {
            EThemeType.YSMenu => new YSMenuThemeImporter(),
            EThemeType.AKMenu => new AKMenuThemeImporter(),
            EThemeType.Moonshell => new MoonshellThemeImporter(),
            EThemeType.TwiLightMenu => new TwiLightMenuThemeImporter(),
            _ => throw new ThemeTypeNotSupportedException($"Theme type '{type}' is not supported yet.", type),
        };
    }

    /// <summary>
    /// Attempts to automatically detect the theme type based on the contents of the specified folder and returns an
    /// appropriate theme importer instance.
    /// </summary>
    /// <remarks>This method examines the folder contents to identify known theme formats. If the folder
    /// matches a supported theme type, the corresponding importer is returned. If the folder contains an unsupported
    /// exported DSpico theme, an exception is thrown. If no supported theme type is detected, a
    /// ThemeTypeNotSupportedException is thrown to prompt manual selection.</remarks>
    /// <param name="folderPath">The path to the folder containing theme files to be analyzed for theme type detection. This value cannot be null
    /// or empty.</param>
    /// <returns>An instance of a class implementing the IThemeImporter interface that corresponds to the detected theme type, or
    /// null if no theme type is detected.</returns>
    /// <exception cref="Exception">Thrown if the specified folder contains an exported DSpico theme, which is not supported for import.</exception>
    /// <exception cref="ThemeTypeNotSupportedException">Thrown if the theme type cannot be determined automatically, indicating that manual selection is required.</exception>
    private static IThemeImporter? AutoDetect(string folderPath)
    {
        if (LooksLikeYsMenu(folderPath))
        {
            return new YSMenuThemeImporter();
        }

        if (LooksLikeAKMenu(folderPath))
        {
            return new AKMenuThemeImporter();
        }

        if (LooksLikeMoonshell(folderPath))
        {
            return new MoonshellThemeImporter();
        }

        if (LooksLikeTwiLightMenu(folderPath))
        {
            return new TwiLightMenuThemeImporter();
        }

        if (LooksLikeDSpicoSource(folderPath))
        {
            return new DSpicoThemeImporter();
        }

        if (LooksLikeDSpicoExported(folderPath))
        {
            throw new Exception("This looks like an exported DSpico theme. We only supported for the DSpico theme source (which means the png files).");
        }

        throw new ThemeTypeNotSupportedException("Could not auto-detect theme type. Please select one manually.", EThemeType.Auto_Detect);
    }

    /// <summary>
    /// Determines whether the specified folder path resembles a menu theme associated with AKMenu.
    /// </summary>
    /// <param name="folderPath">The path of the folder to evaluate for menu theme characteristics. This parameter cannot be null or empty.</param>
    /// <returns>true if the folder path resembles an AK menu theme; otherwise, false.</returns>
    private static bool LooksLikeAKMenu(string folderPath) => folderPath.FindingAkMenuTheme();

    /// <summary>
    /// Determines whether the specified folder contains a theme configuration file named 'theme.ini'.
    /// </summary>
    /// <remarks>This method is useful for validating the presence of theme settings before applying
    /// them.</remarks>
    /// <param name="folderPath">The path to the folder to check for the presence of the 'theme.ini' file. This parameter must not be null or
    /// empty.</param>
    /// <returns>true if the 'theme.ini' file exists in the specified folder; otherwise, false.</returns>
    private static bool LooksLikeTwiLightMenu(string folderPath) => File.Exists(Path.Combine(folderPath, "theme.ini"));

    /// <summary>
    /// Determines whether the specified folder contains a YSMenu configuration file.
    /// </summary>
    /// <remarks>This method checks for the presence of the YSMenu.ini file, which is essential for the YSMenu
    /// functionality. Ensure that the provided folder path is valid and accessible.</remarks>
    /// <param name="folderPath">The path to the folder where the YSMenu.ini file is expected to be located. This parameter cannot be null or
    /// empty.</param>
    /// <returns>true if the YSMenu.ini file exists in the specified folder; otherwise, false.</returns>
    private static bool LooksLikeYsMenu(string folderPath) => File.Exists(Path.Combine(folderPath, "YSMenu.ini"));

    /// <summary>
    /// Determines whether the specified folder contains a file named 'sndeff.dat', indicating a possible Moonshell
    /// installation.
    /// </summary>
    /// <remarks>Use this method to validate whether a directory appears to contain Moonshell-related files
    /// before performing further operations.</remarks>
    /// <param name="folderPath">The path to the folder to check for the presence of the 'sndeff.dat' file. This parameter must not be null or
    /// empty.</param>
    /// <returns>true if the 'sndeff.dat' file exists in the specified folder; otherwise, false.</returns>
    private static bool LooksLikeMoonshell(string folderPath) => File.Exists(Path.Combine(folderPath, "sndeff.dat"));

    /// <summary>
    /// Determines whether the specified folder contains the required image files for a DSpico source.
    /// </summary>
    /// <remarks>This method checks for the existence of specific image files that are essential for
    /// identifying a DSpico source.</remarks>
    /// <param name="folderPath">The path to the folder that is checked for the presence of 'topbg.png' and 'bottombg.png'.</param>
    /// <returns>true if both 'topbg.png' and 'bottombg.png' exist in the specified folder; otherwise, false.</returns>
    private static bool LooksLikeDSpicoSource(string folderPath) => File.Exists(Path.Combine(folderPath, "topbg.png")) && File.Exists(Path.Combine(folderPath, "bottombg.png"));

    /// <summary>
    /// Determines whether the specified folder contains the required files for a DSpico export.
    /// </summary>
    /// <remarks>This method is typically used to validate that a folder is suitable for DSpico export
    /// operations by checking for the existence of essential files.</remarks>
    /// <param name="folderPath">The path to the folder to check for the presence of DSpico export files.</param>
    /// <returns>true if the folder contains 'topbg.bin', 'bottombg.bin', and 'theme.json'; otherwise, false.</returns>
    private static bool LooksLikeDSpicoExported(string folderPath) => File.Exists(Path.Combine(folderPath, "topbg.bin")) && File.Exists(Path.Combine(folderPath, "bottombg.bin")) && File.Exists(Path.Combine(folderPath, "theme.json"));

}

