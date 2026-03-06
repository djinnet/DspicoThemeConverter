namespace DspicoThemeForms.Core.Constants;

/// <summary>
/// Provides a collection of constant values representing file names and patterns used in the application.
/// </summary>
/// <remarks>These constants are used for file operations related to various file types, including executable
/// files and binary files. They help ensure consistency and avoid hardcoding file names throughout the
/// codebase.</remarks>
public class FilesContants
{
    public const string PtexConvExeName = "ptexconv.exe";
    public const string CmdExeName = "cmd.exe";
    public const string Wildcard_PalBinFiles = "*_pal.bin";
    public const string PalBinFileSuffix = "_pal.bin";
    public const string PlttBinFileSuffix = "Pltt.bin";
    public const string Wildcard_TexBinFiles = "*_tex.bin";
    public const string TexBinFileSuffix = "_tex.bin";
    public const string BinFiles = ".bin";
    public const string ThemeJsonFileName = "theme.json";
    public const string MetadataFileName = "metadata.ini";
    public const string ToolsDirectoryName = "Tools";
}
