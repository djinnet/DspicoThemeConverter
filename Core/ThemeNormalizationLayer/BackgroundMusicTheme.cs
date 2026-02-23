namespace DspicoThemeForms.Core.ThemeNormalizationLayer
{
    public class BackgroundMusicTheme
    {
        // metadata for background music theme
        public required string Title { get; set; }
        public string? Description { get; set; }

        public string? FilePath { get; set; } // path to the music file, can be relative or absolute
        public byte[]? Data { get; set; } // raw byte data of the music file, can be used for in-memory processing or writing to disk
    }
}
