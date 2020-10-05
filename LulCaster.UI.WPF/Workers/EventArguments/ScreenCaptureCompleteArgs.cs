using System.Drawing;

namespace LulCaster.UI.WPF.Workers.EventArguments
{
    internal class ScreenCaptureCompletedArgs
    {
        public byte[] ScreenImageStream { get; set; }
        public Rectangle ScreenBounds { get; set; }
    }
}