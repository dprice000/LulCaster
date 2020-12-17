using LulCaster.UI.WPF.Utility;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers.Events;
using LulCaster.Utility.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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

      using (var croppedImage = BitmapHelper.CropBitmap(_screenCapture, _region.BoundingBox))
      using (var resizedImage = BitmapHelper.ResizeBitmap(croppedImage, _region.BoundingBox, 2))
      {
        var scrappedText = ScrapeImage(croppedImage);
        ProcessTriggers(_region.Triggers, _screenCapture, scrappedText);
      }

      IsRunning = false;
    }

    private void ProcessTriggers(IList<TriggerViewModel> triggers, ScreenCapture screenCapture, string scrappedText)
    {
      foreach (var trigger in triggers)
      {
        //TODO: This needs to eventually tap into a trigger factory.
        if (scrappedText.Contains(trigger.ThresholdValue, StringComparison.OrdinalIgnoreCase))
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
  }
}