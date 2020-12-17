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

    public static Bitmap ResizeBitmap(Bitmap bitmap, Rectangle boundingBox, int multiplier)
    {
      int width = boundingBox.Width * multiplier;
      int height = boundingBox.Width * multiplier;

      Bitmap result = new Bitmap(width, height);
      using (Graphics graphic = Graphics.FromImage(result))
      {
        graphic.DrawImage(bitmap, 0, 0, width, height);
      }

      return result;
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