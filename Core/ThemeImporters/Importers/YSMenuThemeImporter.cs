using DspicoThemeForms.Core.Enums;
using DspicoThemeForms.Core.Helper;
using DspicoThemeForms.Core.ThemeNormalizationLayer;

namespace DspicoThemeForms.Core.ThemeImporters.Importers;

/// <summary>
/// Provides functionality to import YSMenu themes by validating and converting theme assets into a normalized format.
/// </summary>
/// <remarks>This class implements the IThemeImporter interface to support importing themes from the YSMenu
/// format. It checks for the required bitmap files and ensures they meet expected resolution requirements before
/// creating a NormalizedTheme instance. If any required files are missing or invalid, the import process will fail
/// gracefully. Use this importer when you need to convert YSMenu theme folders into a standardized theme
/// representation.</remarks>
public class YSMenuThemeImporter : IThemeImporter
{
    public EThemeType Name => EThemeType.YSMenu;

    public bool CanImport(string Folderpath, EgatesFormat format = EgatesFormat.AND)
    {
        if (string.IsNullOrEmpty(Folderpath))
            return false;

        //check for the presence of expected YSMenu theme files
        string topPath = Path.Combine(Folderpath, "YSMenu1.bmp");
        string bottomPath = Path.Combine(Folderpath, "YSMenu2.bmp");

        return format.FileChecking([topPath, bottomPath]);
    }

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
