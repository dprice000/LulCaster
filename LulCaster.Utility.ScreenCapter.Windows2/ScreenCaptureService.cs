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

    public ScreenCaptureService(ScreenOptions screenOptions)
    {
      ScreenOptions = screenOptions;
    }

    public void CaptureScreenshot(ref byte[] byteImage)
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

          byteImage = new Byte[memoryStream.Length];
          byteImage = memoryStream.ToArray();
        }
      }
    }
  }
}