using LulCaster.Utility.Common.Config;
using System;
using System.Drawing;
using System.IO;
using WpfScreenHelper;

namespace LulCaster.Utility.ScreenCapture.Windows
{
  public class ScreenCaptureService : IScreenCaptureService
  {
    private ScreenOptions _screenOptions;

    public ScreenCaptureService()
    {
      InitializeBounds();
    }

    public ScreenCaptureService(ScreenOptions screenOptions)
    {
      _screenOptions = screenOptions;
    }

    private void InitializeBounds()
    {
      _screenOptions = new ScreenOptions();
      _screenOptions.ScreenHeight = 2160; //TODO: This should not be hardcoded
      _screenOptions.ScreenWidth = 3840;
      _screenOptions.X = (int)Screen.PrimaryScreen.Bounds.X;
      _screenOptions.Y = (int)Screen.PrimaryScreen.Bounds.Y;
    }

    public byte[] CaptureScreenshot()
    {
      using (Bitmap screencapImage = new Bitmap(_screenOptions.ScreenWidth, _screenOptions.ScreenHeight))
      using (Graphics graphic = Graphics.FromImage(screencapImage))
      {
        graphic.CopyFromScreen(_screenOptions.X,
                         _screenOptions.Y,
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