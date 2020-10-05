using LulCaster.Utility.Common.Config;

namespace LulCaster.Utility.ScreenCapture.Windows
{
  public interface IScreenCaptureService
  {
    ScreenOptions ScreenOptions { get; set; }
    byte[] CaptureScreenshot();
  }
}