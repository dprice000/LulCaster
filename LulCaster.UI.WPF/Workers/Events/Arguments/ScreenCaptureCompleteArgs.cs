using System.Drawing;

namespace LulCaster.UI.WPF.Workers.Events.Arguments
{
    internal class ScreenCaptureCompletedArgs
    {
        public byte[] ScreenImageStream { get; set; }
        public Rectangle ScreenBounds { get; set; }
        public System.Windows.Size CanvasBounds { get; set; }
    }
}