using System.Text.Json.Serialization;

namespace DspicoThemeForms.Core.DspicoExporter;

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

public sealed class PrimaryColor
{
    [JsonPropertyName("r")]
    public int R { get; set; }
    [JsonPropertyName("g")]
    public int G { get; set; }
    [JsonPropertyName("b")]
    public int B { get; set; }
}

