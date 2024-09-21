using LulCaster.Utility.Common.Config;
using System;
using System.Drawing;
using System.IO;


namespace LulCaster.Utility.ScreenCapture.Windows
{
  public class ScreenCaptureService : IScreenCaptureService
  {
    public ScreenOptions ScreenOptions { get; set; } = new ScreenOptions();

    public ScreenCaptureService(ScreenOptions screenOptions)
    {
      ScreenOptions = screenOptions;
    }

    public Bitmap CaptureScreenshot()
    {
      using (Bitmap screencapImage = new Bitmap(ScreenOptions.ScreenWidth, ScreenOptions.ScreenHeight))
      using (Graphics graphic = Graphics.FromImage(screencapImage))
      {
        graphic.CopyFromScreen(ScreenOptions.X,
                         ScreenOptions.Y,
                         0, 0,
                         screencapImage.Size,
                         CopyPixelOperation.SourceCopy);

        return screencapImage;
      }
    }
  }
}