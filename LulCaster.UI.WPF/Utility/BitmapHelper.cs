using LulCaster.UI.WPF.Workers;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace LulCaster.UI.WPF.Utility
{
  public static class BitmapHelper
  {
    public static Bitmap CropBitmap(ScreenCapture screenCapture, Rectangle regionBoundingBox)
    {
      using (var image = Image.FromStream(screenCapture.ScreenMemoryStream))
      {
        var relativeBounds = AspectRatioConverter.ResolveScreenLocation(screenCapture.ScreenBounds, screenCapture.CanvasBounds, regionBoundingBox);

        Bitmap croppedBitmap = new Bitmap(relativeBounds.Width, relativeBounds.Height);

        using (Graphics graphics = Graphics.FromImage(croppedBitmap))
        {
          graphics.DrawImage(image, new Rectangle(0, 0, croppedBitmap.Width, croppedBitmap.Height), relativeBounds,
                           GraphicsUnit.Pixel);
        }

        return croppedBitmap;
      }
    }

    public static Bitmap ResizeBitmap(Bitmap bitmap, int width, int height, int multiplier)
    {
      width *= multiplier;
      height *= multiplier;

      Bitmap result = new Bitmap(width, height);
      using (Graphics graphic = Graphics.FromImage(result))
      {
        graphic.DrawImage(bitmap, 0, 0, width, height);
      }

      return result;
    }

    public static Bitmap SetGrayscale(Bitmap img)
    {
      LockedBitmap lockedBmp = new LockedBitmap((Bitmap)img.Clone());
      lockedBmp.LockBits(); // lock the bits for faster access
      Color c;
      for (int i = 0; i < lockedBmp.Width; i++)
      {
        for (int j = 0; j < lockedBmp.Height; j++)
        {
          c = lockedBmp.GetPixel(i, j);
          byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);

          lockedBmp.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
        }
      }
      lockedBmp.UnlockBits(); // remember to release resources
      return lockedBmp.Bitmap; // return the bitmap (you don't need to clone it again, that's already been done).
    }

    public static BitmapImage ConvertStreamToBitmap(MemoryStream imageStream)
    {
      var imageBitmap = new BitmapImage();
      imageBitmap.BeginInit();
      imageBitmap.StreamSource = imageStream;
      imageBitmap.CacheOption = BitmapCacheOption.OnLoad;
      imageBitmap.EndInit();
      imageBitmap.Freeze();

      return imageBitmap;
    }
  }
}