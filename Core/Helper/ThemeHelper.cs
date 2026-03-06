using DspicoThemeForms.Core.Constants;
using DspicoThemeForms.Core.DspicoExporter;
using DspicoThemeForms.Core.Runners;
using DspicoThemeForms.Core.ThemeNormalizationLayer;
using Serilog;

namespace DspicoThemeForms.Core.Helper;

/// <summary>
/// Provides utility methods for managing and validating themes, including command presence checks, theme JSON creation,
/// image processing reporting, and theme identification.
/// </summary>
/// <remarks>This static class centralizes common theme-related operations to facilitate consistent theme
/// management across the application. Methods in this class help ensure themes are properly configured, support
/// serialization to JSON, and provide feedback on image processing and theme type detection. Use these utilities to
/// streamline theme validation and setup workflows.</remarks>
public static class ThemeHelper
{
    /// <summary>
    /// Checks whether all expected command keys are present in the specified dictionary for the given theme type.
    /// </summary>
    /// <remarks>The method performs different checks depending on the theme's origin type. For 'None' and
    /// 'Auto_Detect', the check is skipped. For 'DSpico', all standard commands are required. For 'YSMenu' and
    /// 'AKMenu', only 'topbg' and 'bottombg' are required. For other theme types, the check is not
    /// implemented.</remarks>
    /// <param name="Commands">A dictionary containing command names as keys and their presence as boolean values. The method verifies the
    /// existence of required commands based on the theme type.</param>
    /// <param name="theme">The theme context that determines which commands are expected. The check adapts to the theme's origin type.</param>
    /// <returns>A string indicating the result of the command presence check. The return value specifies missing commands,
    /// confirms all are present, or indicates that the check was skipped or not implemented for certain theme types.</returns>
    public static string IsAllCommandsPresent(this IDictionary<string, bool> Commands, NormalizedTheme theme)
    {
        string[] expectedCommandsNames = ["topbg", "bottombg", "bannerListCell", "bannerListCellSelected", "gridCell", "gridCellSelected", "scrim"];

        List<string> missingCommands = expectedCommandsNames.Where(cmd => !Commands.ContainsKey(cmd)).ToList();

        switch (theme.OriginTheme)
        {
            case Enums.EThemeType.None:
                return "Origin theme is None, skipping command presence check.";
            case Enums.EThemeType.Auto_Detect:
                return "Origin theme is Auto_Detect, skipping command presence check.";
            case Enums.EThemeType.DSpico:
                {
                    if (missingCommands.Count == 0)
                    {
                        return "All expected commands are present.";
                    }
                    else
                    {
                        return $"Missing commands: {string.Join(", ", missingCommands)}";
                    }
                }
            case Enums.EThemeType.YSMenu:
            case Enums.EThemeType.AKMenu:
                {
                    // For YSMenu and AKMenu has only two expected commands: "topbg" and "bottombg" since their structure only support two images.
                    string[] expectedCommandsForYSAndAK = ["topbg", "bottombg"];
                    List<string> missingCommandsForYSAndAK = expectedCommandsForYSAndAK.Where(cmd => !Commands.ContainsKey(cmd)).ToList();
                    if (missingCommandsForYSAndAK.Count == 0)
                    {
                        return "All expected commands are present for YSMenu/AKMenu.";
                    }
                    else
                    {
                        return $"Missing commands for YSMenu/AKMenu: {string.Join(", ", missingCommandsForYSAndAK)}";
                    }
                }
            case Enums.EThemeType.Moonshell:
            case Enums.EThemeType.TwiLightMenu:
            default:
                return "Command presence check is not implemented for this theme type.";
        }

    }

