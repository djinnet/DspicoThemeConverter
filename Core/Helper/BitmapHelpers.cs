using System.Drawing.Imaging;

namespace DspicoThemeForms.Core.Helper
{
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

        public static Bitmap LoadBitmap(string path)
        {
            // Avoid locking the file on disk
            using var temp = new Bitmap(path);
            return new Bitmap(temp);
        }

        public static void ValidateResolution(Bitmap bmp, int width, int height, string name)
        {
            if (bmp.Width != width || bmp.Height != height)
            {
                throw new InvalidOperationException($"{name} must be {width}x{height}, but was {bmp.Width}x{bmp.Height}");
            }
        }

        public static bool IsDarkTheme(Bitmap bmp)
        {
            // Very naive for now; good enough
            var avg = ExtractPrimaryColor(bmp);
            return (avg.R + avg.G + avg.B) < (128 * 3);
        }

        public static Color ExtractPrimaryColor(Bitmap bmp)
        {
            // keep it simple
            var x = bmp.Width / 2;
            var y = bmp.Height / 2;
            return bmp.GetPixel(x, y);
        }
    }
}
