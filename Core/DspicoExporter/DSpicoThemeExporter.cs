using DspicoThemeForms.Core.Constants;
using DspicoThemeForms.Core.Enums;
using DspicoThemeForms.Core.Helper;
using DspicoThemeForms.Core.Runners;
using DspicoThemeForms.Core.ThemeNormalizationLayer;
using Serilog;

namespace DspicoThemeForms.Core.DspicoExporter;

/// <summary>
/// Provides functionality for exporting themes, including processing associated images and generating required output
/// files.
/// </summary>
/// <remarks>The DSpicoThemeExporter class validates input parameters, manages the export workflow, and logs the
/// results of the operation. It processes theme images using the ptexconv tool and handles cleanup of temporary files.
/// This class is intended for scenarios where themes need to be exported in a format compatible with downstream systems
/// or applications.</remarks>
public sealed class DSpicoThemeExporter
{
    private readonly PtexConvRunner _ptexConvRunner;
    private readonly string _outputPath;

    private readonly System.Text.Json.JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// Initializes a new instance of the DSpicoThemeExporter class using the specified output directory, Ptex
    /// conversion executable path, and logging delegate.
    /// </summary>
    /// <remarks>Ensure that the provided paths are valid and that the application has sufficient permissions
    /// to read and write files in the specified locations.</remarks>
    /// <param name="outputPath">The file system path where exported files will be saved. Must be a valid, accessible directory.</param>
    /// <param name="ptexConvPath">The file system path to the Ptex conversion executable. Must reference a valid executable file.</param>
    /// <param name="log">A delegate that receives log messages as strings. Used to report progress and errors during export operations.</param>
    public DSpicoThemeExporter(string outputPath, string ptexConvPath)
    {
        _outputPath = outputPath;
        _ptexConvRunner = new PtexConvRunner(ptexConvPath);
    }


    /// <summary>
    /// Exports the specified theme to the designated output path, processing images and generating necessary files.
    /// </summary>
    /// <remarks>The method performs several checks before proceeding with the export, including validation of
    /// the theme and output path. It also handles cleanup of temporary files and logs the results of the export
    /// process.</remarks>
    /// <param name="theme">The theme to be exported. Must not be null.</param>
    /// <param name="overwrittenallow">A boolean indicating whether existing files can be overwritten during the export process.</param>
    /// <returns>true if the export operation is successful; otherwise, false.</returns>
    public bool Export(NormalizedTheme theme, bool overwrittenallow)
    {
        var _log = Serilog.Log.Logger;
        try
        {

            if (theme == null)
            {
                _log.Warning("Error: Theme is null.");
                return false;
            }

            if (string.IsNullOrEmpty(_outputPath))
            {
                _log.Warning("Error: Output path is null or empty.");
                return false;
            }

            // TODO: check if the tool folder doesn't contain any leftover files from previous conversions and clean them up before starting the export process.

            string themeFolderPath = _outputPath.CreateThemeFolderAtDest(theme, _log);

            if (string.IsNullOrEmpty(themeFolderPath))
            {
                _log.Warning("Error: Failed to create theme folder at destination.");
                return false;
            }

            bool createdJson = theme.CreateThemeJson(themeFolderPath, _log, _options);

            if (!createdJson)
            {
                _log.Warning("Error: Failed to create theme.json.");
                return false;
            }

            _log.Information("Theme.json created successfully. Starting image processing with PtexConv...");

            ConversionResult value = Run_Ptexconv(theme, _log);

            // Log the results of the conversion. If successful, report the results. If not, log the errors.
            if (value.Success)
            {
                _log.Information(value.Commands.IsAllCommandsPresent(theme));
                _log.Information("PtexConv processing completed successfully. Reporting results...");
                value.ReportPtexConvResult(_log);
                return true;
            }

            // If the conversion was not successful, log the errors. If there are no specific errors, log a generic message.
            if (value.HasErrors)
            {
                _log.Error("PtexConv encountered errors during processing:");
                foreach (string error in value.Errors)
                {
                    _log.Error($"- {error}");
                }
            }
            else
            {
                _log.Error("PtexConv failed to process the images, but no specific errors were reported.");
            }
            return false;
        }
        catch (Exception ex)
        {
            _log.Fatal($"An error occurred during export: {ex.Message}");
            return false;
        }
        finally
        {
            // Moving the move and delete operations to the finally block to ensure they run regardless of success or failure of the export process.
            string[] files = ["topbg", "bottombg", "bannerListCell", "bannerListCellSelected", "gridCell", "gridCellSelected", "scrim"];
            bool result = files.RemovedPngFiles(_log);
            if (!result)
            {
                _log.Error("Failed to remove temporary png files.");
            }

            bool moved = _outputPath.MoveFiles(theme, _log, FilesContants.TexBinFileSuffix, FilesContants.BinFiles, FilesContants.Wildcard_TexBinFiles);
            if (!moved)
            {
                _log.Error("Failed to move tex files");
            }

            bool movedPal = _outputPath.MoveFiles(theme, _log, FilesContants.PalBinFileSuffix, FilesContants.PlttBinFileSuffix, FilesContants.Wildcard_PalBinFiles);
            if (!movedPal)
            {
                _log.Error("Failed to move pal files");
            }
            _log.Information("Export complete.");
        }
    }

