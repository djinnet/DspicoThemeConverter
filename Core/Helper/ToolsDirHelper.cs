using DspicoThemeForms.Core.ThemeNormalizationLayer;
using System.Drawing.Imaging;

namespace DspicoThemeForms.Core.Helper;

public static class ToolsDirHelper
{
    public static string SaveTempPng(this Bitmap bmp, string name, Action<string> _log)
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

    public static bool RemovedPngFiles(this string[] names, Action<string> _log)
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

        foreach (string name in names)
        {
            string path = Path.Combine(toolsDirectory, $"{name}.png");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        return true;
    }

    public static bool MoveFiles(this string _outputPath, NormalizedTheme theme, Action<string> _log, string OldFileName, string NewFileName, string wildcard)
    {
        string toolsDirectory = PathHelper.GetToolsDirectory();
        if (string.IsNullOrEmpty(toolsDirectory))
        {
            _log("Error: Tools directory path is null or empty.");
            return false;
        }

        string[] findAllPalFiles = Directory.GetFiles(toolsDirectory, wildcard);
        if (findAllPalFiles.Length == 0)
        {
            _log("No palette files found to move.");
            return false;
        }
        bool success = true;
        foreach (string file in findAllPalFiles)
        {
            string fileName = Path.GetFileName(file);
            string originalFileName = fileName.Replace(OldFileName, NewFileName);
            bool moved = MoveOutputFile(_outputPath, fileName, originalFileName, theme, _log);
            if (!moved)
            {
                _log($"Failed to move output file: {fileName}");
                success = false;
            }
        }
        return success;
    }

    public static bool MoveOutputFile(string _outputPath, string sourceFileName, string destFileName, NormalizedTheme theme, Action<string> _log)
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

    public static string CreateThemeFolderAtDest(this string _outputPath, NormalizedTheme theme, Action<string> _log)
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
                Directory.CreateDirectory(themeFolderPath);
            }
            else
            {
                _log($"Theme folder already exists at: {themeFolderPath}.");
            }
            return themeFolderPath;
        }
        catch (Exception ex)
        {
            _log($"Error creating theme folder at destination: {ex.Message}");
            return string.Empty;
        }
    }
}
