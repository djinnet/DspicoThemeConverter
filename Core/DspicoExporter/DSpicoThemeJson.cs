using System.Text.Json.Serialization;

namespace DspicoThemeForms.Core.DspicoExporter;

/// <summary>
/// Represents the configuration for a DSpico application theme, including properties for customization such as type,
/// name, description, author, primary color, and dark theme preference.
/// </summary>
/// <remarks>Use this class to define the visual appearance and metadata of a DSpico theme. The properties allow
/// for specifying theme details and customizing the application's look and feel. The 'Type' property defaults to
/// 'custom', and the 'PrimaryColor' property is initialized with a new instance of the PrimaryColor class. The
/// 'DarkTheme' property indicates whether the theme uses a dark color scheme.</remarks>
public sealed class DSpicoThemeJson
{
    [JsonPropertyName("type")]
    public string? Type { get; set; } = "custom";
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("author")]
    public string? Author { get; set; }

    [JsonPropertyName("primaryColor")]
    public PrimaryColor PrimaryColor { get; set; } = new();
    [JsonPropertyName("darkTheme")]
    public bool DarkTheme { get; set; }
}

/// <summary>
/// Represents a color defined by its red, green, and blue components.
/// </summary>
/// <remarks>Each component is represented as an integer value ranging from 0 to 255, where 0 indicates no
/// intensity and 255 indicates full intensity. This class is typically used in color representation for graphics and UI
/// elements.</remarks>
public sealed class PrimaryColor
{
    [JsonPropertyName("r")]
    public int R { get; set; }
    [JsonPropertyName("g")]
    public int G { get; set; }
    [JsonPropertyName("b")]
    public int B { get; set; }
}

