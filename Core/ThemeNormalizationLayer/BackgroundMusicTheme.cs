namespace DspicoThemeForms.Core.ThemeNormalizationLayer;

/// <summary>
/// Represents a background music theme, including metadata such as title and description, as well as information about
/// the associated music file.
/// </summary>
/// <remarks>The BackgroundMusicTheme class provides properties for specifying both the file path and the raw byte
/// data of a music file. The FilePath property can accept either relative or absolute paths, while the Data property
/// enables in-memory processing or direct writing to disk. This class is typically used to encapsulate all relevant
/// information for a background music theme in applications that support customizable audio experiences. This is currently not implemented yet.</remarks>
public class BackgroundMusicTheme
{
    // metadata for background music theme
    public required string Title { get; set; }
    public string? Description { get; set; }

    public string? FilePath { get; set; } // path to the music file, can be relative or absolute
    public byte[]? Data { get; set; } // raw byte data of the music file, can be used for in-memory processing or writing to disk
}
