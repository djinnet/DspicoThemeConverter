using DspicoThemeForms.Core.ThemeNormalizationLayer;
using System.Drawing.Imaging;

namespace DspicoThemeForms.Core.Helper;

public static class ToolsDirHelper
{
    /// <summary>
    /// Saves the specified Bitmap as a temporary PNG file in the tools directory and returns the full path to the saved
    /// file.
    /// </summary>
    /// <remarks>If a file with the specified name already exists in the tools directory, it will be
    /// overwritten. Ensure that the tools directory is properly configured before calling this method.</remarks>
    /// <param name="bmp">The Bitmap image to be saved as a PNG file. This parameter must not be null.</param>
    /// <param name="name">The name to use for the temporary PNG file, without the extension. This parameter must not be null or empty.</param>
    /// <param name="_log">An action delegate used to log messages, including errors and warnings that occur during the save process.</param>
    /// <returns>The full path to the saved PNG file. Returns an empty string if the save operation fails due to invalid
    /// parameters or configuration.</returns>
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

    /// <summary>
    /// Deletes PNG files from the tools directory that match the specified file names.
    /// </summary>
    /// <remarks>If the names array is null or empty, or if the tools directory path is invalid, the method
    /// logs an appropriate message and returns false. The method does not throw exceptions for missing files; it
    /// silently skips files that do not exist.</remarks>
    /// <param name="names">An array of file name strings, without extensions, identifying the PNG files to delete. The array must not be
    /// null or empty.</param>
    /// <param name="_log">An action delegate used to log messages related to the deletion process, including errors and status updates.</param>
    /// <returns>true if at least one file deletion was attempted; otherwise, false.</returns>
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

    /// <summary>
    /// Moves files from the tools directory to a specified output path, renaming each file according to the provided
    /// parameters and applying the given theme.
    /// </summary>
    /// <remarks>If the tools directory is not found or no files match the specified wildcard pattern, the
    /// method logs an error and returns false. Any failure to move or rename a file is also logged, and the method
    /// returns false if any such failure occurs.</remarks>
    /// <param name="_outputPath">The destination directory where the files will be moved.</param>
    /// <param name="theme">The theme to apply to each moved file, which may affect their appearance or formatting.</param>
    /// <param name="_log">An action delegate used to log informational or error messages during the file move process.</param>
    /// <param name="OldFileName">The substring in the original file names to be replaced during the renaming process.</param>
    /// <param name="NewFileName">The substring that will replace the old file name in each moved file.</param>
    /// <param name="wildcard">The wildcard pattern used to select files in the tools directory for moving.</param>
    /// <returns>true if all files are successfully moved and renamed; otherwise, false.</returns>
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

    /// <summary>
    /// Moves a specified file from the tools directory to a themed subfolder within the designated output path.
    /// </summary>
    /// <remarks>If the origin theme is not specified, the method uses 'None' as the default origin theme. The
    /// destination folder is created based on the origin theme and theme name. Any errors encountered during the move
    /// are logged using the provided delegate.</remarks>
    /// <param name="_outputPath">The directory path where the destination file will be placed. Must be a valid, writable location.</param>
    /// <param name="sourceFileName">The name of the source file to move. The file must exist in the tools directory.</param>
    /// <param name="destFileName">The name to assign to the file after it is moved to the output path.</param>
    /// <param name="theme">A theme object that determines the folder structure for the output file, including the origin theme and theme
    /// name.</param>
    /// <param name="_log">An action delegate used to log informational, warning, or error messages during the file move operation.</param>
    /// <returns>true if the file was successfully moved to the themed output folder; otherwise, false.</returns>
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

    /// <summary>
    /// Creates a theme folder at the specified output path using the provided theme information and logs the process.
    /// </summary>
    /// <remarks>If the theme folder already exists at the destination, no new folder is created and a warning
    /// is logged. The method substitutes default values for missing theme name or origin type and logs corresponding
    /// warnings.</remarks>
    /// <param name="_outputPath">The destination directory path where the theme folder will be created. Must be a valid file system path.</param>
    /// <param name="theme">An object containing the theme's name and origin type. If the name is null or empty, 'Unnamed' will be used; if
    /// the origin type is null, 'None' will be used.</param>
    /// <param name="_log">A delegate that receives log messages about the folder creation process, including warnings and errors.</param>
    /// <returns>The full path of the created or existing theme folder. Returns an empty string if an error occurs during folder
    /// creation.</returns>
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