    /// <summary>
    /// Processes the images associated with various theme elements by converting them using the ptexconv tool.
    /// </summary>
    /// <remarks>Each theme element is processed individually, and any errors encountered during conversion
    /// are captured in the result. The overall success is determined by the presence or absence of errors in the
    /// conversion process.</remarks>
    /// <param name="theme">The theme containing the images to be converted, including backgrounds, banner list cells, grid cells, and
    /// scrims. Cannot be null.</param>
    /// <returns>A ConversionResult that indicates whether the conversion was successful and contains any errors encountered
    /// during processing.</returns>
    private ConversionResult Run_Ptexconv(NormalizedTheme theme, ILogger _log)
    {
        ConversionResult conversionResult = new();
        try
        {
            if (theme.TopBackground != null)
            {
                try
                {
                    _log.Information("Saving top background...");
                    string topBgPath = theme.TopBackground.SaveTempPng("topbg", _log);
                    PtexConvCommand TopBGcommand = new()
                    {
                        InputImage = "topbg.png",
                        OutputBaseName = "topbg",
                        IsTexture = true,
                        TextureFormat = ETextureFormat.Direct,
                    };
                    _log.Information("Running ptexconv on top background...");
                    bool topresult = _ptexConvRunner.Run(TopBGcommand.ToString());
                    conversionResult.Commands.Add("topbg", topresult);
                }
                catch (Exception ex)
                {
                    conversionResult.Commands.Add("topbg", false);
                    conversionResult.Errors.Add($"Top background processing failed: {ex.Message}");
                }
            }

            if (theme.BottomBackground != null)
            {
                try
                {
                    _log.Information("Saving bottom background...");
                    string bottomBgPath = theme.BottomBackground.SaveTempPng("bottombg", _log);
                    PtexConvCommand BottomBGcommand = new()
                    {
                        InputImage = "bottombg.png",
                        OutputBaseName = "bottombg",
                        IsTexture = true,
                        TextureFormat = ETextureFormat.Direct,
                    };
                    _log.Information("Running ptexconv on bottom background...");
                    bool bottomresult = _ptexConvRunner.Run(BottomBGcommand.ToString());
                    conversionResult.Commands.Add("bottombg", bottomresult);
                }
                catch (Exception ex)
                {
                    conversionResult.Commands.Add("bottombg", false);
                    conversionResult.Errors.Add($"Bottom background processing failed: {ex.Message}");
                }
            }

            if (theme.BannerListCell != null)
            {
                try
                {
                    _log.Information("Saving banner list cell...");
                    string bannerListCellPath = theme.BannerListCell.SaveTempPng("bannerListCell", _log);
                    PtexConvCommand BannerListCellCommand = new()
                    {
                        InputImage = "bannerListCell.png",
                        OutputBaseName = "bannerListCell",
                        IsTexture = true,
                        TextureFormat = ETextureFormat.A3I5,
                    };
                    _log.Information("Running ptexconv on banner list cell...");
                    bool bannerListCellResult = _ptexConvRunner.Run(BannerListCellCommand.ToString());
                    conversionResult.Commands.Add("bannerListCell", bannerListCellResult);
                }
                catch (Exception ex)
                {
                    conversionResult.Commands.Add("bannerListCell", false);
                    conversionResult.Errors.Add($"Banner list cell processing failed: {ex.Message}");
                }
            }

            if (theme.BannerListCellSelected != null)
            {
                try
                {
                    _log.Information("Saving banner list cell selected...");
                    string bannerListCellSelectedPath = theme.BannerListCellSelected.SaveTempPng("bannerListCellSelected", _log);
                    PtexConvCommand BannerListCellSelectedCommand = new()
                    {
                        InputImage = "bannerListCellSelected.png",
                        OutputBaseName = "bannerListCellSelected",
                        IsTexture = true,
                        TextureFormat = ETextureFormat.A3I5,
                    };
                    _log.Information("Running ptexconv on banner list cell selected...");
                    bool bannerListCellSelectedResult = _ptexConvRunner.Run(BannerListCellSelectedCommand.ToString());
                    conversionResult.Commands.Add("bannerListCellSelected", bannerListCellSelectedResult);
                }
                catch (Exception ex)
                {
                    conversionResult.Commands.Add("bannerListCellSelected", false);
                    conversionResult.Errors.Add($"Banner list cell selected processing failed: {ex.Message}");
                }
            }


            if (theme.GridCell != null)
            {
                try
                {
                    _log.Information("Saving grid cell...");
                    string gridCellPath = theme.GridCell.SaveTempPng("gridCell", _log);
                    PtexConvCommand GridCellCommand = new()
                    {
                        InputImage = "gridCell.png",
                        OutputBaseName = "gridCell",
                        IsTexture = true,
                        TextureFormat = ETextureFormat.A3I5,
                    };
                    _log.Information("Running ptexconv on grid cell...");
                    bool gridCellResult = _ptexConvRunner.Run(GridCellCommand.ToString());
                    conversionResult.Commands.Add("gridCell", gridCellResult);
                }
                catch (Exception ex)
                {
                    conversionResult.Commands.Add("gridCell", false);
                    conversionResult.Errors.Add($"Grid cell processing failed: {ex.Message}");
                }
            }

            if (theme.GridCellSelected != null)
            {
                try
                {
                    _log.Information("Saving grid cell selected...");
                    string gridCellSelectedPath = theme.GridCellSelected.SaveTempPng("gridCellSelected", _log);
                    PtexConvCommand GridCellSelectedCommand = new()
                    {
                        InputImage = "gridCellSelected.png",
                        OutputBaseName = "gridCellSelected",
                        IsTexture = true,
                        TextureFormat = ETextureFormat.A3I5,
                    };
                    _log.Information("Running ptexconv on grid cell selected...");
                    bool gridCellSelectedResult = _ptexConvRunner.Run(GridCellSelectedCommand.ToString());
                    conversionResult.Commands.Add("gridCellSelected", gridCellSelectedResult);
                }
                catch (Exception ex)
                {
                    conversionResult.Commands.Add("gridCellSelected", false);
                    conversionResult.Errors.Add($"Grid cell selected processing failed: {ex.Message}");
                }
            }

            if (theme.Scrim != null)
            {
                try
                {
                    _log.Information("Saving scrim...");
                    string scrimPath = theme.Scrim.SaveTempPng("scrim", _log);
                    PtexConvCommand ScrimCommand = new()
                    {
                        InputImage = "scrim.png",
                        OutputBaseName = "scrim",
                        IsTexture = true,
                        TextureFormat = ETextureFormat.A5I3,
                    };
                    _log.Information("Running ptexconv on scrim...");
                    bool scrimResult = _ptexConvRunner.Run(ScrimCommand.ToString());
                    conversionResult.Commands.Add("scrim", scrimResult);
                }
                catch (Exception ex)
                {
                    conversionResult.Commands.Add("scrim", false);
                    conversionResult.Errors.Add($"Scrim processing failed: {ex.Message}");
                }
            }

            // Determine overall success based on individual command results
            conversionResult.Success = !conversionResult.HasErrors;

            return conversionResult;
        }
        catch (Exception ex)
        {
            _log.Error($"An error occurred while running ptexconv: {ex.Message}");
            conversionResult.Errors.Add($"An error occurred while running ptexconv: {ex.Message}");
            conversionResult.Success = false;
            return conversionResult;
        }
    }
}

