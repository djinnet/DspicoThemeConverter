using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace DspicoThemeForms.Core.Converters;

public static class NDSTextureEncoder
{
    /// <summary>
    /// Converts a 256x192 pixel Bitmap image to a 15 bits per pixel (15bpp) byte array representation.
    /// </summary>
    /// <remarks>Each pixel is converted to a 15bpp format, where the red, green, and blue components are
    /// mapped to 5 bits each. The alpha channel is used to determine opacity; pixels with an alpha value greater than
    /// 128 are marked as opaque in the output. The resulting array is suitable for use in systems or formats that
    /// require 15bpp image data.</remarks>
    /// <param name="bitmap">The Bitmap image to convert. Must be 256 pixels wide and 192 pixels high.</param>
    /// <returns>A byte array containing the 15bpp representation of the input Bitmap, with each pixel encoded as 2 bytes.</returns>
    // credit: https://github.com/santiagovalencia109/pl-Theme-Creator/tree/main
    public static byte[] BitmapTo15Bpp(Bitmap bitmap)
    {
        int width = 256;
        int height = 192;
        byte[] buffer = new byte[width * height * 2]; // 2 bytes per pixel for 15bpp

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color pixel = bitmap.GetPixel(x, y);
                int r = (int)Math.Round((pixel.R / 255.0) * 31);
                int g = (int)Math.Round((pixel.G / 255.0) * 31);
                int b = (int)Math.Round((pixel.B / 255.0) * 31);

                int color16 = (b << 10) | (g << 5) | r;
                if (pixel.A > 128)
                {
                    color16 |= (1 << 15);
                }

                int j = (y * width + x) * 2;
                buffer[j] = (byte)(color16 & 0xFF);
                buffer[j + 1] = (byte)((color16 >> 8) & 0xFF);
            }
        }
        return buffer;
    }

    public record EncodeSettings
    (
        int AlphaBits,
        int ColorBits,
        Func<Color, Color, double>? ColorDistance = null,
        Func<byte, int, byte>? AlphaQuantizer = null,
        IList<Color>? Palette = null
    );

    public static byte[] Encode(Bitmap bitmap, EncodeSettings settings, out List<Color> palette)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;

        int paletteSize = 1 << settings.ColorBits;
        int alphaLevels = 1 << settings.AlphaBits;

        Func<Color, Color, double> colorDistance = settings.ColorDistance ?? DefaultColorDistance;
        Func<byte, int, byte> alphaQuantizer = settings.AlphaQuantizer ?? DefaultAlphaQuantizer;

        palette = settings.Palette?.ToList() ?? GeneratePalette(bitmap, paletteSize);

        //Only for debugging - remove later
        //PrintDebugInfo($"A{settings.AlphaBits}I{settings.ColorBits}", settings.AlphaBits, settings.ColorBits, bitmap.Width, bitmap.Height, palette.Count);

        byte[] pixels = ExtractPixels(bitmap);

        byte[] output = new byte[width * height];
        int index = 0;

        for (int i = 0; i < pixels.Length; i += 4)
        {
            byte b = pixels[i + 0];
            byte g = pixels[i + 1];
            byte r = pixels[i + 2];
            byte a = pixels[i + 3];

            Color pixel = Color.FromArgb(a, r, g, b);

            byte quantAlpha = alphaQuantizer(a, alphaLevels);

            int paletteIndex = FindClosestColor(pixel, palette, colorDistance);

            byte packed = (byte)((quantAlpha << settings.ColorBits) | (paletteIndex & ((1 << settings.ColorBits) - 1)));

            output[index++] = packed;
        }

        return output;
    }

    public static byte[] EncodePaletteBgr555(IList<Color> palette)
    {
        byte[] output = new byte[palette.Count * 2];

        int index = 0;

        foreach (Color c in palette)
        {
            int r = c.R >> 3;
            int g = c.G >> 3;
            int b = c.B >> 3;

            ushort value = (ushort)((b << 10) | (g << 5) | r);

            output[index++] = (byte)(value & 0xFF);
            output[index++] = (byte)(value >> 8);
        }

        return output;
    }

    static void PrintDebugInfo(string formatName, int alphaBits, int colorBits, int width, int height, int paletteCount)
    {
        int maxColors = 1 << colorBits;
        int alphaLevels = 1 << alphaBits;
        var _log = Log.Logger;
        _log.Debug("==== Texture Encoding Debug ====");
        _log.Debug($"Format: {formatName}");
        _log.Debug($"Image Size: {width} x {height}");
        _log.Debug($"Alpha bits: {alphaBits}");
        _log.Debug($"Color index bits: {colorBits}");
        _log.Debug($"Max colors: {maxColors}");
        _log.Debug($"Alpha levels: {alphaLevels}");
        _log.Debug($"Palette entries: {paletteCount}");
        _log.Debug($"Output size: {width * height} bytes");
        _log.Debug("================================");
    }

    private static byte[] ExtractPixels(Bitmap bitmap)
    {
        Rectangle rect = new(0, 0, bitmap.Width, bitmap.Height);

        BitmapData data = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

        try
        {
            int byteCount = Math.Abs(data.Stride) * bitmap.Height;
            byte[] pixels = new byte[byteCount];

            Marshal.Copy(data.Scan0, pixels, 0, byteCount);

            if (data.Stride == bitmap.Width * 4)
            {
                return pixels;
            }

            // Remove stride padding
            byte[] trimmed = new byte[bitmap.Width * bitmap.Height * 4];

            int src = 0;
            int dst = 0;

            for (int y = 0; y < bitmap.Height; y++)
            {
                Buffer.BlockCopy(pixels, src, trimmed, dst, bitmap.Width * 4);
                src += data.Stride;
                dst += bitmap.Width * 4;
            }

            return trimmed;
        }
        finally
        {
            bitmap.UnlockBits(data);
        }
    }

    public static byte[] EncodeA3I5(Bitmap bitmap, out List<Color> palette)
    {
        return Encode(bitmap, new EncodeSettings(3, 5), out palette);
    }

    public static byte[] EncodeA5I3(Bitmap bitmap, out List<Color> palette)
    {
        return Encode(bitmap, new EncodeSettings(5, 3), out palette);
    }

    private static byte DefaultAlphaQuantizer(byte alpha, int levels)
    {
        double normalized = alpha / 255.0;
        return (byte)(int)Math.Round(normalized * (levels - 1));
    }

    private static int FindClosestColor(Color pixel, IList<Color> palette, Func<Color, Color, double> distance)
    {
        double best = double.MaxValue;
        int bestIndex = 0;

        for (int i = 0; i < palette.Count; i++)
        {
            double d = distance(pixel, palette[i]);
            if (d < best)
            {
                best = d;
                bestIndex = i;
            }
        }

        return bestIndex;
    }

    private static double DefaultColorDistance(Color a, Color b)
    {
        int dr = a.R - b.R;
        int dg = a.G - b.G;
        int db = a.B - b.B;
        return dr * dr + dg * dg + db * db;
    }

    private static List<Color> GeneratePalette(Bitmap bitmap, int maxColors)
    {
        byte[] pixels = ExtractPixels(bitmap);
        Dictionary<int, int> histogram = [];

        for (int i = 0; i < pixels.Length; i += 4)
        {
            int key = (pixels[i + 2] << 16) | (pixels[i + 1] << 8) | pixels[i + 0];

            histogram.TryGetValue(key, out int count);
            histogram[key] = count + 1;
        }

        return [.. histogram
            .OrderByDescending(k => k.Value)
            .Take(maxColors)
            .Select(k =>
            {
                int red = (k.Key >> 16) & 255;
                int green = (k.Key >> 8) & 255;
                int blue = k.Key & 255;
                return Color.FromArgb(red, green, blue);
            })];
    }
}
