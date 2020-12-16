using LulCaster.UI.WPF.Utility;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers.EventArguments;
using LulCaster.Utility.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;

namespace LulCaster.UI.WPF.Workers
{
  public class TriggerProcessorWorker : LulWorkerBase
  {
    public event EventHandler<ScreenCaptureProgressArgs> ProgressChanged;
    public event EventHandler<TriggerViewModel> TriggerActivated;

    private const int LOOP_HALT_TIMEOUT = 100;
    private int _idleHaltTimeout = 500;
    private IOcrService _ocrService = new OcrService();
    private readonly ConcurrentQueue<ScreenCapture> _processingQueue = new ConcurrentQueue<ScreenCapture>();

    public TriggerProcessorWorker(ConcurrentQueue<ScreenCapture> processingQueue)
    {
      _processingQueue = processingQueue;
    }

    protected override void DoWork()
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
      foreach (var trigger in triggers)
      {
        //TODO: This needs to eventually tap into a trigger factory.
        if (scrappedText.Contains(trigger.ThresholdValue))
        {
          Console.WriteLine($"Seconds before trigger was processed: {DateTime.Now.Subtract(screenCapture.CreationTime).TotalSeconds}");
          
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

    private void OnTriggerActivated(TriggerViewModel trigger)
    {
      TriggerActivated?.Invoke(this, trigger);
    }
  }
}