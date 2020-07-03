using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace LulCaster.Utility.ScreenCapture.Windows.Snipping
{
  public class BoundingBoxBrush
  {
    [DllImport("User32.dll")]
    public static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("User32.dll")]
    public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

    private Point startPosition;
    private Rectangle boundingRectangle;

    public Image Image { get; set; }

    public Rectangle OnMouseDown(MouseButtonEventArgs e)
    {
      // Start the snip on mouse down
      if (e.LeftButton == MouseButtonState.Released) return new Rectangle();

      var mousePoint = e.GetPosition(e.Device.Target);
      startPosition = new Point((int)mousePoint.X, (int)mousePoint.Y);
      return new Rectangle(new Point((int)startPosition.X, (int)startPosition.Y), new Size(0, 0));
    }

    public Rectangle OnMouseMove(MouseEventArgs e)
    {
      // Modify the selection on mouse move
      if (e.LeftButton == MouseButtonState.Released) return new Rectangle();

      var mousePoint = e.GetPosition(e.Device.Target);
      int x1 = (int)Math.Min(mousePoint.X, startPosition.X);
      int y1 = (int)Math.Min(mousePoint.Y, startPosition.Y);
      int x2 = (int)Math.Max(mousePoint.X, startPosition.X);
      int y2 = (int)Math.Max(mousePoint.Y, startPosition.Y);
      return new Rectangle(x1, y1, x2 - x1, y2 - y1);
    }

    public void DrawRectangle()
    {
      IntPtr desktopPtr = GetDC(IntPtr.Zero);
      Graphics graphics = Graphics.FromHdc(desktopPtr);

      SolidBrush brush = new SolidBrush(Color.LightBlue);
      Pen pen = new Pen(brush);

      graphics.DrawRectangle(pen, boundingRectangle);

      graphics.Dispose();
      ReleaseDC(IntPtr.Zero, desktopPtr);
    }

    public System.Windows.Shapes.Rectangle ConvertRectToWindowsRect(System.Drawing.Rectangle rectangle)
    {
      return new System.Windows.Shapes.Rectangle() { Width = rectangle.Width, Height = rectangle.Height, Fill = System.Windows.Media.Brushes.LightBlue, StrokeThickness = 2 };
    }
  }
}