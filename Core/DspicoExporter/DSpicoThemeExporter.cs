using DspicoThemeForms.Core.Helper;
using DspicoThemeForms.Core.Runners;
using DspicoThemeForms.Core.ThemeNormalizationLayer;
using System.Drawing.Imaging;

namespace DspicoThemeForms.Core.DspicoExporter;
public sealed class DSpicoThemeExporter
{
    private readonly PtexConvRunner _ptexConvRunner;
    private readonly string _outputPath;
    private readonly Action<string> _log;

    private readonly System.Text.Json.JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    public DSpicoThemeExporter(string outputPath, string ptexConvPath, Action<string> log)
    {
        _outputPath = outputPath;
        _log = log;
        _ptexConvRunner = new PtexConvRunner(ptexConvPath, log);
    }

    public void Export(NormalizedTheme theme)
    {
        try
        {
            if (theme == null)
            {
                _log("Error: Theme is null.");
                return;
            }

            string themeFolderPath = CreateThemeFolderAtDest(theme);

            CreateThemeJson(theme, themeFolderPath);

            (bool flowControl, PtexConvResult? value) = Run_Ptexconv(theme);

            if (!flowControl)
            {
                return;
            }

            if (value == null)
            {
                _log("Error: PtexConvResult is null.");
                return;
            }

            ReportPtexConvResult(value);
        }
        catch (Exception ex)
        {
            _log($"An error occurred during export: {ex.Message}");
        }
        finally
        {
            var result = RemovedTempFile(["topbg", "bottombg"]);
            if (!result)
            {
                _log("Failed to remove temporary files: topbg.png, bottombg.png");
            }

            var moved = MoveTexFiles(theme);
            if (!moved)
            {
                _log("Failed to move tex files");
            }

            var movedPal = MovePalFiles(theme);
            if (!movedPal)
            {
                _log("Failed to move pal files");
            }
            _log("Export complete.");
        }
    }

    private void ReportPtexConvResult(PtexConvResult value)
    {
        if (value.NoneRan)
        {
            _log("No images were processed with ptexconv. Please check if the theme contains any images.");
        }
        else if (value.AllRan)
        {
            _log("All images were processed with ptexconv successfully.");
        }
        else if (value.SomeRan)
        {

            _log("Some images were processed with ptexconv. Please check the log for details.");
        }
        else
        {
            _log("Unexpected result from ptexconv processing. Please check the log for details.");
        }
    }

    private (bool flowControl, PtexConvResult? value) Run_Ptexconv(NormalizedTheme theme)
    {
        try
        {
            PtexConvResult result = new();

            if (theme.TopBackground != null)
            {
                _log("Saving top background...");
                var topBgPath = SaveTempPng(theme.TopBackground, "topbg");
                PtexConvCommand TopBGcommand = new()
                {
                    InputImage = "topbg.png",
                    OutputBaseName = "topbg",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.Direct,
                };
                _log("Running ptexconv on top background...");
                var topresult = _ptexConvRunner.Run(TopBGcommand.ToString());
                result.HasRunTopBackground = topresult;
            }

            if (theme.BottomBackground != null)
            {
                _log("Saving bottom background...");
                var bottomBgPath = SaveTempPng(theme.BottomBackground, "bottombg");
                PtexConvCommand BottomBGcommand = new()
                {
                    InputImage = "bottombg.png",
                    OutputBaseName = "bottombg",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.Direct,
                };
                _log("Running ptexconv on bottom background...");
                var bottomresult = _ptexConvRunner.Run(BottomBGcommand.ToString());
                result.HasRunBottomBackground = bottomresult;
            }

            if (theme.BannerListCell != null)
            {
                _log("Saving banner list cell...");
                var bannerListCellPath = SaveTempPng(theme.BannerListCell, "bannerListCell");
                PtexConvCommand BannerListCellCommand = new()
                {
                    InputImage = "bannerListCell.png",
                    OutputBaseName = "bannerListCell",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.A3I5,
                };
                _log("Running ptexconv on banner list cell...");
                var bannerListCellResult = _ptexConvRunner.Run(BannerListCellCommand.ToString());
                result.HasRunBannerListCell = bannerListCellResult;
            }

            if (theme.BannerListCellSelected != null)
            {
                _log("Saving banner list cell selected...");
                var bannerListCellSelectedPath = SaveTempPng(theme.BannerListCellSelected, "bannerListCellSelected");
                PtexConvCommand BannerListCellSelectedCommand = new()
                {
                    InputImage = "bannerListCellSelected.png",
                    OutputBaseName = "bannerListCellSelected",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.A3I5,
                };
                _log("Running ptexconv on banner list cell selected...");
                var bannerListCellSelectedResult = _ptexConvRunner.Run(BannerListCellSelectedCommand.ToString());
                result.HasRunBannerListCellSelected = bannerListCellSelectedResult;
            }


            if (theme.GridCell != null)
            {
                _log("Saving grid cell...");
                var gridCellPath = SaveTempPng(theme.GridCell, "gridCell");
                PtexConvCommand GridCellCommand = new()
                {
                    InputImage = "gridCell.png",
                    OutputBaseName = "gridCell",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.A3I5,
                };
                _log("Running ptexconv on grid cell...");
                var gridCellResult = _ptexConvRunner.Run(GridCellCommand.ToString());
                result.HasRunGridCell = gridCellResult;
            }

            if (theme.GridCellSelected != null)
            {
                _log("Saving grid cell selected...");
                var gridCellSelectedPath = SaveTempPng(theme.GridCellSelected, "gridCellSelected");
                PtexConvCommand GridCellSelectedCommand = new()
                {
                    InputImage = "gridCellSelected.png",
                    OutputBaseName = "gridCellSelected",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.A3I5,
                };
                _log("Running ptexconv on grid cell selected...");
                var gridCellSelectedResult = _ptexConvRunner.Run(GridCellSelectedCommand.ToString());
                result.HasRunGridCellSelected = gridCellSelectedResult;
            }

            if (theme.Scrim != null)
            {
                _log("Saving scrim...");
                var scrimPath = SaveTempPng(theme.Scrim, "scrim");
                PtexConvCommand ScrimCommand = new()
                {
                    InputImage = "scrim.png",
                    OutputBaseName = "scrim",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.A5I3,
                };
                _log("Running ptexconv on scrim...");
                var scrimResult = _ptexConvRunner.Run(ScrimCommand.ToString());
                result.HasRunScrim = scrimResult;
            }



            return (flowControl: true, value: result);
        }
        catch (Exception ex)
        {
            _log($"An error occurred while running ptexconv: {ex.Message}");
            return (flowControl: false, value: null);
        }
    }

