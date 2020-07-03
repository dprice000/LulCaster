using System.Runtime.InteropServices;
using System.Drawing;
using System;

namespace LulCaster.Utility.ScreenCapture.Windows.Snipping
{
  public class BoundingBoxBrush
  {
    [DllImport("User32.dll")]
    public static extern IntPtr GetDC(IntPtr hwnd);
    [DllImport("User32.dll")]
    public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

    public void DrawRectangle()
    {
      IntPtr desktopPtr = GetDC(IntPtr.Zero);
      Graphics graphics = Graphics.FromHdc(desktopPtr);

      SolidBrush brush = new SolidBrush(Color.Blue);
      Pen pen = new Pen(brush);

      graphics.DrawRectangle(pen, new Rectangle(0, 0, 1920, 1080));

      graphics.Dispose();
      ReleaseDC(IntPtr.Zero, desktopPtr);
    }
  }
}