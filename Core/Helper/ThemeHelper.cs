using DspicoThemeForms.Core.Constants;
using DspicoThemeForms.Core.DspicoExporter;
using DspicoThemeForms.Core.Runners;
using DspicoThemeForms.Core.ThemeNormalizationLayer;

namespace DspicoThemeForms.Core.Helper;

public static class ThemeHelper
{
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

    public static bool CreateThemeJson(this NormalizedTheme theme, string themeFolderPath, Action<string> _log, System.Text.Json.JsonSerializerOptions _options)
    {
        try
        {
            _log("Writing theme.json...");
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
                _log($"Warning: theme.json already exists at {themeJsonPath} and will be overwritten.");
            }

            File.WriteAllText(themeJsonPath, themeJsonContent);
            return true;
        }
        catch (Exception ex)
        {
            _log($"Error creating theme.json: {ex.Message}");
            return false;
        }
    }

    public static void ReportPtexConvResult(this ConversionResult value, Action<string> _log)
    {

        if (value.DidNoneCommandRan())
        {
            _log("No images were processed with ptexconv. Please check if the theme contains any images.");
        }
        else if (value.DidAllCommandRan())
        {
            _log("All images were processed with ptexconv successfully.");
        }
        else if (value.DidAnyCommandRan())
        {

            _log("Some images were processed with ptexconv. Please check the log for details.");
        }
        else
        {
            _log("Unexpected result from ptexconv processing. Please check the log for details.");
        }
    }

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
