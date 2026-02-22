using DspicoThemeForms.Core.ThemeNormalizationLayer;

namespace DspicoThemeForms.Core.MetadataFinderHelper
{
    public class MetadataFinder
    {
        public static string FileName { get; set; } = "metadata.ini";

        public static bool TryFindMetadataFile(string directory)
        {
            string metadataFilePath = Path.Combine(directory, FileName);
            return File.Exists(metadataFilePath);
        }

        public static string? TryReadMetadataFile(string directory)
        {
            try
            {
                string metadataFilePath = Path.Combine(directory, FileName);
                return !File.Exists(metadataFilePath) ? string.Empty : File.ReadAllText(metadataFilePath);
            }
            catch (Exception)
            {
                // Log or handle exceptions as needed
                return null;
            }
        }

        public static NormalizedTheme Parse(string path)
        {
            NormalizedTheme theme = new()
            {
                Name = Path.GetFileName(path)
            };

            string? metadataContent = TryReadMetadataFile(path);
            if (string.IsNullOrEmpty(metadataContent))
            {
                return theme; // Return default theme if metadata is missing or empty
            }

            string author = string.Empty;
            bool isDarkTheme = false;
            Color primaryColor = Color.White; // Default to white if not specified
            string description = string.Empty;
            string version = string.Empty;
            string name = theme.Name;
            try
            {
                // Extract author's name
                if (metadataContent.Contains("author ="))
                {
                    int authorStartIndex = metadataContent.IndexOf("author =") + "author =".Length;
                    int authorEndIndex = metadataContent.IndexOf('\n', authorStartIndex);
                    if (authorEndIndex == -1) authorEndIndex = metadataContent.Length; // Handle case where it's the last line
                    author = metadataContent[authorStartIndex..authorEndIndex].Trim();
                    if (string.IsNullOrEmpty(author))
                    {
                        author = "Unknown Author";
                    }
                }

                // Extract theme type (dark or light)
                if (metadataContent.Contains("darktheme ="))
                {
                    int typeStartIndex = metadataContent.IndexOf("darktheme =") + "darktheme =".Length;
                    int typeEndIndex = metadataContent.IndexOf('\n', typeStartIndex);
                    if (typeEndIndex == -1) typeEndIndex = metadataContent.Length; // Handle case where it's the last line
                    string themeType = metadataContent[typeStartIndex..typeEndIndex].Trim();
                    if (string.IsNullOrEmpty(themeType)) {
                        themeType = "false"; // Default to light theme if not specified
                    }
                    isDarkTheme = themeType.Equals("true", StringComparison.OrdinalIgnoreCase);
                }

                // Extract primary color
                if (metadataContent.Contains("primarycolor ="))
                {
                    // Expected format: primarycolor = #RRGGBB
                    int colorStartIndex = metadataContent.IndexOf("primarycolor =") + "primarycolor =".Length;
                    int colorEndIndex = metadataContent.IndexOf('\n', colorStartIndex);
                    if (colorEndIndex == -1) colorEndIndex = metadataContent.Length; // Handle case where it's the last line
                    string colorValue = metadataContent[colorStartIndex..colorEndIndex].Trim();
                    if (colorValue.StartsWith('#') && colorValue.Length == 7)
                    {
                        try
                        {
                            int r = Convert.ToInt32(colorValue.Substring(1, 2), 16);
                            int g = Convert.ToInt32(colorValue.Substring(3, 2), 16);
                            int b = Convert.ToInt32(colorValue.Substring(5, 2), 16);
                            primaryColor = Color.FromArgb(r, g, b);
                        }
                        catch (FormatException)
                        {
                            // Handle invalid color format
                            primaryColor = Color.FromArgb(255, 255, 255); // Default to white
                        }
                    }
                }

                // extract description
                if (metadataContent.Contains("description ="))
                {
                    int descStartIndex = metadataContent.IndexOf("description =") + "description =".Length;
                    int descEndIndex = metadataContent.IndexOf('\n', descStartIndex);
                    if (descEndIndex == -1) descEndIndex = metadataContent.Length; // Handle case where it's the last line
                    description = metadataContent[descStartIndex..descEndIndex].Trim();
                    if(string.IsNullOrEmpty(description)) 
                        description = "No description provided.";
                }

                if(metadataContent.Contains("version ="))
                {
                    int versionStartIndex = metadataContent.IndexOf("version =") + "version =".Length;
                    int versionEndIndex = metadataContent.IndexOf('\n', versionStartIndex);
                    if (versionEndIndex == -1) versionEndIndex = metadataContent.Length; // Handle case where it's the last line
                    version = metadataContent[versionStartIndex..versionEndIndex].Trim();
                }

                if( metadataContent.Contains("name ="))
                {
                    int nameStartIndex = metadataContent.IndexOf("name =") + "name =".Length;
                    int nameEndIndex = metadataContent.IndexOf('\n', nameStartIndex);
                    if (nameEndIndex == -1) nameEndIndex = metadataContent.Length; // Handle case where it's the last line
                    name = metadataContent[nameStartIndex..nameEndIndex].Trim();
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed
                Console.WriteLine($"Error parsing metadata: {ex.Message}");
            }

            theme.Author = author;
            theme.DarkTheme = isDarkTheme;
            theme.PrimaryColor = primaryColor;
            theme.Description = description;
            theme.ThemeVersion = version;
            theme.Name = name;
            return theme;
        }
    }
}
