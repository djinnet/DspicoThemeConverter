using DspicoThemeForms.Core.Helper;
using DspicoThemeForms.Core.ThemeNormalizationLayer;

namespace DspicoThemeForms.Core.ThemeImporters.Importers;

public class AKMenuThemeImporter : IThemeImporter
{
    public string Name => "AKMenu";

    public NormalizedTheme? Import(string Folderpath)
    {
        try
        {
            if (!Directory.Exists(Folderpath))
                throw new DirectoryNotFoundException("The specified theme folder was not found.");

            // Expected AKMenu filenames
            var topPath = Path.Combine(Folderpath, "upper_screen.bmp");
            var bottomPath = Path.Combine(Folderpath, "lower_screen.bmp");

            if (!File.Exists(topPath))
                throw new FileNotFoundException("AKMenu top background not found", topPath);

            if (!File.Exists(bottomPath))
                throw new FileNotFoundException("AKMenu bottom background not found", bottomPath);

            var topBitmap = BitmapHelpers.LoadBitmap(topPath);
            var bottomBitmap = BitmapHelpers.LoadBitmap(bottomPath);

            string customIniPath = Path.Combine(Folderpath, "custom.ini");
            string author = "Unknown Author";
            if (File.Exists(customIniPath))
            {
                // Optional: Read custom.ini for additional metadata (e.g., Author, Description)
                var iniLines = File.ReadAllLines(customIniPath);
                // Simple parsing logic can be implemented here if needed
                author = iniLines.FirstOrDefault(line => line.StartsWith("text ="))?.Split('=')[1].Trim() ?? "Unknown";
            }

            BitmapHelpers.ValidateResolution(topBitmap, 256, 192, "Top background");
            BitmapHelpers.ValidateResolution(bottomBitmap, 256, 192, "Bottom background");

            return new NormalizedTheme
            {
                Name = Path.GetFileName(Folderpath),
                Description = "Converted from AKMenu theme",
                Author = author,
                OriginTheme = Name,
                DarkTheme = BitmapHelpers.IsDarkTheme(topBitmap),
                PrimaryColor = BitmapHelpers.ExtractPrimaryColor(topBitmap),

                TopBackground = topBitmap,
                BottomBackground = bottomBitmap
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error importing AKMenu theme: {e.Message}");
            return null;
        }
    }
}
