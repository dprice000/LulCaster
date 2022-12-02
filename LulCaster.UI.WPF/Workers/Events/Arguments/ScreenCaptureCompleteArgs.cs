using LulCaster.UI.WPF.Utility;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace LulCaster.UI.WPF.Workers.Events.Arguments
{
  internal class ScreenCaptureCompletedArgs : IDisposable
  {
    private byte[] _byteArray;
    private MemoryStream _memoryStream;

    public byte[] ByteArray
    {
      get => _byteArray;
      set
      {
        _byteArray = value;
        _memoryStream = new MemoryStream(_byteArray);
        BitmapImage = BitmapHelper.ConvertStreamToBitmap(_memoryStream);
      }
    }

    public MemoryStream MemoryStream => _memoryStream;

    public BitmapImage BitmapImage { get; private set; }
    public Rectangle ScreenBounds { get; set; }
    public System.Windows.Size CanvasBounds { get; set; }
    public bool HasBeenProcessed, HasBeenDrawn;

    public void Dispose()
    {
      BitmapImage = null;
      _memoryStream?.Dispose();
    }
  }
}