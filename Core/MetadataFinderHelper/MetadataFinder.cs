using DspicoThemeForms.Core.Constants;
using DspicoThemeForms.Core.ThemeNormalizationLayer;

namespace DspicoThemeForms.Core.MetadataFinderHelper;

/// <summary>
/// Provides methods for locating, reading, and parsing theme metadata files to produce normalized theme objects.
/// </summary>
/// <remarks>The MetadataFinder class enables discovery and extraction of theme metadata from files, including
/// author, theme type, primary color, description, and version. It handles missing or malformed metadata gracefully by
/// supplying default values and ensures exceptions during file access or parsing do not disrupt usage. This class is
/// intended for scenarios where robust theme metadata handling is required, such as theme conversion or validation
/// workflows.</remarks>
public class MetadataFinder
{
    /// <summary>
    /// Determines whether a metadata file exists in the specified directory.
    /// </summary>
    /// <remarks>This method checks for the presence of a file named 'metadata.ini' in the provided
    /// directory.</remarks>
    /// <param name="directory">The path of the directory to search for the metadata file. This parameter cannot be null or empty.</param>
    /// <returns>true if the metadata file exists; otherwise, false.</returns>
    private static bool TryFindMetadataFile(string directory)
    {
        string metadataFilePath = Path.Combine(directory, FilesContants.MetadataFileName);
        return File.Exists(metadataFilePath);
    }

    /// <summary>
    /// Attempts to read the contents of the metadata file located in the specified directory.
    /// </summary>
    /// <remarks>If the metadata file is not found, the method returns an empty string without throwing an
    /// exception. Any exceptions encountered during file reading are caught and handled internally.</remarks>
    /// <param name="directory">The path to the directory where the metadata file is expected to be located. This parameter cannot be null or
    /// empty.</param>
    /// <returns>The contents of the metadata file as a string if found; otherwise, an empty string.</returns>
    private static string? TryReadMetadataFile(string directory)
    {
        try
        {
            string metadataFilePath = Path.Combine(directory, FilesContants.MetadataFileName);
            return !TryFindMetadataFile(directory) ? string.Empty : File.ReadAllText(metadataFilePath);
        }
        catch (Exception ex)
        {
            // Log or handle exceptions as needed
            return string.Empty;
        }
    }

    /// <summary>
    /// Parses the specified theme metadata file and returns a normalized theme object containing its properties.
    /// </summary>
    /// <remarks>The method attempts to read various properties from the metadata file, including author,
    /// theme type (dark or light), primary color, description, and version. If any of these properties are not
    /// specified in the metadata, default values are used. The method handles potential exceptions during parsing and
    /// defaults to a white primary color if the specified color format is invalid.</remarks>
    /// <param name="path">The path to the theme metadata file. This must be a valid file path and cannot be null or empty.</param>
    /// <returns>A NormalizedTheme object populated with the theme's properties, including the author's name, theme type, primary
    /// color, description, and version. If the metadata file is missing or empty, a default theme is returned.</returns>
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
                if (authorEndIndex == -1)
                {
                    authorEndIndex = metadataContent.Length; // Handle case where it's the last line
                }

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
                if (typeEndIndex == -1)
                {
                    typeEndIndex = metadataContent.Length; // Handle case where it's the last line
                }

                string themeType = metadataContent[typeStartIndex..typeEndIndex].Trim();
                if (string.IsNullOrEmpty(themeType))
                {
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
                if (colorEndIndex == -1)
                {
                    colorEndIndex = metadataContent.Length; // Handle case where it's the last line
                }

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
                if (descEndIndex == -1)
                {
                    descEndIndex = metadataContent.Length; // Handle case where it's the last line
                }

                description = metadataContent[descStartIndex..descEndIndex].Trim();
                if (string.IsNullOrEmpty(description))
                {
                    description = "No description provided.";
                }
            }

            if (metadataContent.Contains("version ="))
            {
                int versionStartIndex = metadataContent.IndexOf("version =") + "version =".Length;
                int versionEndIndex = metadataContent.IndexOf('\n', versionStartIndex);
                if (versionEndIndex == -1)
                {
                    versionEndIndex = metadataContent.Length; // Handle case where it's the last line
                }

                version = metadataContent[versionStartIndex..versionEndIndex].Trim();
            }

            if (metadataContent.Contains("name ="))
            {
                int nameStartIndex = metadataContent.IndexOf("name =") + "name =".Length;
                int nameEndIndex = metadataContent.IndexOf('\n', nameStartIndex);
                if (nameEndIndex == -1)
                {
                    nameEndIndex = metadataContent.Length; // Handle case where it's the last line
                }

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
