using LulCaster.Utility.Common.Config;
using System;
using System.Drawing;
using System.IO;
using WpfScreenHelper;

namespace LulCaster.Utility.ScreenCapture.Windows
{
  public class ScreenCaptureService : IScreenCaptureService
  {
    public ScreenOptions ScreenOptions { get; set; } = new ScreenOptions();

    public ScreenCaptureService()
    {
      InitializeBounds();
    }

    public ScreenCaptureService(ScreenOptions screenOptions)
    {
      ScreenOptions = screenOptions;
    }

    private void InitializeBounds()
    {
      ScreenOptions.ScreenHeight = 2160; //TODO: This should not be hardcoded
      ScreenOptions.ScreenWidth = 3840;
      ScreenOptions.X = (int)Screen.PrimaryScreen.Bounds.X;
      ScreenOptions.Y = (int)Screen.PrimaryScreen.Bounds.Y;
    }

    public byte[] CaptureScreenshot()
    {
      using (Bitmap screencapImage = new Bitmap(ScreenOptions.ScreenWidth, ScreenOptions.ScreenHeight))
      using (Graphics graphic = Graphics.FromImage(screencapImage))
      {
        graphic.CopyFromScreen(ScreenOptions.X,
                         ScreenOptions.Y,
                         0, 0,
                         screencapImage.Size,
                         CopyPixelOperation.SourceCopy);

        using (var memoryStream = new MemoryStream())
        {
          screencapImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Tiff);

          byte[] byteImage = new Byte[memoryStream.Length];
          byteImage = memoryStream.ToArray();

          return byteImage;
        }
      }
    }
  }
}