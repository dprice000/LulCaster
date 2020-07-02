using LulCaster.Utility.Common.Config;
using System.Drawing;
using WpfScreenHelper;
using System.Windows;

namespace LulCaster.Utility.ScreenCapture.Windows
{
  public class ScreenCap
  {
    private ScreenOptions _screenOptions;

    public ScreenCap()
    {
      InitializeBounds();
    }

    public ScreenCap(ScreenOptions screenOptions)
    {
      _screenOptions = screenOptions;
    }

    public void InitializeBounds()
    {
            Rectangle x = new Rectangle();
      _screenOptions.ScreenHeight = (int)Screen.PrimaryScreen.;
      _screenOptions.ScreenWidth = (int)Screen.PrimaryScreen.WorkingArea.Width;
      _screenOptions.X = (int)Screen.PrimaryScreen.Bounds.X;
      _screenOptions.Y = (int)Screen.PrimaryScreen.Bounds.Y;
    }

    public Bitmap CaptureScreenshot()
    {
      using (Bitmap screencapImage = new Bitmap(_screenOptions.ScreenWidth,
                                            _screenOptions.ScreenHeight))
      using (Graphics graphic = Graphics.FromImage(screencapImage))
      {
        graphic.CopyFromScreen(_screenOptions.X,
                         _screenOptions.Y,
                         0, 0,
                         screencapImage.Size,
                         CopyPixelOperation.SourceCopy);

        return new Bitmap(_screenOptions.X, _screenOptions.Y, graphic);
      }
    }
  }
}