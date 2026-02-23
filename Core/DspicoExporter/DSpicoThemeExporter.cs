using DspicoThemeForms.Core.Constants;
using DspicoThemeForms.Core.Enums;
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

            if (string.IsNullOrEmpty(_outputPath))
            {
                _log("Error: Output path is null or empty.");
                return;
            }

            string themeFolderPath = CreateThemeFolderAtDest(theme);

            if (string.IsNullOrEmpty(themeFolderPath))
            {
                _log("Error: Failed to create theme folder at destination.");
                return;
            }

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
            bool result = RemovedTempFile(["topbg", "bottombg", "bannerListCell", "bannerListCellSelected", "gridCell", "gridCellSelected", "scrim"]);
            if (!result)
            {
                _log("Failed to remove temporary files: topbg.png, bottombg.png");
            }

            bool moved = MoveTexFiles(theme);
            if (!moved)
            {
                _log("Failed to move tex files");
            }

            bool movedPal = MovePalFiles(theme);
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
                string topBgPath = SaveTempPng(theme.TopBackground, "topbg");
                PtexConvCommand TopBGcommand = new()
                {
                    InputImage = "topbg.png",
                    OutputBaseName = "topbg",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.Direct,
                };
                _log("Running ptexconv on top background...");
                bool topresult = _ptexConvRunner.Run(TopBGcommand.ToString());
                result.HasRunTopBackground = topresult;
            }

            if (theme.BottomBackground != null)
            {
                _log("Saving bottom background...");
                string bottomBgPath = SaveTempPng(theme.BottomBackground, "bottombg");
                PtexConvCommand BottomBGcommand = new()
                {
                    InputImage = "bottombg.png",
                    OutputBaseName = "bottombg",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.Direct,
                };
                _log("Running ptexconv on bottom background...");
                bool bottomresult = _ptexConvRunner.Run(BottomBGcommand.ToString());
                result.HasRunBottomBackground = bottomresult;
            }

            if (theme.BannerListCell != null)
            {
                _log("Saving banner list cell...");
                string bannerListCellPath = SaveTempPng(theme.BannerListCell, "bannerListCell");
                PtexConvCommand BannerListCellCommand = new()
                {
                    InputImage = "bannerListCell.png",
                    OutputBaseName = "bannerListCell",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.A3I5,
                };
                _log("Running ptexconv on banner list cell...");
                bool bannerListCellResult = _ptexConvRunner.Run(BannerListCellCommand.ToString());
                result.HasRunBannerListCell = bannerListCellResult;
            }

            if (theme.BannerListCellSelected != null)
            {
                _log("Saving banner list cell selected...");
                string bannerListCellSelectedPath = SaveTempPng(theme.BannerListCellSelected, "bannerListCellSelected");
                PtexConvCommand BannerListCellSelectedCommand = new()
                {
                    InputImage = "bannerListCellSelected.png",
                    OutputBaseName = "bannerListCellSelected",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.A3I5,
                };
                _log("Running ptexconv on banner list cell selected...");
                bool bannerListCellSelectedResult = _ptexConvRunner.Run(BannerListCellSelectedCommand.ToString());
                result.HasRunBannerListCellSelected = bannerListCellSelectedResult;
            }


            if (theme.GridCell != null)
            {
                _log("Saving grid cell...");
                string gridCellPath = SaveTempPng(theme.GridCell, "gridCell");
                PtexConvCommand GridCellCommand = new()
                {
                    InputImage = "gridCell.png",
                    OutputBaseName = "gridCell",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.A3I5,
                };
                _log("Running ptexconv on grid cell...");
                bool gridCellResult = _ptexConvRunner.Run(GridCellCommand.ToString());
                result.HasRunGridCell = gridCellResult;
            }

            if (theme.GridCellSelected != null)
            {
                _log("Saving grid cell selected...");
                string gridCellSelectedPath = SaveTempPng(theme.GridCellSelected, "gridCellSelected");
                PtexConvCommand GridCellSelectedCommand = new()
                {
                    InputImage = "gridCellSelected.png",
                    OutputBaseName = "gridCellSelected",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.A3I5,
                };
                _log("Running ptexconv on grid cell selected...");
                bool gridCellSelectedResult = _ptexConvRunner.Run(GridCellSelectedCommand.ToString());
                result.HasRunGridCellSelected = gridCellSelectedResult;
            }

            if (theme.Scrim != null)
            {
                _log("Saving scrim...");
                string scrimPath = SaveTempPng(theme.Scrim, "scrim");
                PtexConvCommand ScrimCommand = new()
                {
                    InputImage = "scrim.png",
                    OutputBaseName = "scrim",
                    IsTexture = true,
                    TextureFormat = ETextureFormat.A5I3,
                };
                _log("Running ptexconv on scrim...");
                bool scrimResult = _ptexConvRunner.Run(ScrimCommand.ToString());
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
        try
        {
            _log("Creating theme folder...");
            if (theme.OriginTheme == null)
            {
                _log("Warning: Origin theme is null or empty. Using 'None' as origin theme.");
                theme.OriginTheme = Enums.EThemeType.None;
            }

            if (string.IsNullOrEmpty(theme.Name))
            {
                _log("Warning: Theme name is null or empty. Using 'Unnamed' as theme name.");
                theme.Name = "Unnamed";
            }
            string themeFolderPath = Path.Combine(_outputPath, theme.OriginTheme + "_" + theme.Name);
            if (!Directory.Exists(themeFolderPath))
            {
                _log($"Theme folder does not exist. Creating new folder at: {themeFolderPath}");
            }
            else
            {
                _log($"Theme folder already exists at: {themeFolderPath}. It will be overwritten.");
            }
            Directory.CreateDirectory(themeFolderPath);
            return themeFolderPath;
        }
        catch (Exception ex)
        {
            _log($"Error creating theme folder at destination: {ex.Message}");
            return string.Empty;
        }
    }

    private void CreateThemeJson(NormalizedTheme theme, string themeFolderPath)
    {
        try
        {
            _log("Writing theme.json...");
            string themeJsonPath = Path.Combine(themeFolderPath, FilesContants.ThemeJsonFileName);

            DSpicoThemeJson json = new()
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
            };


            string themeJsonContent = System.Text.Json.JsonSerializer.Serialize(json, _options);
            File.WriteAllText(themeJsonPath, themeJsonContent);
        }
        catch (Exception ex)
        {
            _log($"Error creating theme.json: {ex.Message}");
        }
    }

    private string SaveTempPng(Bitmap bmp, string name)
    {
        if (bmp == null)
        {
            _log($"Error: Bitmap for {name} is null.");
            return string.Empty;
        }

        if (string.IsNullOrEmpty(name))
        {
            _log("Error: Name for temporary PNG file is null or empty.");
            return string.Empty;
        }

        string toolsDirectory = PathHelper.GetToolsDirectory();
        if (string.IsNullOrEmpty(toolsDirectory))
        {
            _log("Error: Tools directory path is null or empty.");
            return string.Empty;
        }

        string path = Path.Combine(toolsDirectory, $"{name}.png");

        if (File.Exists(path))
        {
            _log($"Warning: Temporary file {name}.png already exists and will be overwritten.");
        }

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

        string toolsDirectory = PathHelper.GetToolsDirectory();
        if (string.IsNullOrEmpty(toolsDirectory))
        {
            _log("Error: Tools directory path is null or empty.");
            return false;
        }

        foreach (var name in names)
        {
            string path = Path.Combine(toolsDirectory, $"{name}.png");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        return true;
    }

    private bool MovePalFiles(NormalizedTheme theme)
    {
        string toolsDirectory = PathHelper.GetToolsDirectory();
        if (string.IsNullOrEmpty(toolsDirectory))
        {
            _log("Error: Tools directory path is null or empty.");
            return false;
        }

        string[] findAllPalFiles = Directory.GetFiles(toolsDirectory, FilesContants.Wildcard_PalBinFiles);
        if (findAllPalFiles.Length == 0)
        {
            _log("No palette files found to move.");
            return false;
        }
        bool success = true;
        foreach (string file in findAllPalFiles)
        {
            string fileName = Path.GetFileName(file);
            //removed the _pal suffix from the file name to get the original name
            //e.g. topbg_pal.bin -> topbg.bin
            string originalFileName = fileName.Replace(FilesContants.PalBinFileSuffix, FilesContants.PlttBinFileSuffix);
            bool moved = MoveOutputFile(fileName, originalFileName, theme);
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
        string toolsDirectory = PathHelper.GetToolsDirectory();
        if (string.IsNullOrEmpty(toolsDirectory))
        {
            _log("Error: Tools directory path is null or empty.");
            return false;
        }
        string[] findAllTexFiles = Directory.GetFiles(toolsDirectory, FilesContants.Wildcard_TexBinFiles);
        if (findAllTexFiles.Length == 0)
        {
            _log("No texture files found to move.");
            return false;
        }
        bool success = true;
        foreach (string file in findAllTexFiles)
        {
            string fileName = Path.GetFileName(file);
            //removed the _tex suffix from the file name to get the original name
            //e.g. topbg_tex.bin -> topbg.bin
            string originalFileName = fileName.Replace(FilesContants.TexBinFileSuffix, FilesContants.BinFiles);
            bool moved = MoveOutputFile(fileName, originalFileName, theme);
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
        string toolsDirectory = PathHelper.GetToolsDirectory();
        if (string.IsNullOrEmpty(toolsDirectory))
        {
            _log("Error: Tools directory path is null or empty.");
            return false;
        }
        string sourcePath = Path.Combine(toolsDirectory, sourceFileName);
        string destPath = Path.Combine(_outputPath, destFileName);
        if (!File.Exists(sourcePath))
        {
            _log($"Error: Source file {sourceFileName} not found.");
            return false;
        }

        if (theme.OriginTheme == null)
        {
            _log("Warning: Origin theme is null or empty. Using 'None' as origin theme.");
            theme.OriginTheme = Enums.EThemeType.None;
        }

        string themeFolderPath = Path.Combine(_outputPath, theme.OriginTheme + "_" + theme.Name);

        string finalDestPath = Path.Combine(themeFolderPath, destFileName);
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

