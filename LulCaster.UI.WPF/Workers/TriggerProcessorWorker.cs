using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers.EventArguments;
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
using System.Linq;
using LulCaster.UI.WPF.Utility;
using System.Diagnostics;

namespace LulCaster.UI.WPF.Workers
{
  public class TriggerProcessorWorker : LulWorkerBase
  {
    public event EventHandler<ScreenCaptureProgressArgs> ProgressChanged;

    private const int LOOP_HALT_TIMEOUT = 500;
    private int _idleHaltTimeout = 2000;
    private Task _workerTask;
    private IOcrService _ocrService = new OcrService();
    private ConcurrentQueue<ScreenCapture> _processingQueue { get; set; } = new ConcurrentQueue<ScreenCapture>();

    public void EnqueueScreenCapture(ScreenCapture screenCapture)
    {
      _processingQueue.Enqueue(screenCapture);
    }

    protected override void DoWork()
    {
      _workerTask = Task.Run(() =>
      {
        while (IsRunning)
        {
          if (_processingQueue.IsEmpty)
          {
            Thread.Sleep(_idleHaltTimeout);
            continue;
          }

          _processingQueue.TryDequeue(out var screenCapture);

          using (screenCapture)
          {
            if (screenCapture.RegionViewModels == null || !screenCapture.RegionViewModels.Any())
            {
              throw new InvalidOperationException("Region must be selected before queuing screenshots for processing.");
            }

            foreach (var region in screenCapture.RegionViewModels)
            {
              if (region.BoundingBox.Width < 1 || region.BoundingBox.Height < 1)
                continue;

              using (var croppedImage = CropBitmap(screenCapture, region.BoundingBox))
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
                ProcessTriggers(region.Triggers, screenCapture, scrappedText);
              }
            }
          }

          if (!AutoReset)
          {
            IsRunning = false;
            break;
          }

          Thread.Sleep(LOOP_HALT_TIMEOUT);
        }
      });
    }

    private string ScrapeImage(Bitmap image)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        image.Save(memoryStream, ImageFormat.Tiff);

        return _ocrService.ProcessImage(memoryStream);
      }
    }

    private void ProcessTriggers(IList<TriggerViewModel> triggers, ScreenCapture screenCapture, string scrappedText)
    {
      foreach(var trigger in triggers)
      {
        //TODO: This needs to eventually tap into a trigger factory.
        if (scrappedText.Contains(trigger.ThresholdValue))
        {
          Console.WriteLine($"Seconds before trigger was processed: {DateTime.Now.Subtract(screenCapture.CreationTime).TotalSeconds}");
          trigger.SoundFile.Play();
        }
      }
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