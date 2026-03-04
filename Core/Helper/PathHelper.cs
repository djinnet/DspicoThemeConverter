using DspicoThemeForms.Core.Constants;

namespace DspicoThemeForms.Core.Helper;

/// <summary>
/// Provides utility methods for retrieving file system paths related to the application's tools directory and
/// associated executables.
/// </summary>
/// <remarks>Use this class to obtain the locations of the tools directory and the ptexconv executable within the
/// application's base directory. The methods ensure that the required files and directories exist before returning
/// their paths, allowing callers to verify availability prior to usage.</remarks>
public class PathHelper
{
    /// <summary>
    /// Gets the full path to the Tools directory located in the application's base directory.
    /// </summary>
    /// <remarks>This method locates the Tools directory relative to the application's base directory. If the
    /// directory is missing, a DirectoryNotFoundException is thrown before returning an empty string.</remarks>
    /// <returns>A string containing the path to the Tools directory. Returns an empty string if the directory cannot be
    /// accessed.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown if the Tools directory does not exist at the expected location.</exception>
    public static string GetToolsDirectory()
    {
        try
        {
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string toolsDir = Path.Combine(exeDir, FilesContants.ToolsDirectoryName);
            if (!Directory.Exists(toolsDir))
            {
                throw new DirectoryNotFoundException($"Tools directory not found at expected location: {toolsDir}");
            }

            return toolsDir;
        }
        catch (Exception ex)
        {
            // Log the exception as needed
            Console.WriteLine($"Error getting tools directory: {ex.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the full file path to the ptexconv executable if it is available in the tools directory.
    /// </summary>
    /// <remarks>This method checks for the existence of the tools directory and the ptexconv executable
    /// within it. If either is missing, the method returns an empty string. Use this method to determine the location
    /// of ptexconv before attempting to execute or reference it.</remarks>
    /// <returns>The full path to the ptexconv executable if it exists; otherwise, an empty string.</returns>
    public static string GetPtexConvPath()
    {
        string toolsDir = GetToolsDirectory();
        if (string.IsNullOrEmpty(toolsDir))
        {
            return string.Empty; // Tools directory not found
        }
        string ptexConvPath = Path.Combine(toolsDir, FilesContants.PtexConvExeName);
        if (!File.Exists(ptexConvPath))
        {
            Console.WriteLine($"ptexconv.exe not found at expected location: {ptexConvPath}");
            return string.Empty; // ptexconv.exe not found
        }
        return ptexConvPath;
    }
}
