using EnumStringValues;

namespace DspicoThemeForms.Core.Enums;

/// <summary>
/// Specifies the available texture formats for representing image data within the application.
/// </summary>
/// <remarks>Each format defines a distinct method of storing texture information, which can impact both
/// performance and visual fidelity. The enumeration includes options for palette-based, direct color, and compressed
/// formats, allowing developers to select the most appropriate format for their use case.</remarks>
public enum ETextureFormat : int
{
    None = 0,
    [StringValue("a3i5")]
    A3I5 = 1,
    [StringValue("a5i3")]
    A5I3 = 2,
    [StringValue("palette16")]
    Palette16 = 3,
    [StringValue("palette256")]
    Palette256 = 4,
    [StringValue("rgba8888")]
    RGBA8888 = 5,
    [StringValue("rgb565")]
    RGB565 = 6,
    [StringValue("direct")]
    Direct = 7
}