    /// <summary>
    /// Creates a JSON file representing the specified theme and writes it to the theme folder. If a theme.json file
    /// already exists at the target location, it will be overwritten.
    /// </summary>
    /// <remarks>If the theme.json file already exists in the specified folder, it will be replaced without
    /// prompting. Any errors encountered during file creation or serialization are logged using the provided delegate
    /// and result in a return value of false.</remarks>
    /// <param name="theme">The theme object containing the properties to be serialized into the theme.json file.</param>
    /// <param name="themeFolderPath">The path to the folder where the theme.json file will be created or overwritten. Must be a valid and accessible
    /// directory.</param>
    /// <param name="_log">An action delegate used to log informational, warning, or error messages during the execution of the method.</param>
    /// <param name="_options">The options to customize the JSON serialization process, such as formatting and property naming policies.</param>
    /// <returns>true if the theme.json file was successfully created or overwritten; otherwise, false if an error occurred
    /// during the process.</returns>
    public static bool CreateThemeJson(this NormalizedTheme theme, string themeFolderPath, ILogger _log, System.Text.Json.JsonSerializerOptions _options)
    {
        try
        {
            _log.Information("Writing theme.json...");
            string themeJsonPath = Path.Combine(themeFolderPath, FilesContants.ThemeJsonFileName);

            DSpicoThemeJson json = new()
            {
                Name = theme.Name,
                Description = theme.Description,
                Author = theme.Author,
                PrimaryColor = new PrimaryColor
                {
                    R = theme.PrimaryColor.R,
                    G = theme.PrimaryColor.G,
                    B = theme.PrimaryColor.B
                },
                DarkTheme = theme.DarkTheme,
                Type = "custom"
            };


            string themeJsonContent = System.Text.Json.JsonSerializer.Serialize(json, _options);

            if (File.Exists(themeJsonPath))
            {
                _log.Warning($"Warning: theme.json already exists at {themeJsonPath} and will be overwritten.");
            }

            File.WriteAllText(themeJsonPath, themeJsonContent);
            return true;
        }
        catch (Exception ex)
        {
            _log.Error($"Error creating theme.json: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Reports the outcome of ptexconv image processing based on the specified conversion result.
    /// </summary>
    /// <remarks>This method evaluates the conversion result and logs a message indicating whether no images,
    /// all images, some images, or an unexpected set of images were processed. Use this method to provide clear
    /// feedback to users or logs after attempting ptexconv image conversion.</remarks>
    /// <param name="value">The conversion result that indicates the status of ptexconv processing for the current operation.</param>
    /// <param name="_log">An action delegate used to log messages describing the processing outcome.</param>
    public static void ReportPtexConvResult(this ConversionResult value, ILogger _log)
    {

        if (value.DidNoneCommandRan())
        {
            _log.Warning("No images were processed with ptexconv. Please check if the theme contains any images.");
        }
        else if (value.DidAllCommandRan())
        {
            _log.Information("All images were processed with ptexconv successfully.");
        }
        else if (value.DidAnyCommandRan())
        {

            _log.Error("Some images were processed with ptexconv. Please check the log for details.");
        }
        else
        {
            _log.Error("Unexpected result from ptexconv processing. Please check the log for details.");
        }
    }

    /// <summary>
    /// Determines whether the specified folder contains a 'custom.ini' file with a line that specifies 'text =
    /// acekard'.
    /// </summary>
    /// <remarks>This method checks for the existence of a specific configuration in the 'custom.ini' file,
    /// which is used to identify the AkMenu theme.</remarks>
    /// <param name="folderPath">The path to the folder to check for the presence of the 'custom.ini' file.</param>
    /// <returns>true if the 'custom.ini' file exists and contains the line 'text = acekard'; otherwise, false.</returns>
    public static bool FindingAkMenuTheme(this string folderPath)
    {
        //if folder contains a file named "custom.ini" and within that file there's a line with "text = acekard"
        string customIniPath = Path.Combine(folderPath, "custom.ini");
        if (File.Exists(customIniPath))
        {
            string[] lines = File.ReadAllLines(customIniPath);
            if (lines.Any(line => line.Trim().Equals("text = acekard", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
        }
        return false;
    }
}
