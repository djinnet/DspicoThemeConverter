namespace DspicoThemeForms.Core.Helper
{
    public class PathHelper
    {
        public static string GetToolsDirectory()
        {
            var exeDir = AppDomain.CurrentDomain.BaseDirectory;
            var toolsDir = Path.Combine(exeDir, "Tools");
            if (!Directory.Exists(toolsDir))
                throw new DirectoryNotFoundException($"Tools directory not found at expected location: {toolsDir}");
            return toolsDir;
        }
    }
}
