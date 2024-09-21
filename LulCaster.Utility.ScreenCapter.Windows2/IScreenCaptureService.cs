using LulCaster.Utility.Common.Config;
using System.Drawing;

namespace LulCaster.Utility.ScreenCapture.Windows
{
  public interface IScreenCaptureService
  {
    ScreenOptions ScreenOptions { get; set; }
    Bitmap CaptureScreenshot();
  }
}