    private string CreateThemeFolderAtDest(NormalizedTheme theme)
    {
        _log("Creating theme folder...");
        if (string.IsNullOrEmpty(theme.OriginTheme))
        {
            _log("Warning: Origin theme is null or empty. Using 'Unknown' as origin theme.");
            theme.OriginTheme = "Unknown";
        }

        if (string.IsNullOrEmpty(theme.Name))
        {
            _log("Warning: Theme name is null or empty. Using 'Unnamed' as theme name.");
            theme.Name = "Unnamed";
        }
        var themeFolderPath = Path.Combine(_outputPath, theme.OriginTheme + "_" + theme.Name);
        Directory.CreateDirectory(themeFolderPath);
        return themeFolderPath;
    }

    private void CreateThemeJson(NormalizedTheme theme, string themeFolderPath)
    {
        _log("Writing theme.json...");
        var themeJsonPath = Path.Combine(themeFolderPath, "theme.json");

        var themeJsonContent = System.Text.Json.JsonSerializer.Serialize(new DSpicoThemeJson
        {
            Name = theme.Name,
            Description = theme.Description,
            Author = theme.Author,
            PrimaryColor = new PrimaryColor
            {
                R = theme.PrimaryColor.R,
                G = theme.PrimaryColor.G,
                B = theme.PrimaryColor.B
            },
            DarkTheme = theme.DarkTheme,
            Type = "Custom"
        }, _options);
        File.WriteAllText(themeJsonPath, themeJsonContent);
    }

    private string SaveTempPng(Bitmap bmp, string name)
    {
        if (bmp == null)
        {
            _log($"Error: Bitmap for {name} is null.");
            return string.Empty;
        }

        if (File.Exists(Path.Combine(PathHelper.GetToolsDirectory(), $"{name}.png")))
        {
            _log($"Warning: Temporary file {name}.png already exists and will be overwritten.");
        }
        var path = Path.Combine(PathHelper.GetToolsDirectory(), $"{name}.png");
        bmp.Save(path, ImageFormat.Png);
        return path;
    }

    private bool RemovedTempFile(string[] names)
    {
        if (names == null || names.Length == 0)
        {
            _log("No temporary file names provided for deletion.");
            return false;
        }

        foreach (var name in names)
        {
            var path = Path.Combine(PathHelper.GetToolsDirectory(), $"{name}.png");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        return true;
    }

    private bool MovePalFiles(NormalizedTheme theme)
    {
        var findAllPalFiles = Directory.GetFiles(PathHelper.GetToolsDirectory(), "*_pal.bin");
        if (findAllPalFiles.Length == 0)
        {
            return false;
        }
        var success = true;
        foreach (var file in findAllPalFiles)
        {
            var fileName = Path.GetFileName(file);
            //removed the _pal suffix from the file name to get the original name
            //e.g. topbg_pal.bin -> topbg.bin
            var originalFileName = fileName.Replace("_pal.bin", "Pltt.bin");
            var moved = MoveOutputFile(fileName, originalFileName, theme);
            if (!moved)
            {
                _log($"Failed to move output file: {fileName}");
                success = false;
            }
        }
        return success;
    }


    private bool MoveTexFiles(NormalizedTheme theme)
    {
        var findAllTexFiles = Directory.GetFiles(PathHelper.GetToolsDirectory(), "*_tex.bin");
        if (findAllTexFiles.Length == 0)
        {
            return false;
        }
        var success = true;
        foreach (var file in findAllTexFiles)
        {
            var fileName = Path.GetFileName(file);
            //removed the _tex suffix from the file name to get the original name
            //e.g. topbg_tex.bin -> topbg.bin
            var originalFileName = fileName.Replace("_tex.bin", ".bin");
            var moved = MoveOutputFile(fileName, originalFileName, theme);
            if (!moved)
            {
                _log($"Failed to move output file: {fileName}");
                success = false;
            }
        }
        return success;
    }

    private bool MoveOutputFile(string sourceFileName, string destFileName, NormalizedTheme theme)
    {
        var sourcePath = Path.Combine(PathHelper.GetToolsDirectory(), sourceFileName);
        var destPath = Path.Combine(_outputPath, destFileName);
        if (!File.Exists(sourcePath))
        {
            _log($"Error: Source file {sourceFileName} not found.");
            return false;
        }
        var themeFolderPath = Path.Combine(_outputPath, theme.OriginTheme + "_" + theme.Name);

        var finalDestPath = Path.Combine(themeFolderPath, destFileName);
        try
        {
            _log($"Moving {sourcePath} to output: {finalDestPath}");
            File.Move(sourcePath, finalDestPath);
            return true;
        }
        catch (Exception ex)
        {
            _log($"Error moving file {sourceFileName} to output: {ex.Message}");
            return false;
        }
    }
}

