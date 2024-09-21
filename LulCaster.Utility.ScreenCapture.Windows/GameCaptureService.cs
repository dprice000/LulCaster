using LulCaster.Utility.Common.Config;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using WpfScreenHelper;

namespace LulCaster.Utility.ScreenCapture.Windows
{
  public class GameCaptureService : IScreenCaptureService
  {
    private object _gameHandleLock = new object();
    public ScreenOptions ScreenOptions { get; set; } = new ScreenOptions();
    private IntPtr _processPtr;

    private IntPtr ProcessPtr
    {
      get
      {
        lock (_gameHandleLock)
        {
          return _processPtr;
        }
      }
      set
      {
        lock (_gameHandleLock)
        {
          _processPtr = value;
        }
      }
    }

    public GameCaptureService()
    {
      InitializeBounds();
    }

    public GameCaptureService(ScreenOptions screenOptions)
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

    public void SetProcessPointer(IntPtr processPtr)
    {
      ProcessPtr = processPtr;
    }

    /// <summary>
    /// Gets Image object containing screen shot
    /// </summary>
    /// <param name="handle">The handle to the window. 
    /// <returns></returns>
    public Bitmap CaptureScreenshot()
    {
      if (ProcessPtr == null)
      {
        return null;
      }

      var hdcSrc = User32.GetWindowDC(ProcessPtr);
      var windowRect = new User32.Rect();
      User32.GetWindowRect(ProcessPtr, ref windowRect);
      var destinationPtr = Gdi32.CreateCompatibleDC(hdcSrc);
      var bitmapPtr = Gdi32.CreateCompatibleBitmap(hdcSrc, ScreenOptions.ScreenWidth, ScreenOptions.ScreenHeight);
      var hOld = Gdi32.SelectObject(destinationPtr, bitmapPtr);

      try
      {
        Gdi32.BitBlt(destinationPtr, 0, 0, ScreenOptions.ScreenWidth, ScreenOptions.ScreenHeight, hdcSrc, 0, 0, Gdi32.Srccopy);
        Gdi32.SelectObject(destinationPtr, hOld);

        var screenCaptureImage = Image.FromHbitmap(bitmapPtr);
        var bitmap = new Bitmap(screenCaptureImage);

        return bitmap;
      }
      finally
      {
        Gdi32.DeleteDC(destinationPtr);
        User32.ReleaseDC(ProcessPtr, hdcSrc);
        Gdi32.DeleteObject(bitmapPtr);
        Gdi32.DeleteObject(hOld);
      }
    }

    /// <summary>
    /// Gdi32 API functions
    /// </summary>
    private class Gdi32
    {
      public const int Srccopy = 0x00CC0020; // BitBlt dwRop parameter

      [DllImport("gdi32.dll")]
      public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
          int nWidth, int nHeight, IntPtr hObjectSource,
          int nXSrc, int nYSrc, int dwRop);

      [DllImport("gdi32.dll")]
      public static extern IntPtr CreateCompatibleDC(IntPtr hDc);

      [DllImport("gdi32.dll")]
      public static extern IntPtr CreateCompatibleBitmap(IntPtr hDc, int nWidth,
          int nHeight);

      [DllImport("gdi32.dll")]
      public static extern IntPtr SelectObject(IntPtr hDc, IntPtr hObject);

      [DllImport("gdi32.dll")]
      public static extern bool DeleteDC(IntPtr hDc);

      [DllImport("gdi32.dll")]
      public static extern bool DeleteObject(IntPtr hObject);

    }

    /// <summary>
    /// User32 API functions
    /// </summary>
    private class User32
    {
      [StructLayout(LayoutKind.Sequential)]
      public struct Rect
      {
        public readonly int top;
        public readonly int left;
        public readonly int bottom;
        public readonly int right;
      }
      [DllImport("user32.dll")]
      public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

      [DllImport("user32.dll")]
      public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

      [DllImport("user32.dll")]
      public static extern IntPtr GetWindowDC(IntPtr hWnd);
    }
  }
}