using DspicoThemeForms.Core.Enums;

namespace DspicoThemeForms.Core.ThemeNormalizationLayer;

public sealed class NormalizedTheme
{
    public required string Name { get; set; }
    public string? Description { get; set; } = string.Empty;
    public string? Author { get; set; } = "Unknown Author";
    public EThemeType? OriginTheme { get; set; } = EThemeType.None; // Optional: which theme this was normalized from, if any

    public string? ThemeVersion { get; set; } = null; // Optional: version of the theme, if specified in metadata

    public Color PrimaryColor { get; set; } = Color.White; // Default to white if not specified
    public bool DarkTheme { get; set; } = false;

    public bool AllowedOverwriteData { get; set; } = false; // If true, this theme can overwrite name, description, author, and primary color when converting to a DSpico theme. Used for themes that have metadata files with this information.

    // Bitmaps in normal PC formats
    public Bitmap? TopBackground { get; set; } = null;// 256x192 15 bpp topbg.bin
    public Bitmap? BottomBackground { get; set; } = null; // 256x192 15 bpp bottombg.bin

    public Bitmap? GridCell { get; set; } = null; // 64x48 (48x48 used) A3I5 32 color palette gridcell.bin
    public Bitmap? GridCellPltt { get; set; } = null; // 64x48 (48x48 used) A3I5 32 color palette gridcell_pltt.bin

    public Bitmap? BannerListCell { get; set; } = null; // 256x49 (209x49 used) A3I5 32 color palette bannerlistcell.bin
    public Bitmap? BannerListCellPltt { get; set; } = null; // 256x49 (209x49 used) A3I5 32 color palette bannerlistcell_pltt.bin

    public Bitmap? BannerListCellSelected { get; set; } = null; // 256x49 (209x49 used) A3I5 32 color palette bannerListCellSelected.bin
    public Bitmap? BannerListCellSelectedPltt { get; set; } = null; // 256x49 (209x49 used) A3I5 32 color palette bannerListCellSelectedPltt.bin

    public Bitmap? GridCellSelected { get; set; } = null; // 64x48 (48x48 used) A3I5 32 color palette gridcell_selected.bin
    public Bitmap? GridCellPlttSelected { get; set; } = null; // 64x48 (48x48 used) A3I5 32 color palette gridcell_selected_pltt.bin

    public Bitmap? Scrim { get; set; } = null; // 8x42 A5I3 8 color palette scrim.bin
    public Bitmap? ScrimPltt { get; set; } = null; // 8x42 A5I3 8 color palette scrim_pltt.bin

    public List<BackgroundMusicTheme> BackgroundMusicThemes { get; set; } = []; // List of background music themes, can be empty if no music themes are included
}

