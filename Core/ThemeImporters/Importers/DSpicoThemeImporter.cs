using DspicoThemeForms.Core.Enums;
using DspicoThemeForms.Core.Helper;
using DspicoThemeForms.Core.ThemeNormalizationLayer;

namespace DspicoThemeForms.Core.ThemeImporters.Importers;

public class DSpicoThemeImporter : IThemeImporter
{
    public EThemeType Name => EThemeType.DSpico;
    public NormalizedTheme? Import(string Folderpath)
    {
        try
        {
            if (!Directory.Exists(Folderpath))
                throw new DirectoryNotFoundException("The specified theme folder was not found.");

            // Expected DSpico filenames
            string topPath = Path.Combine(Folderpath, "topbg.png");
            string bottomPath = Path.Combine(Folderpath, "bottombg.png");
            string gridCellPath = Path.Combine(Folderpath, "gridcell.png");
            string bannerListCellPath = Path.Combine(Folderpath, "bannerListCell.png");
            string bannerListCellSelectedPath = Path.Combine(Folderpath, "bannerListCellSelected.png");
            string scrimPath = Path.Combine(Folderpath, "scrim.png");
            string gridCellSelectedPath = Path.Combine(Folderpath, "gridcellSelected.png");


            Bitmap? topBitmap = null;
            Bitmap? bottomBitmap = null;
            Bitmap? gridCellBitmap = null;
            Bitmap? bannerListCellBitmap = null;
            Bitmap? bannerListCellSelectedBitmap = null;
            Bitmap? scrimBitmap = null;
            Bitmap? gridCellSelectedBitmap = null;

            if (File.Exists(topPath))
            {
                topBitmap = BitmapHelpers.LoadBitmap(topPath);
                BitmapHelpers.ValidateResolution(topBitmap, 256, 192, "Top background");
            }

            if (File.Exists(bottomPath))
            {
                bottomBitmap = BitmapHelpers.LoadBitmap(bottomPath);
                BitmapHelpers.ValidateResolution(bottomBitmap, 256, 192, "Bottom background");
            }

            if (File.Exists(gridCellPath))
            {
                gridCellBitmap = BitmapHelpers.LoadBitmap(gridCellPath);
            }

            if (File.Exists(bannerListCellPath))
            {
                bannerListCellBitmap = BitmapHelpers.LoadBitmap(bannerListCellPath);
            }

            if (File.Exists(scrimPath))
            {
                scrimBitmap = BitmapHelpers.LoadBitmap(scrimPath);
            }

            if (File.Exists(gridCellSelectedPath))
            {
                gridCellSelectedBitmap = BitmapHelpers.LoadBitmap(gridCellSelectedPath);
            }

            if (File.Exists(bannerListCellSelectedPath))
            {
                bannerListCellSelectedBitmap = BitmapHelpers.LoadBitmap(bannerListCellSelectedPath);
            }

            var parsedThemeFromMetadata = MetadataFinderHelper.MetadataFinder.Parse(Folderpath);

            return new NormalizedTheme
            {
                Name = parsedThemeFromMetadata.Name,
                Description = parsedThemeFromMetadata.Description,
                Author = parsedThemeFromMetadata.Author,
                OriginTheme = Name,
                DarkTheme = parsedThemeFromMetadata.DarkTheme,
                PrimaryColor = parsedThemeFromMetadata.PrimaryColor,

                TopBackground = topBitmap,
                BottomBackground = bottomBitmap,

                GridCell = gridCellBitmap,

                GridCellSelected = gridCellSelectedBitmap,

                BannerListCell = bannerListCellBitmap,

                BannerListCellSelected = bannerListCellSelectedBitmap,

                Scrim = scrimBitmap,
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing DSpico theme: {ex.Message}");
            return null;
        }
    }
}
