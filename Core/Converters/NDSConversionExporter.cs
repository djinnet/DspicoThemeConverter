using DspicoThemeForms.Core.Helper;
using DspicoThemeForms.Core.Runners;
using DspicoThemeForms.Core.ThemeNormalizationLayer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Windows.Forms.Design.AxImporter;

namespace DspicoThemeForms.Core.Converters;

public class NDSConversionExporter
{
    private readonly System.Text.Json.JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    public async Task<bool> Export(string outputPath, NormalizedTheme theme, bool overwrittenallow)
    {
        ILogger _log = Log.Logger;
        try
        {
            if(theme == null) 
            {
                _log.Error("Theme is null. Cannot proceed with export.");
                return false;
            }

            if (string.IsNullOrEmpty(outputPath))
            {
                _log.Error("Output path is null or empty. Cannot proceed with export.");
                return false;
            }

            string themeFolderPath = outputPath.CreateThemeFolderAtDest(theme, _log);

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

            _log.Information("Theme.json created successfully. Starting image processing with NDSConversion...");

            ConversionResult? conversionResult = await RunNDSConversion(theme, themeFolderPath, _log);

            if(conversionResult == null)
            {
                _log.Error("Conversion result is null. Something went wrong during conversion.");
                return false;
            }

            if (conversionResult.Success)
            {
                _log.Information("NDSConversion completed successfully. All images processed without errors.");
                return true;
            }


            if (conversionResult.HasErrors)
            {
                _log.Error("NDSConversion encountered errors during processing:");
                foreach (string error in conversionResult.Errors)
                {
                    _log.Error($"- {error}");
                }
            }
            else
            {
                _log.Error("NDSConversion failed to process the images, but no specific errors were reported.");
            }
            return false;
        }
        catch (Exception ex)
        {
            _log.Error($"Unexpected error during export: {ex.Message}");
            return false;
        }
    }

    private async Task<ConversionResult?> RunNDSConversion(NormalizedTheme theme, string themepath, ILogger _log)
    {
        ConversionResult conversionResult = new();
        try
		{
            if(theme.TopBackground != null) 
            {
                try
                {
                    Encoding(EncodingFormat.Direct, theme.TopBackground, "topbg", themepath, _log);
                }
                catch (Exception rx)
                {
                    conversionResult.Commands.Add("topbg", false);
                    conversionResult.Errors.Add($"Error processing top background: {rx.Message}");
                }
            }

            if(theme.BottomBackground != null) 
            {
                try
                {
                    Encoding(EncodingFormat.Direct, theme.BottomBackground, "bottombg", themepath, _log);
                }
                catch (Exception rx)
                {
                    conversionResult.Commands.Add("bottombg", false);
                    conversionResult.Errors.Add($"Error processing bottom background: {rx.Message}");
                }
            }

            if(theme.BannerListCell != null)
            {
                try
                {
                    Encoding(EncodingFormat.A3I5, theme.BannerListCell, "bannerListCell", themepath, _log);
                }
                catch (Exception ex)
                {
                    conversionResult.Commands.Add("bannerListCell", false);
                    conversionResult.Errors.Add($"Error processing banner list cell: {ex.Message}");
                }
            }

            if(theme.BannerListCellSelected != null)
            {
                try
                {
                    Encoding(EncodingFormat.A3I5, theme.BannerListCellSelected, "bannerListCellSelected", themepath, _log);
                }
                catch (Exception ex)
                {
                    conversionResult.Commands.Add("bannerListCellSelected", false);
                    conversionResult.Errors.Add($"Error processing banner list cell selected: {ex.Message}");
                }
            }

            if(theme.GridCell != null)
            {
                try
                {
                    Encoding(EncodingFormat.A3I5, theme.GridCell, "gridCell", themepath, _log);
                }
                catch (Exception ex)
                {
                    conversionResult.Commands.Add("gridCell", false);
                    conversionResult.Errors.Add($"Error processing grid cell: {ex.Message}");
                }
            }

            if(theme.GridCellSelected != null)
            {
                try
                {
                    Encoding(EncodingFormat.A3I5, theme.GridCellSelected, "gridCellSelected", themepath, _log);
                }
                catch (Exception ex)
                {
                    conversionResult.Commands.Add("gridCellSelected", false);
                    conversionResult.Errors.Add($"Error processing grid cell selected: {ex.Message}");
                }
            }


            if(theme.Scrim != null)
            {
                try
                {
                    Encoding(EncodingFormat.A5I3, theme.Scrim, "scrim", themepath, _log);
                }
                catch (Exception ex)
                {
                    conversionResult.Commands.Add("scrim", false);
                    conversionResult.Errors.Add($"Error processing scrim: {ex.Message}");
                }
            }

            await Task.Delay(100); // delay to ensure that the file system has completed all write operations before we finalize the conversion result
            conversionResult.Success = !conversionResult.HasErrors;
            return conversionResult;
        }
		catch (Exception ex)
		{
            _log.Error($"Unexpected error during NDS conversion: {ex.Message}");
            conversionResult.Errors.Add($"Unexpected error during NDS conversion: {ex.Message}");
            conversionResult.Success = false;
            return conversionResult;
		}
    }

    private enum EncodingFormat
    {
        A3I5,
        A5I3,
        Direct
    }

    private static bool Encoding(EncodingFormat format, Bitmap theme, string name, string themepath, ILogger _log)
    {
        try
        {
            if (string.IsNullOrEmpty(name))
            {
                _log.Error("Name for encoding is null or empty. Cannot proceed with encoding.");
                return false;
            }

            if (theme == null)
            {
                _log.Error($"{name} Theme bitmap is null. Cannot proceed with encoding.");
                return false;
            }

            byte[] EncodingData;
            List<Color>? Palette;

            switch (format)
            {
                case EncodingFormat.A3I5:
                    {
                        EncodingData = NDSTextureEncoder.EncodeA3I5(theme, out Palette);
                    }
                    break;
                case EncodingFormat.A5I3:
                    {
                        EncodingData = NDSTextureEncoder.EncodeA5I3(theme, out Palette);
                    }
                    break;
                case EncodingFormat.Direct:
                    {
                        EncodingData = NDSTextureEncoder.BitmapTo15Bpp(theme);
                        Palette = null;
                    }
                    break;
                default:
                    _log.Error($"Unsupported encoding format: {format}. Cannot proceed with encoding.");
                    return false;
            }

            string result = EncodingData.SaveBytesAsBinInThemeFolder(themepath, name, _log);
            if (string.IsNullOrEmpty(result))
            {
                _log.Warning($"Failed to save {name} data.");
            }
            else
            {
                _log.Information($"{name} data saved successfully.");
            }

            // For A3I5 and A5I3 formats, we also need to save the palette data. Direct format does not use a palette, so we skip it.
            if (format == EncodingFormat.A3I5 || format == EncodingFormat.A5I3)
            {
                if(Palette == null)
                {
                    _log.Error($"Palette data for {name} is null. Cannot proceed with saving palette.");
                    return false;
                }
                byte[] encodedPalette = NDSTextureEncoder.EncodePaletteBgr555(Palette);
                string paletteResult = encodedPalette.SaveBytesAsBinInThemeFolder(themepath, name + "Pltt", _log);
                if (string.IsNullOrEmpty(paletteResult))
                {
                    _log.Warning($"Failed to save {name} palette data.");
                }
                else
                {
                    _log.Information($"{name} palette data saved successfully.");
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            _log.Error($"Error during encoding {name}: {ex.Message}");
            return false;
        }
    }
}
