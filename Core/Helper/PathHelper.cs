using DspicoThemeForms.Core.Constants;

namespace DspicoThemeForms.Core.Helper;

public class PathHelper
{
    public static string GetToolsDirectory()
    {
        try
        {
            var exeDir = AppDomain.CurrentDomain.BaseDirectory;
            var toolsDir = Path.Combine(exeDir, "Tools");
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
