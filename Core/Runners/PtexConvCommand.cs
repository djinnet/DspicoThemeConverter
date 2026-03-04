using DspicoThemeForms.Core.Constants;
using DspicoThemeForms.Core.Enums;
using EnumStringValues;

namespace DspicoThemeForms.Core.Runners;

/// <summary>
/// Represents a configuration for constructing command-line arguments to invoke the PtexConv tool for image conversion
/// and texture processing.
/// </summary>
/// <remarks>Use this class to specify input and output file names, texture format, palette limits, and dithering
/// options when preparing a command for the PtexConv tool. The properties should be set according to the desired
/// conversion scenario. For texture images, ensure that the TextureFormat property is specified. This class is not
/// thread-safe.</remarks>
public sealed class PtexConvCommand
{
    public bool IsTexture { get; set; } = false; // -gt for textures, -gb for bitmaps
    public string InputImage { get; set; } = string.Empty;
    public string OutputBaseName { get; set; } = string.Empty;

    // Texture-only options
    public ETextureFormat TextureFormat { get; set; } = ETextureFormat.None; // a3i5, a5i3, palette16, etc.
    public int? PaletteLimit { get; set; } = null;    // -cm
    public int? DitherPercent { get; set; } = null;   // -d
    public bool DitherAlpha { get; set; } = false;     // -da

    /// <summary>
    /// Generates a command-line string for invoking the PtexConv tool based on the current object's properties.
    /// </summary>
    /// <remarks>This method constructs a command string that includes various options based on the object's
    /// state, such as texture format and dithering settings. It is essential to ensure that all required properties are
    /// set before calling this method.</remarks>
    /// <returns>A string representing the command-line arguments for the PtexConv tool, constructed from the object's
    /// properties.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the InputImage or OutputBaseName properties are not set, or if TextureFormat is required but not
    /// provided for texture images.</exception>
    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(InputImage))
        {
            throw new InvalidOperationException("InputImage not set");
        }

        if (string.IsNullOrWhiteSpace(OutputBaseName))
        {
            throw new InvalidOperationException("OutputBaseName not set");
        }

        List<string> args =
        [
            FilesContants.PtexConvExeName,
            InputImage
        ];

        if (IsTexture)
        {
            args.Add("-gt");

            if (TextureFormat == ETextureFormat.None)
            {
                throw new InvalidOperationException("TextureFormat required for textures");
            }

            args.Add("-f");
            args.Add(TextureFormat.GetStringValue());
        }
        else
        {
            args.Add("-gb");
            args.Add("-bB"); // bitmap BG
        }

        if (PaletteLimit.HasValue)
        {
            args.Add("-cm");
            args.Add(PaletteLimit.Value.ToString());
        }

        if (DitherPercent.HasValue && DitherPercent.Value > 0)
        {
            args.Add("-d");
            args.Add(DitherPercent.Value.ToString());
        }

        if (DitherAlpha)
        {
            args.Add("-da");
        }


        args.Add("-o");
        args.Add(OutputBaseName);

        return string.Join(" ", args);
    }
}
