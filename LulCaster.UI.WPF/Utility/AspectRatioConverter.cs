using System;
using System.Drawing;

namespace LulCaster.UI.WPF.Utility
{
  public static class AspectRatioConverter
  {
    public static Rectangle ResolveScreenLocation(Rectangle screenBounds, System.Windows.Size canvasBounds, Rectangle regionBounds)
    {
      var x = regionBounds.X / canvasBounds.Width;
      var y = regionBounds.Y / canvasBounds.Height;
      var width = regionBounds.Width / canvasBounds.Width;
      var height = regionBounds.Height / canvasBounds.Height;

      return new Rectangle
      {
        X = Convert.ToInt32(x * screenBounds.Width),
        Y = Convert.ToInt32(y * screenBounds.Height),
        Width = Convert.ToInt32(width * screenBounds.Width),
        Height = Convert.ToInt32(height * screenBounds.Height)
      };
    }
  }
}