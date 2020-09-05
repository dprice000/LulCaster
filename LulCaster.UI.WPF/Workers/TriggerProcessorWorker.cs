using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers.EventArguments;
using LulCaster.Utility.Common.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;
using LulCaster.Utility.Service;
using System.Drawing.Imaging;
using System.Threading;
using System.Media;
using System.Linq;

namespace LulCaster.UI.WPF.Workers
{
  public class TriggerProcessorWorker : LulWorkerBase
  {
    public event EventHandler<ScreenCaptureProgressArgs> ProgressChanged;

    private int _idleHaltTimeout = 2000;
    private Task _workerTask;
    private IOcrService _ocrService = new OcrService();

    public ConcurrentQueue<ScreenCapture> ProcessingQueue { get; set; } = new ConcurrentQueue<ScreenCapture>();

    protected override void DoWork()
    {
      _workerTask = Task.Run(() =>
      {
        while (IsRunning)
        {
          if (ProcessingQueue.IsEmpty)
          {
            Thread.Sleep(_idleHaltTimeout);
            continue;
          }

          ProcessingQueue.TryDequeue(out var screenCapture);

          if (screenCapture.RegionViewModels == null || !screenCapture.RegionViewModels.Any())
          {
            Thread.Sleep(_idleHaltTimeout);
            continue;
          }

          // Convert to image file type needed for processing
          var screenBitmap = ConvertStreamToBitmap(screenCapture.ScreenMemoryStream);

          foreach (var region in screenCapture.RegionViewModels)
          {
            var croppedImage = CropBitmap(screenBitmap.StreamSource, region.BoundingBox);
            var scrappedText = ScrapeImage(croppedImage);

            ProcessTriggers(region.Triggers, scrappedText);
          }

          if (!AutoReset)
          {
            IsRunning = false;
            break;
          }
        }
      });
    }

    private string ScrapeImage(Bitmap image)
    {
      MemoryStream memoryStream = new MemoryStream();
      image.Save(memoryStream, ImageFormat.Bmp);

      return _ocrService.ProcessImage(memoryStream);
    }

    private void ProcessTriggers(IList<TriggerViewModel> triggers, string scrappedText)
    {
      foreach(var trigger in triggers)
      {
        //TODO: This needs to eventually tap into a trigger factory.
        if (scrappedText.Contains(trigger.ThresholdValue))
        {
          ThreadPool.QueueUserWorkItem(ignoredState =>
          {
            using (var player = new SoundPlayer(trigger.SoundFilePath))
            {
              player.PlaySync();
            }
          });
        }
      }
    }

    private Bitmap CropBitmap(Stream bitmapStream, Rectangle boundingBox)
    {
      Image image = Image.FromStream(bitmapStream);
      Bitmap croppedBitmap = new Bitmap(boundingBox.Width, boundingBox.Height);

      using (Graphics graphics = Graphics.FromImage(croppedBitmap))
      {
        graphics.DrawImage(image, new Rectangle(0, 0, croppedBitmap.Width, croppedBitmap.Height), boundingBox,
                         GraphicsUnit.Pixel);
      }

      return croppedBitmap;
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