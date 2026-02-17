namespace DspicoThemeForms.Core.DspicoExporter;

public sealed class DSpicoThemeJson
{
    public string? Type { get; set; } = "custom";
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }

    public PrimaryColor PrimaryColor { get; set; } = new();
    public bool DarkTheme { get; set; }
}

public sealed class PrimaryColor
{
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
}

