using DspicoThemeForms.Core.ThemeImporters.Importers;
namespace DspicoThemeForms.Core.ThemeImporters;
public class ThemeImporterFactory
{
    public static IThemeImporter? Create(string path, string? themeOverrideType)
    {
        if (string.IsNullOrEmpty(themeOverrideType))
        {
            throw new ArgumentException("Theme type must be provided.", nameof(themeOverrideType));
        }

        themeOverrideType ??= "Auto-detect";

        if (themeOverrideType != "Auto-detect")
        {
            return CreateFromOverride(path, themeOverrideType);
        }

        return AutoDetect(path);
    }

    private static IThemeImporter? CreateFromOverride(string folderPath, string type)
    {
        if (type == "Auto-detect")
        {
            throw new ArgumentException("Theme type cannot be 'Auto-detect' when using override.", nameof(type));
        }

        if (type == "DSpico")
        {
            // For DSpico, we only support the source format (png files), not the exported format (bin files)
            if (LooksLikeDSpicoSource(folderPath))
            {
                return new DSpicoThemeImporter();
            }

            if (LooksLikeDSpicoExported(folderPath))
            {
                throw new Exception("This looks like an exported DSpico theme. We only supported for the DSpico theme source (which means the png files).");
            }
        }

        return type switch
        {
            "YSMenu" => new YSMenuThemeImporter(),
            "AKMenu" => new AKMenuThemeImporter(),
            "Moonshell" => new MoonshellThemeImporter(),
            "TwiLightMenu" => new TwiLightMenuThemeImporter(),
            _ => throw new NotSupportedException($"Theme type '{type}' is not supported yet."),
        };
    }

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

        throw new NotSupportedException("Could not auto-detect theme type. Please select one manually.");
    }

    private static bool LooksLikeTwiLightMenu(string folderPath)
    {
        // Most common TwiLightMenu indicators
        return File.Exists(Path.Combine(folderPath, "theme.ini"));
    }

    private static bool LooksLikeDSpicoExported(string folderPath)
    {
        // Indicators of an exported DSpico theme (e.g., presence of .ptex files or a specific export log)
        return File.Exists(Path.Combine(folderPath, "topbg.bin")) && File.Exists(Path.Combine(folderPath, "bottombg.bin")) && File.Exists(Path.Combine(folderPath, "theme.json"));
    }

    private static bool LooksLikeDSpicoSource(string folderPath)
    {
        // Most common DSpico indicators
        return File.Exists(Path.Combine(folderPath, "topbg.png")) && File.Exists(Path.Combine(folderPath, "bottombg.png"));
    }

    private static bool LooksLikeAKMenu(string folderPath)
    {
        // Most common AKMenu indicators

        //if folder contains a file named "custom.ini" and within that file there's a line with "text = acekard"
        string customIniPath = Path.Combine(folderPath, "custom.ini");
        if (File.Exists(customIniPath))
        {
            var lines = File.ReadAllLines(customIniPath);
            if (lines.Any(line => line.Trim().Equals("text = acekard", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
        }
        return false;
    }

    private static bool LooksLikeYsMenu(string folderPath)
    {
        // Most common YSMenu indicators
        return File.Exists(Path.Combine(folderPath, "YSMenu.ini"));
    }

    private static bool LooksLikeMoonshell(string folderPath)
    {
        // Most common Moonshell indicators
        return File.Exists(Path.Combine(folderPath, "sndeff.dat"));
    }
}

