using LulCaster.UI.WPF.Utility;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers.Events;
using LulCaster.Utility.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace LulCaster.UI.WPF.Workers
{
  public class RegionWorker : LulWorkerBase
  {
    private RegionViewModel _region;
    private ScreenCapture _screenCapture;
    private IOcrService _ocrService = new OcrService();

    public RegionWorker(ScreenCapture screenCapture, RegionViewModel region)
    {
      _region = region;
      _screenCapture = screenCapture;
    }

    protected override void DoWork()
    {
      if (_region.BoundingBox.Width < 1 || _region.BoundingBox.Height < 1)
      {
        Stop();
        return;
      }

      using (var croppedImage = CropBitmap(_screenCapture, _region.BoundingBox))
      {
        //TODO: Do we want to remove this? Or do we want to do it better?
        //using (MemoryStream memory = new MemoryStream())
        //{
        //  using (FileStream fs = new FileStream(@"C:\Users\David\Documents\Overwatch\poo2.bmp", FileMode.Create, FileAccess.ReadWrite))
        //  {
        //    croppedImage.Save(memory, ImageFormat.Jpeg);
        //    byte[] bytes = memory.ToArray();
        //    fs.Write(bytes, 0, bytes.Length);
        //  }
        //}

        var scrappedText = ScrapeImage(croppedImage);
        ProcessTriggers(_region.Triggers, _screenCapture, scrappedText);
      }

      IsRunning = false;
    }

    private Bitmap CropBitmap(ScreenCapture screenCapture, Rectangle regionBoundingBox)
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

    private void ProcessTriggers(IList<TriggerViewModel> triggers, ScreenCapture screenCapture, string scrappedText)
    {
      foreach (var trigger in triggers)
      {
        //TODO: This needs to eventually tap into a trigger factory.
        if (scrappedText.Contains(trigger.ThresholdValue))
        {
          TriggerEmitter.OnTriggerActivated(new Events.Arguments.TriggerSoundArgs()
          {
            FilePath = trigger.SoundFilePath
          });

          Console.WriteLine($"Seconds before trigger was processed: {DateTime.Now.Subtract(screenCapture.CreationTime).TotalSeconds}");
        }
      }
    }

    private string ScrapeImage(Bitmap image)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        image.Save(memoryStream, ImageFormat.Tiff);

        return _ocrService.ProcessImage(memoryStream);
      }
    }

    private BitmapImage ConvertStreamToBitmap(MemoryStream imageStream)
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