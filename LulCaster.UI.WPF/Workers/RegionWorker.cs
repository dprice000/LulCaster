using LulCaster.UI.WPF.Utility;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers.Events;
using LulCaster.Utility.Service;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace LulCaster.UI.WPF.Workers
{
  public class RegionWorker : LulWorkerBase
  {
    private readonly RegionViewModel _region;
    private readonly ScreenCapture _screenCapture;
    private readonly IOcrService _ocrService = new OcrService();
    private const int IMAGE_MULTIPLIER = 2;

    public RegionWorker(ScreenCapture screenCapture, RegionViewModel region, int idleTimeout) : base(idleTimeout)
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

      using (var screenCapture = _screenCapture)
      using (var croppedImage = BitmapHelper.CropBitmap(_screenCapture, _region.BoundingBox))
      using (var resizedImage = BitmapHelper.ResizeBitmap(croppedImage, croppedImage.Width, croppedImage.Height, IMAGE_MULTIPLIER))
      using (var greyScaledImage = BitmapHelper.SetGrayscale(resizedImage))
      {
        var scrappedText = ScrapeImage(greyScaledImage);
        scrappedText = Regex.Replace(scrappedText, @"[^0-9a-zA-Z]", "");
        ProcessTriggers(_region, screenCapture, scrappedText);
      }

      IsRunning = false;
    }

    private void ProcessTriggers(RegionViewModel region, ScreenCapture screenCapture, string scrappedText)
    {
      if (region.RegionType != LulCaster.Utility.Common.Enums.RegionTypes.SimpleString)
      {

      }

      foreach (var trigger in region?.Triggers)
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