using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers;
using LulCaster.UI.WPF.Workers.Events.Arguments;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace LulCaster.UI.WPF.Utility
{
  public static class BitmapHelper
  {
    public static Bitmap CropBitmap(ScreenCapture screenCapture, Rectangle regionBoundingBox)
    {
      using (var image = (Image)screenCapture.Image)
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

    public static BitmapImage ConvertByteArrayToBitmapImage(byte[] byteImage)
    {
      var imageStream = new MemoryStream(byteImage);
      var screenCaptureImage = new BitmapImage();
      screenCaptureImage.BeginInit();
      screenCaptureImage.StreamSource = imageStream;
      screenCaptureImage.CacheOption = BitmapCacheOption.OnLoad;
      screenCaptureImage.EndInit();
      screenCaptureImage.Freeze();

      return screenCaptureImage;
    }

    internal static ScreenCapture ConvertArgsToScreenCap(ScreenCaptureCompletedArgs args, IList<RegionViewModel> regions, DateTime timeCreated)
    {
      return new ScreenCapture()
      {
        Image = args.Image,
        ScreenBitmap = args.BitmapImage,
        RegionViewModels = regions,
        ScreenBounds = args.ScreenBounds,
        CanvasBounds = args.CanvasBounds,
        CreationTime = timeCreated
      };
    }

    internal static byte[] ImageToByteArray(Image imageIn)
    {
      using (var ms = new MemoryStream())
      {
        imageIn.Save(ms, ImageFormat.Bmp);
        return ms.ToArray();
      }
    }
  }
}