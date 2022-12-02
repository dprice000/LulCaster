using LulCaster.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace LulCaster.UI.WPF.Workers
{
  public class ScreenCapture : IDisposable
  {
    private MemoryStream _memoryStream;
    private byte[] _byteArray;

    public byte[] ByteArray
    {
      get => _byteArray;
      set
      {
        _byteArray = value;
        _memoryStream = new MemoryStream(_byteArray);
      }
    }
    public MemoryStream MemoryStream => _memoryStream;
    public BitmapImage ScreenBitmap { get; set; }
    public IList<RegionViewModel> RegionViewModels { get; set; }
    public Rectangle ScreenBounds { get; set; }
    public System.Windows.Size CanvasBounds { get; set; }
    public DateTime CreationTime { get; set; }

    public void Dispose()
    {
      ScreenBitmap = null;
      _memoryStream?.Dispose();
    }
  }
}