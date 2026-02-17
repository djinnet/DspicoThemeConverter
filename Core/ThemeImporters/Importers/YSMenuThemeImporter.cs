using DspicoThemeForms.Core.Helper;
using DspicoThemeForms.Core.ThemeNormalizationLayer;

namespace DspicoThemeForms.Core.ThemeImporters.Importers;

public class YSMenuThemeImporter : IThemeImporter
{
    public string Name => "YSMenu";
    public NormalizedTheme? Import(string Folderpath)
    {
        try
        {
            if (!Directory.Exists(Folderpath))
                throw new DirectoryNotFoundException("The specified theme folder was not found.");

            // Expected YSMenu filenames
            var topPath = Path.Combine(Folderpath, "YSMenu1.bmp");
            var bottomPath = Path.Combine(Folderpath, "YSMenu2.bmp");

            if (!File.Exists(topPath))
                throw new FileNotFoundException("YSMenu top background not found", topPath);

            if (!File.Exists(bottomPath))
                throw new FileNotFoundException("YSMenu bottom background not found", bottomPath);

            var topBitmap = BitmapHelpers.LoadBitmap(topPath);
            var bottomBitmap = BitmapHelpers.LoadBitmap(bottomPath);

            BitmapHelpers.ValidateResolution(topBitmap, 256, 192, "Top background");
            BitmapHelpers.ValidateResolution(bottomBitmap, 256, 192, "Bottom background");

            return new NormalizedTheme
            {
                Name = Path.GetFileName(Folderpath),
                Description = "Converted from YSMenu theme",
                OriginTheme = Name,
                DarkTheme = BitmapHelpers.IsDarkTheme(topBitmap),
                PrimaryColor = BitmapHelpers.ExtractPrimaryColor(topBitmap),
                TopBackground = topBitmap,
                BottomBackground = bottomBitmap
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error importing YSMenu theme: {e.Message}");
            return null;
        }
    }


}
