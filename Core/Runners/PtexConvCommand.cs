using EnumStringValues;

namespace DspicoThemeForms.Core.Runners;

public sealed class PtexConvCommand
{
    public string ptexconvExeName { get; set; } = "ptexconv.exe";
    public bool IsTexture { get; set; } = false; // -gt for textures, -gb for bitmaps
    public string InputImage { get; set; } = string.Empty;
    public string OutputBaseName { get; set; } = string.Empty;

    // Texture-only options
    public ETextureFormat TextureFormat { get; set; } = ETextureFormat.None; // a3i5, a5i3, palette16, etc.
    public int? PaletteLimit { get; set; } = null;    // -cm
    public int? DitherPercent { get; set; } = null;   // -d
    public bool DitherAlpha { get; set; } = false;     // -da

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(InputImage))
            throw new InvalidOperationException("InputImage not set");

        if (string.IsNullOrWhiteSpace(OutputBaseName))
            throw new InvalidOperationException("OutputBaseName not set");

        var args = new List<string>
        {
            ptexconvExeName,
            InputImage
        };

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
