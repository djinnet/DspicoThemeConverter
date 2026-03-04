using System.Drawing.Imaging;

namespace DspicoThemeForms.Core.Helper;

/// <summary>
/// Provides static utility methods for preparing, loading, validating, and analyzing bitmap images in .NET
/// applications.
/// </summary>
/// <remarks>BitmapHelpers includes methods for creating bitmaps with specific dimensions and transparency,
/// loading images from disk without locking files, validating bitmap resolutions, and determining if a bitmap
/// represents a dark theme based on its primary color. These helpers are intended to simplify common bitmap operations
/// and ensure consistent image handling across applications.</remarks>
public class BitmapHelpers
{
    public static Bitmap Prepare(Bitmap src, int w, int h)
    {
        var bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.Transparent);
        g.DrawImage(src, 0, 0, w, h);
        return bmp;
    }

    /// <summary>
    /// Loads a bitmap image from the specified file path without locking the source file.
    /// </summary>
    /// <remarks>This method creates a temporary Bitmap to avoid locking the image file on disk, allowing the
    /// file to be accessed or modified after loading.</remarks>
    /// <param name="path">The file path of the bitmap image to load. This path must refer to an existing image file.</param>
    /// <returns>A new Bitmap instance that represents the loaded image.</returns>
    public static Bitmap LoadBitmap(string path)
    {
        // Avoid locking the file on disk
        using var temp = new Bitmap(path);
        return new Bitmap(temp);
    }

    /// <summary>
    /// Loads a bitmap image from the specified file path and returns a copy with a transparent background.
    /// </summary>
    /// <remarks>The returned bitmap uses a pixel format that supports transparency. If the specified file
    /// does not exist or cannot be opened, an exception will be thrown.</remarks>
    /// <param name="path">The file path of the image to load. This parameter cannot be null or empty.</param>
    /// <returns>A Bitmap object with a transparent background, representing the loaded image.</returns>
    public static Bitmap LoadBitmapWithTransparency(string path)
    {
        using var temp = new Bitmap(path);
        var bmp = new Bitmap(temp.Width, temp.Height, PixelFormat.Format32bppArgb);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.Transparent);
        g.DrawImage(temp, 0, 0, temp.Width, temp.Height);
        return bmp;
    }

    /// <summary>
    /// Validates that the specified bitmap matches the expected width and height.
    /// </summary>
    /// <param name="bmp">The bitmap image to validate. Cannot be null.</param>
    /// <param name="width">The expected width of the bitmap, in pixels.</param>
    /// <param name="height">The expected height of the bitmap, in pixels.</param>
    /// <param name="name">The name of the bitmap, used in the exception message if validation fails.</param>
    /// <exception cref="InvalidOperationException">Thrown if the bitmap's dimensions do not match the specified width and height.</exception>
    public static void ValidateResolution(Bitmap bmp, int width, int height, string name)
    {
        if (bmp.Width != width || bmp.Height != height)
        {
            throw new InvalidOperationException($"{name} must be {width}x{height}, but was {bmp.Width}x{bmp.Height}");
        }
    }

    /// <summary>
    /// Determines whether the specified bitmap image represents a dark theme based on its average color.
    /// </summary>
    /// <remarks>This method uses a simple approach by calculating the average color of the provided bitmap.
    /// It considers the theme dark if the combined RGB values of the average color are less than 384 (128 *
    /// 3).</remarks>
    /// <param name="bmp">The bitmap image to analyze for theme darkness. Cannot be null.</param>
    /// <returns>true if the average color of the bitmap is darker than mid-gray; otherwise, false.</returns>
    public static bool IsDarkTheme(Bitmap bmp)
    {
        // Very naive for now; good enough
        Color avg = ExtractPrimaryColor(bmp);

        // If the average color is darker than mid-gray, consider it a dark theme
        return (avg.R + avg.G + avg.B) < (128 * 3);
    }

    /// <summary>
    /// Extracts the color of the pixel located at the center of the specified bitmap image.
    /// </summary>
    /// <remarks>If the bitmap has zero width or height, the behavior is undefined. The method does not
    /// perform bounds checking and assumes the bitmap has a valid, non-empty size.</remarks>
    /// <param name="bmp">The bitmap image from which to extract the center pixel color. This parameter must not be null.</param>
    /// <returns>The color of the pixel at the center coordinates of the bitmap.</returns>
    public static Color ExtractPrimaryColor(Bitmap bmp)
    {
        // keep it simple
        int x = bmp.Width / 2;
        int y = bmp.Height / 2;
        return bmp.GetPixel(x, y);
    }
}
