using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace LulCaster.UI.WPF.Workers.Events.Arguments
{
  internal class ScreenCaptureCompletedArgs : IDisposable
  {
    public Bitmap Image { get; set; }
    public BitmapImage BitmapImage { get; private set; }
    public Rectangle ScreenBounds { get; set; }
    public System.Windows.Size CanvasBounds { get; set; }
    public bool HasBeenProcessed, HasBeenDrawn;

    public void Dispose()
    {
      BitmapImage = null;
      Image.Dispose();
    }
  }
}